using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyApp
{
    public interface ILogger
    {
        void Write(string value);

        void WriteLine(string value);
    }

    public class ConsoleLog : ILogger
    {
        public void Write(string value)
        {
            Console.Write(value);
        }

        public void WriteLine(string value)
        {
            Console.WriteLine(value);
        }
    }
}
