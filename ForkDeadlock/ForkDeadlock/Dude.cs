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

        public Dude(Fork leftFork, Fork rightFork, string name)
        {
            this.leftFork = leftFork;
            this.rightFork = rightFork;
            this.name = name;
        }

        /// <summary>
        /// Waits until fork becomes available, then takes it.
        /// </summary>
        private void aquireFork(Fork fork)
        {
            lock (fork)
            {
                // TODO Kevin: This 'while' probably doesn't make sense, when the fork is locked.
                while (fork.inUseBy != this)
                    if (fork.inUseBy == null)
                        fork.inUseBy = this;
            }
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

                //this.aquireFork(this.leftFork);
                //this.aquireFork(this.rightFork);

                //if (!(this.tryGetFork(this.leftFork) && this.tryGetFork(this.rightFork)))
                //{
                //    //Console.WriteLine($"{this.name} cant eat");
                //    this.leftFork.inUseBy = null;  // The left fork may have been grabbed when we get here, so we release it.
                //    continue;
                //}
                    

                if (!this.tryGetFork(this.leftFork))
                {
                    //Console.WriteLine($"{this.name} cant get first fork");
                    continue;
                }
                    
                if (!this.tryGetFork(this.rightFork))
                {
                    this.leftFork.inUseBy = null;
                    //Console.WriteLine($"{this.name} couldn't get second fork");
                    continue;
                }
                    

                this.isEating = true;

                Thread.Sleep(new Random().Next(250, 500));

                leftFork.inUseBy = null;
                rightFork.inUseBy = null;

                this.isEating = false;

            }
        }
    }
}
