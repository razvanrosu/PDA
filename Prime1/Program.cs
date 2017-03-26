using System;
using MPI;
using System.Collections.Generic;
using System.Linq;

namespace MPI1
{
    class Program
    {
        public static bool isPrime(int number)
        {

            if (number == 1) return false;
            if (number == 2) return true;

            for (int i = 2; i <= Math.Ceiling(Math.Sqrt(number)); ++i)
            {
                if (number % i == 0) return false;
            }

            return true;

        }

        public static int processId, nrPerProcessor, start, finish, i, j;

        const int n = 10;


        static void Main(string[] args)
        {

            using (new MPI.Environment(ref args))
            {

                processId = Communicator.world.Rank;
                nrPerProcessor = Communicator.world.Size;
                Communicator.world.Barrier();

                start = 2 + processId * (n - 1) / nrPerProcessor;
                finish = 1 + (processId + 1) * (n - 1) / nrPerProcessor;

                Intracommunicator world = Communicator.world;

                int nrOfPrimeNumbers = 0;
                for (j = start; j <= finish; j++)
                {
                    if (isPrime(j))
                    {
                        ++nrOfPrimeNumbers;
                        Console.WriteLine("The prime nr {0} has been found", j);
                    }

                }

                if (world.Rank == 0)
                {
                    int totalPrimeNumbers = world.Reduce<int>(nrOfPrimeNumbers, Operation<int>.Add, 0);
                    System.Console.WriteLine("Number of prime numbers found: {0}", totalPrimeNumbers);
                }
                else
                {
                    world.Reduce<int>(nrOfPrimeNumbers, Operation<int>.Add, 0);
                }
            }


        }
    }
}