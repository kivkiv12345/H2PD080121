using System;
using System.Threading;

namespace ForkDeadlock
{
    class Program
    {
        static void Main(string[] args)
        {
            Fork[] forks =
            {
                new Fork(),
                new Fork(),
                new Fork(),
                new Fork(),
                new Fork(),
            };

            Dude[] dudes =
            {
                new Dude(forks[4], forks[0], "Gustav"),
                new Dude(forks[0], forks[1], "Kevin"),
                new Dude(forks[1], forks[2], "Rasmus"),
                new Dude(forks[2], forks[3], "Niels"),
                new Dude(forks[3], forks[4], "Frank"),
            };

            Thread[] threads = new Thread[5];

            for (int i = 0; i < dudes.Length; i++)
                threads[i] = new Thread(new ThreadStart(dudes[i].Eat));

            foreach (Thread thread in threads)
                thread.Start();

            static void ClearCurrentConsoleLine()
            {
                int currentLineCursor = Console.CursorTop;
                Console.SetCursorPosition(0, Console.CursorTop);
                Console.Write(new string(' ', Console.WindowWidth));
                Console.SetCursorPosition(0, currentLineCursor);
            }

            while (true)
            {
                for (int i = 0; i < dudes.Length; i++)
                {
                    Console.SetCursorPosition(0, i);
                    ClearCurrentConsoleLine();
                    Console.WriteLine($"{dudes[i].name}\t is {(dudes[i].isEating ? "" : "not")} eating");
                }

                /*foreach (Dude dude in dudes)
                {
                    Console.WriteLine($"{dude.name}\t is {(dude.isEating ? "" : "not")} eating");
                }*/
                Thread.Sleep(200);
                //Console.Clear();
            }

            foreach (Thread thread in threads)
            {
                thread.Join();
            }
            
        }
    }
}
