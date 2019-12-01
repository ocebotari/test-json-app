using System;
using System.Diagnostics;

namespace CrazyApp
{
    class Program
    {
        const string TheAFile = @"C:\Users\ocebotar\Documents\20191022 Technical Assessment Global Blue\A.json";
        const string TheBFile = @"C:\Users\ocebotar\Documents\20191022 Technical Assessment Global Blue\B.json";

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
