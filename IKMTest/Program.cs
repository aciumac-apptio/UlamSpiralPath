using System;
using System.Collections.Generic;

namespace UlamSpiralImplementation
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<int, int[]> cellLocation = new Dictionary<int, int[]>();
            int[,] ulamSpiral;
            string line;
            int caseNumber = 1;
            while ((line = Console.ReadLine()) != null)
            {
                string[] split = line.Split(new char[] { ' ' }, StringSplitOptions.None);
                int input1 = int.Parse(split[0]);
                int input2 = int.Parse(split[1]);

                if (IsPrime(input1) || IsPrime(input2))
                {
                    Console.WriteLine($"Case {caseNumber}: impossible");
                }
                else
                {
                    ulamSpiral = UlamSpiral(input1 * input2, input1, input2, cellLocation);

                    int pathLength = BFS(cellLocation, input1, input2, ulamSpiral);

                    if (pathLength != -1)
                    {
                        Console.WriteLine($"Case {caseNumber}: {pathLength}");
                    }
                    else
                    {
                        Console.WriteLine($"Case {caseNumber}: impossible");
                    }
                }

                caseNumber++;
                // Clear dictionary
                cellLocation.Clear();
            }

            Console.ReadKey();
        }


        private static int BFS(Dictionary<int, int[]> cellLocation, int input1, int input2, int[,] ulamSpiral)
        {
            // BFS
            Queue<int[]> q = new Queue<int[]>();
            Queue<int> pathQueue = new Queue<int>();
            q.Enqueue(new int[] { cellLocation[input1][0], cellLocation[input1][1] });
            pathQueue.Enqueue(0);

            // marking visited nodes
            ulamSpiral[cellLocation[input1][0], cellLocation[input1][1]] = -1;

            while (q.Count != 0)
            {
                int[] loc = q.Dequeue();
                int path = pathQueue.Dequeue();

                if (ValidateInput(loc[0] + 1, loc[1], ulamSpiral))
                {
                    if (ulamSpiral[loc[0] + 1, loc[1]] == input2)
                    {
                        return path + 1;
                    }

                    q.Enqueue(new int[] { loc[0] + 1, loc[1] });
                    pathQueue.Enqueue(path + 1);

                    ulamSpiral[loc[0] + 1, loc[1]] = -1;
                }

                if (ValidateInput(loc[0] - 1, loc[1], ulamSpiral))
                {
                    if (ulamSpiral[loc[0] - 1, loc[1]] == input2)
                    {
                        return path + 1;
                    }
                    q.Enqueue(new int[] { loc[0] - 1, loc[1] });
                    pathQueue.Enqueue(path + 1);

                    ulamSpiral[loc[0] - 1, loc[1]] = -1;
                }

                if (ValidateInput(loc[0], loc[1] + 1, ulamSpiral))
                {
                    if (ulamSpiral[loc[0], loc[1] + 1] == input2)
                    {
                        return path + 1;
                    }

                    q.Enqueue(new int[] { loc[0], loc[1] + 1 });
                    pathQueue.Enqueue(path + 1);

                    ulamSpiral[loc[0], loc[1] + 1] = -1;
                }

                if (ValidateInput(loc[0], loc[1] - 1, ulamSpiral))
                {
                    if (ulamSpiral[loc[0], loc[1] - 1] == input2)
                    {
                        return path + 1;
                    }

                    q.Enqueue(new int[] { loc[0], loc[1] - 1 });
                    pathQueue.Enqueue(path + 1);

                    ulamSpiral[loc[0], loc[1] - 1] = -1;
                }
            }

            return -1;
        }

        /// <summary>
        /// If location is within spiral's bounds, has not been visited and not a prime number
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="ulamSpiral"></param>
        /// <returns></returns>
        private static bool ValidateInput(int x, int y, int[,] ulamSpiral)
        {
            return x < ulamSpiral.GetLength(0) && x >= 0 && y < ulamSpiral.GetLength(1)
                && y >= 0 && ulamSpiral[x, y] != -1 && !IsPrime(ulamSpiral[x, y]);
        }


        /// <summary>
        /// Checks if number is prime or not
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        private static bool IsPrime(int n)
        {
            if (n <= 2 || n % 2 == 0)
            {
                return n == 2;
            }

            for (int i = 3; i * i <= n; i += 2)
            {
                if (n % i == 0)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Builds Ulam's spiral
        /// </summary>
        /// <param name="n">Dimensions of Ulam's spiral</param>
        /// <param name="x">First value</param>
        /// <param name="y">Second value</param>
        /// <param name="cellLocation">Marks endpoints</param>
        /// <returns></returns>
        public static int[,] UlamSpiral(int n, int x, int y, Dictionary<int, int[]> cellLocation)
        {
            // Generate a 5-5 spiral
            int[,] ulamSpiral = new int[n, n];
            int currentNumber = n * n;

            int round = 0;

            // Round 0
            // bottom row [4,4], [4, 3], [4, 2], [4, 1], [4, 0]
            // left column [3, 0], [2, 0], [1, 0], [0, 0]
            // top row [0, 1], [0, 2], [0, 3], [0, 4]
            // right column [1, 4], [2, 4], [3, 4]

            // Round 1
            // bottom row [3,3], [3, 2], [3, 1]
            // left column [2, 1], [1, 1]
            // top row [1, 2], [1, 3]
            // right column [2, 3]

            //Round 2
            // bottom row [2,2]          

            while (round <= n / 2)
            {
                //Get bottom row
                for (int i = ulamSpiral.GetLength(0) - 1 - round; i >= round; i--)
                {
                    ulamSpiral[ulamSpiral.GetLength(0) - 1 - round, i] = currentNumber;
                    if (currentNumber == x || currentNumber == y)
                    {
                        cellLocation.Add(currentNumber, new int[] { ulamSpiral.GetLength(0) - 1 - round, i });
                    }
                    currentNumber--;
                }

                //Get left column
                for (int i = ulamSpiral.GetLength(0) - 2 - round; i >= round; i--)
                {
                    ulamSpiral[i, round] = currentNumber;
                    if (currentNumber == x || currentNumber == y)
                    {
                        cellLocation.Add(currentNumber, new int[] { i, round });
                    }
                    currentNumber--;
                }

                // Get top row
                for (int i = round + 1; i < ulamSpiral.GetLength(0) - round; i++)
                {
                    ulamSpiral[round, i] = currentNumber;
                    if (currentNumber == x || currentNumber == y)
                    {
                        cellLocation.Add(currentNumber, new int[] { round, i });
                    }
                    currentNumber--;
                }

                // Get right column
                for (int i = round + 1; i < ulamSpiral.GetLength(0) - 1 - round; i++)
                {
                    ulamSpiral[i, ulamSpiral.GetLength(0) - 1 - round] = currentNumber;
                    if (currentNumber == x || currentNumber == y)
                    {
                        cellLocation.Add(currentNumber, new int[] { i, ulamSpiral.GetLength(0) - 1 - round });
                    }
                    currentNumber--;
                }

                round++;
            }

            return ulamSpiral;
        }


    }
}
