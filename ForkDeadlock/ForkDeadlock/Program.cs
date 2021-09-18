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

            Fork nextFork(ref int fork)
            {
                fork = (fork + 1) % forks.Length;
                return forks[fork];
            }

            int leftFork = 4;
            int rightFork = 0;

            Dude[] dudes =
            {
                new Dude(nextFork(ref leftFork), nextFork(ref rightFork), "Gustav"),
                new Dude(nextFork(ref leftFork), nextFork(ref rightFork), "Kevin"),
                new Dude(nextFork(ref leftFork), nextFork(ref rightFork), "Rasmus"),
                new Dude(nextFork(ref leftFork), nextFork(ref rightFork), "Niels"),
                new Dude(nextFork(ref leftFork), nextFork(ref rightFork), "Frank"),
            };

            Thread[] threads = new Thread[dudes.Length];

            for (int i = 0; i < dudes.Length; i++)
                threads[i] = new Thread(new ThreadStart(dudes[i].Eat));

            foreach (Thread thread in threads)
                thread.Start();

            static void ClearCurrentConsoleLine()  // Stolen from StackOverflow; somewhere...
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
