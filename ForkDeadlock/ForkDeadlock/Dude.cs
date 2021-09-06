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

        private void aquireFork(Fork fork)
        {
            lock (fork)
            {
                while (fork.inUseBy != this)
                    if (fork.inUseBy == null)
                    {
                        fork.inUseBy = this;
                        Thread.Sleep(50);
                    }
            }
        }

        public void Eat()
        {
            while (true)
            {
                Thread.Sleep(new Random().Next(500, 1000));

                Console.WriteLine("Eating");
                this.aquireFork(this.leftFork);
                Console.WriteLine("one fork");
                this.aquireFork(this.rightFork);
                Console.WriteLine("two fork");

                this.isEating = true;

                Thread.Sleep(new Random().Next(500, 1000));

                leftFork.inUseBy = null;
                rightFork.inUseBy = null;

                this.isEating = false;

                //Thread.Sleep(new Random().Next(500, 1000));
            }
        }
    }
}
