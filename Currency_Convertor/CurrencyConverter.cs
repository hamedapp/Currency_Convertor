using QuickGraph;
using QuickGraph.Algorithms;
using System;
using System.Collections.Generic;

namespace Currency_Convertor
{
    public class CurrencyConverter: ICurrencyConverter
    {
        private static readonly CurrencyConverter instance = new CurrencyConverter();

        private CurrencyConverter()
        {
        }
        public static CurrencyConverter Instance
        {
            get
            {
                return instance;
            }
        }

        private readonly AdjacencyGraph<string, Edge<string>> _graph = new AdjacencyGraph<string, Edge<string>>();
        private readonly Dictionary<Edge<string>, double> _costs = new Dictionary<Edge<string>, double>();

        private void AddEdgeWithCosts(string source, string target, double cost)
        {
            var edge = new Edge<string>(source, target);

            _graph.AddVerticesAndEdge(edge);
            _costs.Add(edge, cost);
        }

        public void ClearConfiguration()
        {
            _graph.Clear();
            _costs.Clear();
        }

        public double Convert(string fromCurrency, string toCurrency, double amount)
        {
            CheckPathFromGraph(fromCurrency, toCurrency, out bool isPathExist, out IEnumerable<Edge<string>> path);

            if (isPathExist)
            {
                return CalculateAmountFromGraph(amount, path); ;
            }
            else
            {
                Console.WriteLine("No path found from {0} to {1}.");
                return -1;
            }
        }

        private void CheckPathFromGraph(string fromCurrency, string toCurrency, out bool isPathExist, out IEnumerable<Edge<string>> path)
        {
            var edgeCost = AlgorithmExtensions.GetIndexer(_costs);
            var tryGetPath = _graph.ShortestPathsDijkstra(edgeCost, fromCurrency);

            isPathExist = tryGetPath(toCurrency, out path);
        }

        private double CalculateAmountFromGraph(double amount, IEnumerable<Edge<string>> path)
        {
            double finalAmount = amount;

            foreach (var item in path)
            {
                var cost = _costs[item];
                finalAmount *= cost;
            }

            return finalAmount;
        }

        public void UpdateConfiguration(IEnumerable<ValueTuple<string, string, double>> conversionRates)
        {
            foreach (var item in conversionRates)
            {
                bool rateExist = _graph.TryGetEdge(item.Item1, item.Item2, out Edge<string> prevEdge);

                if (rateExist)
                {
                    _graph.RemoveEdge(prevEdge);

                    AddEdgeWithCosts(item.Item1, item.Item2, item.Item3);

                    _costs[prevEdge] = item.Item3;

                    continue;
                }

                AddEdgeWithCosts(item.Item1, item.Item2, item.Item3);
            }
        }
    }
}
