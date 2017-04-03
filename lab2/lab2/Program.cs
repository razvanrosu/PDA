using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPI;

namespace lab2
{
    class Program
    {
        static int rank, size;
        static int nrToSearch = 18;
        static int nvalues;
        static int[] array = new int[50];
        static int[] positions = new int[50];
        static int i, j, temp;
        static bool inrange, found;
        static void Main(string[] args)
        {
            using (new MPI.Environment(ref args))
            {
                Intracommunicator world = Communicator.world;
                rank = Communicator.world.Rank;
                size = Communicator.world.Size;
                Communicator.world.Barrier();
                found = false;

                if (rank == 0)
                {
                    for (i = 0; i < 50; ++i)
                    {
                        if (i % 10 == 0)
                        {
                            array[i] = 18;
                        }
                        else
                        {
                            array[i] = i;
                        }
                    }
                }

                Communicator.world.Broadcast<int[]>(ref array, 0);
                Communicator.world.ImmediateReceive<int>(rank, 1);

                nvalues = 50 / size;
                i = rank * nvalues;

                inrange = ((i <= ((rank + 1) * nvalues - 1)) & (i >= rank * nvalues));
                List<int> indexes = new List<int>();
                while (inrange)
                {

                    if (array[i] == nrToSearch)
                    {
                        temp = 23;
                        indexes.Add(i);
                        for (j = 0; j < size; ++j)
                        {
                            Communicator.world.Send<int>(temp, j, 1);
                        }
                        Console.WriteLine("Process: " + rank + " has found number " + array[i] + " at index " + i + "\n");
                        found = true;

                    }
                    ++i;
                    inrange = (i <= ((rank + 1) * nvalues - 1) && i >= rank * nvalues);
                }
                if (!found)
                {
                    Console.WriteLine("The process: " + rank + " stopped index " + (i - 1) + "\n");
                }

                int max = -1;
                for(i = 0; i< indexes.Count; i++)
                {
                    if(indexes[i] > max)
                    {
                        max = indexes[i];
                    }

                }

                int high = world.Reduce<int>(max, Operation<int>.Max, 0);
                System.Console.Write("The highest index is " + high);
            }

        }

    }

}
