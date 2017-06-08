using NMF.Models.Repository;
using NMF.Utilities;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using TTC2017.StateElimination.Transitiongraph;

namespace TTC2017.StateElimination
{
    class Program
    {
        static void Main(string[] args)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var regex = CalculateRegex(args[0]);
            stopwatch.Stop();
            File.WriteAllText(args[1], regex);
            Console.WriteLine(stopwatch.ElapsedMilliseconds);
        }

        private static string CalculateRegex(string path)
        {
            var repository = new ModelRepository();
            var transitionGraph = repository.Resolve(path).RootElements[0] as TransitionGraph;

            var initial = transitionGraph.States.FirstOrDefault(s => s.IsInitial);
            if (initial.Incoming.Count > 0)
            {
                var newInitial = new State { IsInitial = true };
                transitionGraph.Transitions.Add(new Transition
                {
                    Source = newInitial,
                    Target = initial
                });
                initial = newInitial;
            }
            var final = CreateFinal(transitionGraph);

            foreach (var s in transitionGraph.States.OrderBy(s => s.Incoming.Count * s.Outgoing.Count).ToArray())
            {
                if (s == initial || s == final) continue;

                var selfEdge = string.Join("+", from edge in s.Outgoing
                                                where edge.Target == s
                                                select edge.Label);

                if (!string.IsNullOrEmpty(selfEdge)) selfEdge = string.Concat("(", selfEdge, ")*");

                foreach (var incoming in s.Incoming.Where(t => t.Source != s))
                {
                    if (incoming.Source == null) continue;
                    foreach (var outgoing in s.Outgoing.Where(t => t.Target != s))
                    {
                        if (outgoing.Target == null) continue;
                        var transition = incoming.Source.Outgoing.FirstOrDefault(t => t.Target == outgoing.Target);
                        if (transition == null)
                        {
                            transitionGraph.Transitions.Add(new Transition
                            {
                                Source = incoming.Source,
                                Target = outgoing.Target,
                                Label = incoming.Label + selfEdge + outgoing.Label
                            });
                        }
                        else
                        {
                            transition.Label = string.Concat("(", transition.Label, "+", incoming.Label, selfEdge, outgoing.Label, ")");
                        }
                    }
                }

                s.Delete();
            }

            return string.Join("+", from edge in initial.Outgoing where edge.Target == final select edge.Label);
        }

        private static IState CreateFinal(TransitionGraph transitionGraph)
        {
            var finalStates = transitionGraph.States.Where(s => s.IsFinal).ToList();
            if (finalStates.Count == 1 && finalStates[0].Outgoing.Count == 0)
            {
                return finalStates[0];
            }
            else
            {
                var newFinal = new State();
                foreach (var s in finalStates)
                {
                    transitionGraph.Transitions.Add(new Transition
                    {
                        Source = s,
                        Target = newFinal
                    });
                }
                transitionGraph.States.Add(newFinal);
                return newFinal;
            }
        }
    }
}