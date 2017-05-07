using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPI;

namespace Roy_Floyd
{
    class Program
    {
        public const int INF = 99999;
        public static int[,] graph = {
            { 0, 2,  3, 10 },
            { 4, 0,  INF, 7 },
            { INF, INF, 0,   1 },
            { 7, INF, INF, 0 }
        };
        static int procid, numproc, n = 4, j, k, constant = 4;

        static void Main(string[] args)
        {

            k = 0;


            using (new MPI.Environment(ref args))
            {
                Intracommunicator comm = Communicator.world;
                procid = Communicator.world.Rank;
                numproc = Communicator.world.Size;
                FloydWarshall(graph, constant, procid, numproc, comm);

            }

        }

        private static void Print(int[,] distance, int nrOfVertices, int procid)
        {

            for (int i = 0; i < nrOfVertices; ++i)
            {
                for (int j = 0; j < nrOfVertices; ++j)
                {
                    {

                        if (distance[i, j] == INF)
                            Console.Write("INF".PadLeft(7));
                        else
                            Console.Write(distance[i, j].ToString().PadLeft(7));
                    }
                }

            }
        }

        public static void FloydWarshall(int[,] graph, int nrOfVertices, int procid, int numproc, Intracommunicator comm)
        {
            int[,] distance = new int[nrOfVertices, nrOfVertices];

            int count = n / numproc;
            int remainder = n % numproc;
            int start, stop;

            if (procid < remainder)
            {
                start = procid * (count + 1);
                stop = start + count;
            }
            else
            {
                start = procid * count + remainder;
                stop = start + (count - 1);
            }

            for (k = 0; k < nrOfVertices; k++)
            {
                for (int i = start; i <= stop; ++i)
                {
                    for (int j = 0; j < nrOfVertices; ++j)
                    {
                        if (graph[i, k] + graph[k, j] < graph[i, j])
                            graph[i, j] = graph[i, k] + graph[k, j];
                    }
                }
                comm.Allgather<int[,]>(graph);
            }

            //comm.Gather<int[,]>(distance, 0);
            Print(graph, nrOfVertices, procid);

        }
    }
}
