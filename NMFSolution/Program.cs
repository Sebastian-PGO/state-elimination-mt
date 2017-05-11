using NMF.Models.Repository;
using System;
using System.Diagnostics;
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
            Console.WriteLine(CalculateRegex(args[0]));
            stopwatch.Stop();
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

            foreach (var s in transitionGraph.States.ToArray())
            {
                if (s == initial || s == final) continue;

                var selfEdge = string.Join("+", from edge in s.Outgoing
                                                where edge.Target == s
                                                select edge.Label);

                if (!string.IsNullOrEmpty(selfEdge)) selfEdge = $"({selfEdge})*";

                foreach (var incoming in s.Incoming.Where(t => t.Source != s))
                {
                    foreach (var outgoing in s.Outgoing.Where(t => t.Target != s))
                    {
                        transitionGraph.Transitions.Add(new Transition
                        {
                            Source = incoming.Source,
                            Target = outgoing.Target,
                            Label = incoming.Label + selfEdge + outgoing.Label
                        });
                    }
                }

                s.Delete();
            }

            return string.Join("+", from edge in initial.Outgoing where edge.Target == final select edge.Label);
        }

        private static IState CreateFinal(TransitionGraph transitionGraph)
        {
            var finalStates = transitionGraph.States.Where(s => s.IsFinal).ToList();
            if (finalStates.Count == 1)
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