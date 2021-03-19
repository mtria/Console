using System;
using System.Threading.Tasks;

namespace AsyncAwaitConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }

    public class LongRunning
    {
        public async Task<string> GetString()
        {
            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine((i + 1).ToString());
                await Task.Delay(1000);
            }

            return "Ecco la stringa";
        }
    }
}
