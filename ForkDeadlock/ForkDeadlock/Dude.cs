using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ForkDeadlock
{
    class Dude
    {
        private Fork leftFork;
        private Fork rightFork;

        public bool isEating { get; set; }
        public string name { get; set; }
        private bool isPolite { get; set; }

        public Dude(Fork leftFork, Fork rightFork, string name, bool isPolite = false)
        {
            this.leftFork = leftFork;
            this.rightFork = rightFork;
            this.name = name;
            this.isPolite = isPolite;
        }

        /// <summary>
        /// makes a single attempt at grabbing the fork.
        /// </summary>
        /// <returns>True for success, otherwise false.</returns>
        private bool tryGetFork(Fork fork)
        {
            lock (fork)
            {
                if (fork.inUseBy == null)
                {
                    fork.inUseBy = this;
                    return true;
                }
                return false;
            }
        }

        public void Eat()
        {
            while (true)
            {
                Thread.Sleep(new Random().Next(250, 500));

                if (!this.tryGetFork(this.leftFork))
                {
                    //Console.WriteLine($"{this.name} cant get first fork");
                    continue;
                }
                    
                if (!this.tryGetFork(this.rightFork))
                {
                    if (this.isPolite) this.leftFork.inUseBy = null;
                    //Console.WriteLine($"{this.name} couldn't get second fork");
                    continue;
                }
                    

                this.isEating = true;

                Thread.Sleep(new Random().Next(250, 500));

                leftFork.inUseBy = null;
                rightFork.inUseBy = null;

                this.isEating = false;

                Thread.Sleep(new Random().Next(250, 500));

            }
        }
    }
}
