using System;
using System.Threading.Tasks;

namespace AsyncAwaitConsoleTest
{
    class Program
    {
        async static Task Main(string[] args)
        {
            string s1 = null;
            var s2 = "stringa";
            if (s1 == s2)
            {
                Console.WriteLine("Le stringhe sono uguali");
            }
            var lr = new LongRunning();
            var stringa1 = await lr.GetString();
            Console.WriteLine(stringa1);

            Console.WriteLine("Stringa finale");

            Console.ReadKey();
        }
    }

    public class LongRunning
    {
        public async Task<string> GetString(string s = "")
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
