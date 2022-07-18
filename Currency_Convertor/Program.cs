using System;
using System.Collections.Generic;

namespace Currency_Convertor
{
    class Program
    {
        static void Main(string[] args)
        {
            var currenctConverItems = new List<ValueTuple<string, string, double>>
            {
                new ValueTuple<string, string, double>("USD", "CAD", 1.34),
                new ValueTuple<string, string, double>("CAD", "GBP", 0.58),
                new ValueTuple<string, string, double>("USD", "EUR", 0.86),
                new ValueTuple<string, string, double>("USD", "CAD", 1.35),
                new ValueTuple<string, string, double>("EUR", "IRR", 3.86),
            };

            CurrencyConverter convertor = CurrencyConverter.Instance;
            convertor.UpdateConfiguration(currenctConverItems);

            foreach (var item in currenctConverItems)
            {
                Console.WriteLine($"Add {item.Item1} to {item.Item2} with rate {item.Item3} to currency convert graph");
            }

            var finalAmount = convertor.Convert("USD", "IRR", 1000);

            Console.WriteLine($"Converted Value from USD to IRR is {finalAmount}");
            convertor.ClearConfiguration();

            Console.ReadLine();
        }
    }
}
