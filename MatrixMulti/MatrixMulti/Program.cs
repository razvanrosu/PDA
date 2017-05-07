using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPI;
namespace MatrixMultiplication
{
    class Program
    {
        public static int rank, numproc, start, finish, n = 4, size, i, j, k;

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
        public static int[,] result = { { 0, 0, 0, 0},
                                   { 0, 0, 0, 0},
                                   { 0, 0, 0, 0},
                                   { 0, 0, 0, 0}};

        public static int[] d = new int[16];

        static void Main(string[] args)
        {
            using (new MPI.Environment(ref args))
            {
                Intracommunicator comm = Communicator.world;
                rank = comm.Rank;
                numproc = comm.Size;
                comm.Barrier();
                Random random = new Random();
                if (rank == 0)
                {
                    for (int i = 0; i < 4; i++)
                        for (int j = 0; j < 4; j++)
                            a[i, j] = random.Next() % 10 + 1;

                    for (int i = 0; i < 4; i++)
                        for (int j = 0; j < 4; j++)
                            b[i, j] = random.Next() % 10 + 1;

                    for (int i = 0; i < 4; i++)
                        for (int j = 0; j < 4; j++)
                            c[i, j] = 0;
                }
                comm.Broadcast(ref a, 0);
                comm.Broadcast(ref b, 0);
                comm.Broadcast(ref c, 0);



                for (int k = rank; k < 4; k = k + numproc)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            c[k, j] = c[k, j] + a[k, i] * b[i, j];
                            
                        }

                    }
                }

                result = comm.Reduce<int[,]>(c, Operation<int[,]>.Add, 0);



                /*
                if (rank == 0)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = 0; j < 4; j++)
                            Console.WriteLine(result[i,j]);
                        Console.WriteLine("\n");
                    }

                }
                */
            }
        }

    }
}