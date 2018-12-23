using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    struct ClayInput
    {
        public int FromX;
        public int ToX;
        public int FromY;
        public int ToY;
    }

    static void Main(string[] args)
    {
        List<ClayInput> input = new List<ClayInput>();
        foreach (string line in File.ReadAllLines("input.txt"))
        {
            int eq1 = line.IndexOf('=');
            int comma = line.IndexOf(',', eq1);
            int eq2 = line.IndexOf('=', comma);
            int dot = line.IndexOf('.', eq2);

            int val1 = int.Parse(line.Substring(eq1 + 1, comma - eq1 - 1));
            int val2 = int.Parse(line.Substring(eq2 + 1, dot - eq2 - 1));
            int val3 = int.Parse(line.Substring(dot + 2));

            if (line[0] == 'x')
            {
                input.Add(new ClayInput() { FromX = val1, ToX = val1, FromY = val2, ToY = val3 });
            }
            else
            {
                input.Add(new ClayInput() { FromX = val2, ToX = val3, FromY = val1, ToY = val1 });
            }
        }

        int minX = input.Min(i => i.FromX) - 1;
        int maxX = input.Max(i => i.ToX) + 1;
        int minY = input.Min(i => i.FromY);
        int maxY = input.Max(i => i.ToY) + 1;
        int width = maxX - minX + 1;
        int height = maxY + 1;

        char[,] grid = new char[width, height];
        foreach (ClayInput i in input)
        {
            if (i.FromX == i.ToX)
            {
                for (int y = i.FromY; y <= i.ToY; y++) grid[i.FromX - minX, y] = '#';
            }
            else
            {
                for (int x = i.FromX; x <= i.ToX; x++) grid[x - minX, i.FromY] = '#';
            }
        }

        grid[500 - minX, 0] = '+';
        for (int x = 0; x < width; x++) grid[x, height - 1] = 'x';  // For bottom detection

        Console.WriteLine($"{minX} - {maxX}, 0 - {maxY}");

        FillWater(grid, 500 - minX, 0, width, height);

        //DrawGrid(grid, width, height);

        int waterTiles = 0 - (minY - 1);
        int waterAtRestTiles = 0;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (grid[x, y] == '~' || grid[x, y] == '|') waterTiles++;
                if (grid[x, y] == '~') waterAtRestTiles++;
            }
        }

        Console.WriteLine(waterTiles);
        Console.WriteLine(waterAtRestTiles);

        Console.ReadLine();
    }

    private static  void FillWater(char[,] grid, int x, int y, int width, int height)
    {
        if (grid[x, y] == '\0') grid[x, y] = '|';

        if (grid[x, y + 1] == 'x')
        {
            return;     // Reached bottom
        }
        else if (grid[x, y + 1] == '\0' || grid[x, y + 1] == '|')
        {
            FillWater(grid, x, y + 1, width, height);       // Flow down
        }

        if (grid[x, y + 1] == '~' || grid[x, y + 1] == '#')
        {
            // Flow to the sides

            int leftWall = -1, rightWall = -1;

            for (int xx = x - 1; xx >= 0; xx--)         // Flow to the left
            {
                if (grid[xx, y] == '#')
                {
                    leftWall = xx;
                    break;
                }

                if (grid[xx, y + 1] == '#' || grid[xx, y + 1] == '~')
                {
                    if (grid[xx, y] == '\0') grid[xx, y] = '|';
                }
                else if (grid[xx + 1, y + 1] == '#' && (grid[xx, y + 1] == '\0' || grid[xx, y + 1] == '|'))
                {
                    if (grid[xx, y] == '\0') grid[xx, y] = '|';
                    FillWater(grid, xx, y + 1, width, height);

                    if (grid[xx, y + 1] != '~') break;
                }
                else
                {
                    break;
                }
            }

            for (int xx = x + 1; xx < width; xx++)      // Flow to the right
            {
                if (grid[xx, y] == '#')
                {
                    rightWall = xx;
                    break;
                }

                if (grid[xx, y + 1] == '#' || grid[xx, y + 1] == '~')
                {
                    if (grid[xx, y] == '\0') grid[xx, y] = '|';
                }
                else if (grid[xx - 1, y + 1] == '#' && (grid[xx, y + 1] == '\0' || grid[xx, y + 1] == '|'))
                {
                    if (grid[xx, y] == '\0') grid[xx, y] = '|';
                    FillWater(grid, xx, y + 1, width, height);

                    if (grid[xx, y + 1] != '~') break;
                }
                else
                {
                    break;
                }
            }

            if (leftWall >= 0 && rightWall >= 0)
            {
                for (int xx = leftWall + 1; xx < rightWall; xx++)
                {
                    grid[xx, y] = '~';
                }
            }
        }
    }


    public static void DrawGrid(char[,] grid, int width, int height)
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Console.Write(grid[x, y] == 0 ? '.' : grid[x, y]);
                Console.Write(' ');
            }

            Console.WriteLine();
        }
    }


}
