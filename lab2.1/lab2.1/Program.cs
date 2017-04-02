﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPI;

namespace lab21{
    class Program
    {
        static int rank, size;
        static int searchedNr = 2;
        static int nvalues;
        static int[] array = new int[50];
        static List<int> positionsWhereFound = new List<int>();
        static int[] positions = new int[50];
        static int i, j, temp;
        static bool inrange, found;

        static void Main(string[] args)
        {
            using (MPI.Environment environment = new MPI.Environment(ref args))
            {
                Intracommunicator comm = Communicator.world;
                rank = Communicator.world.Rank;
                size = Communicator.world.Size;
                Communicator.world.Barrier();
                found = false;

                if (comm.Rank == 0)
                {
                    int[] numbers = new int[comm.Size];

                    for (int k = 0; k < numbers.Length; k++)
                        numbers[k] = k + 1;

                    int r = comm.Scatter(numbers);

                    Console.WriteLine("Received {0} at {1}", r, comm.Rank);
                }
                else
                {
                    int r = comm.Scatter<int>(0);
                    Console.WriteLine("Received {0} at {1}", r, comm.Rank);
                }

                nvalues = 50 / size;
                i = rank * nvalues;

                inrange = ((i <= ((rank + 1) * nvalues - 1)) & (i >= rank * nvalues));

                while (inrange)
                {
                    int p = 0;
                    if (array[i] == searchedNr)
                    {
                        temp = 23;
                        for (j = 0; j < size; ++j)
                        {
                            Communicator.world.Send<int>(temp, j, 1);
                        }
                        Console.WriteLine("The process: " + rank + "has found " + array[i] + " at index " + i + "\n");
                        found = true;
                    }
                    ++i;
                    inrange = (i <= ((rank + 1) * nvalues - 1) && i >= rank * nvalues);
                }
                if (!found)
                {
                    Console.WriteLine("The process: " + rank + " stopped at index " + (i - 1) + "\n");
                }

            }
        }
    }
}
