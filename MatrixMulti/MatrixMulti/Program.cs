using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPI;

namespace MM
{
    class Program
    {
        public static int procid, numproc, start, finish, n = 4, size, i, j, k;
        public static int[,] a = { { 1, 1, 1, 1 },
                                   { 1, 1, 1, 1 },
                                   { 1, 1, 1, 1 },
                                   { 1, 1, 1, 1 }  };

        public static int[,] b = { { 1, 0, 0, 0 },
                                   { 0, 1, 0, 0 },
                                   { 0, 0, 1, 0 },
                                   { 0, 0, 0, 1 }  };
        public static int[,] c = { { 0, 0, 0, 0},
                                   { 0, 0, 0, 0},
                                   { 0, 0, 0, 0},
                                   { 0, 0, 0, 0}};
        static void Main(string[] args)
        {
            using (new MPI.Environment(ref args))
            {
                Intracommunicator comm = Communicator.world;
                procid = comm.Rank;
                numproc = comm.Size;
                comm.Barrier();
                matrixM(a, b, c, comm);
                // PrintMatrix(c);


            }
        }
        public static void PrintMatrix(int[,] c)
        {
            if (procid == 0)
            {
                for (i = 0; i < n; ++i)
                {
                    for (j = 0; j < n; ++j)
                    {
                        Console.WriteLine("rank " + procid + " c[" + i + "][" + j + "] = " + c[i, j]);
                    }
                }
            }
        }
        public static void matrixM(int[,] a, int[,] b, int[,] c, Intracommunicator comm)
        {
            int[] d = new int[16];
            for (i = 0; i < 16; i++)
            {
                d[i] = 0;
                Console.WriteLine(d[k]);

            }
            /*size = n / numproc;
            start = procid * size;
            finish = (procid + 1) * size;*/
            int root = 0;
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


            Console.WriteLine("proc with rank " + procid + " is getting from " + start + " to " + stop);
            /*Console.WriteLine("start = "+ start);
            Console.WriteLine("start = " + stop);*/
            for (i = start; i <= stop; ++i)
            {
                //Console.WriteLine("here");
                for (j = 0; j < n; ++j)
                {
                    for (k = 0; k < n; ++k)
                    {
                        c[i, j] += a[i, k] * b[k, j];
                        //Console.WriteLine("rack "+procid+" c[" + i + "][" + j + "] = " + c[i, j]);
                        comm.Allgather<int>(c[i, j], ref d);
                    }
                }

            }
            for (k = 0; k < n; k++)
            {
                Console.WriteLine("d[ " + k + "] = " + d[k]);
            }
            // comm.Gather<int[,]>(c,root);
            comm.Barrier();

        }

    }
}