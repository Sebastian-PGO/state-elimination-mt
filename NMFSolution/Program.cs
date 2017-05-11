using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NMF.Models.Repository;
using TTC2017.StateElimination.Transitiongraph;
using System.Diagnostics;

namespace TTC2017.StateElimination
{
    class Program
    {
        static void Main(string[] args)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            CalculateRegex(args[0]);
            stopwatch.Stop();
            Console.WriteLine($"Operation took {stopwatch.Elapsed.TotalSeconds}s");
        }

        private static void CalculateRegex(string path)
        {
            var repository = new ModelRepository();
            var transitionGraph = repository.Resolve(path).RootElements[0] as TransitionGraph;

            var initial = transitionGraph.States.FirstOrDefault(s => s.IsInitial);
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
