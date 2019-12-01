using System;
using System.Diagnostics;

namespace CrazyApp
{
    class Program
    {
        const string TheAFile = @"data\A.json";
        const string TheBFile = @"data\B.json";

        static void Main(string[] args)
        {
            Stopwatch sw = Stopwatch.StartNew();

            var rdFile1 = new ReadJsonFile(TheAFile);
            var rdFile2 = new ReadJsonFile(TheBFile);
            var logger = new ConsoleLog();

            var comparator = new JsonFileComparator(rdFile1, rdFile2, logger);
            comparator.DOWork();

            sw.Stop();

            Console.WriteLine("Time elapsed: " + sw.ElapsedMilliseconds);

            Console.ReadKey();
        }        
    }    
    
}
