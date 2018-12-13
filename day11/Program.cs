using System;

class Program
{
    static void Main(string[] args)
    {
        const bool Part1 = false;
        const int GridSize = 300;
        const int MinSquareSize = Part1 ? 3 : 1;
        const int MaxSquareSize = Part1 ? 3 : GridSize; 

        const int Input = 1718;

        int[,] grid = new int[GridSize, GridSize];

        for (int x = 0; x < GridSize; x++)
        {
            for (int y = 0; y < GridSize; y++)
            {
                int rackId = (x + 1) + 10;
                int power = rackId * (y + 1) + Input;
                power = ((int)((long)power * rackId) % 1000) / 100;
                power -= 5;
                grid[x, y] = power;
            }
        }

        int maxSuareSum = Int32.MinValue;
        (int x, int y) maxSquareCoords = (-1, -1);
        
        if (Part1)
        {
            for (int x = 0; x < (GridSize - 2); x++)
            {
                for (int y = 0; y < (GridSize - 2); y++)
                {
                    int sum = grid[x, y] + grid[x, y + 1] + grid[x, y + 2] +
                              grid[x + 1, y] + grid[x + 1, y + 1] + grid[x + 1, y + 2] +
                              grid[x + 2, y] + grid[x + 2, y + 1] + grid[x + 2, y + 2];

                    if (sum > maxSuareSum)
                    {
                        maxSuareSum = sum;
                        maxSquareCoords = (x, y);
                    }
                }
            }

            Console.WriteLine($"Sum {maxSuareSum} at {maxSquareCoords.x + 1},{maxSquareCoords.y + 1}");
        }
        else
        {
            int squareSize = 0;

            for (int size = MinSquareSize; size <= MaxSquareSize; size++)
            {
                Console.WriteLine("Calculating squares with size " + size);

                for (int x = 0; x < (GridSize - size + 1); x++)
                {
                    for (int y = 0; y < (GridSize - size + 1); y++)
                    {
                        int sum = 0;

                        for (int xx = x; xx < (x + size); xx++)
                        {
                            for (int yy = y; yy < (y + size); yy++)
                            {
                                sum += grid[xx, yy];
                            }
                        }

                        if (sum > maxSuareSum)
                        {
                            maxSuareSum = sum;
                            maxSquareCoords = (x, y);
                            squareSize = size;
                        }
                    }
                }
            }

            Console.WriteLine($"Sum {maxSuareSum} at {maxSquareCoords.x + 1},{maxSquareCoords.y + 1},{squareSize}");
        }


        Console.ReadLine();
    }
}
