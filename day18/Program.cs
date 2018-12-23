using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        const bool Part1 = false;
        const int MaxMinutes = Part1 ? 10 : 1_000_000_000;
        string[] input = File.ReadAllLines("input.txt");
        int width = input[0].Length;
        int height = input.Length;
        char[,] area = new char[width, height];
        char[,] changedArea = new char[width, height];

        for (int y = 0; y < input.Length; y++)
        {
            for (int x = 0; x < width; x++)
            {
                area[x, y] = input[y][x];
            }
        }


        char[,] area10k = null;
        int value10k = -1;
        int answerMinute = -1;

        Dictionary<int, (int, char[,])> history = new Dictionary<int, (int, char[,])>();

        for (int minutes = 1; minutes <= MaxMinutes; minutes++)
        {
            for (int x = 0; x < width; x++)
            { 
                for (int y = 0; y < height; y++)
                {
                    changedArea[x, y] = GetAcreType(area, x, y, width, height);
                }
            }

            var tmp = area;
            area = changedArea;
            changedArea = tmp;

            if (Part1)
            {
                Console.WriteLine($"After {minutes} minutes");
                DrawGrid(area, width, height);
            }
            else
            {
                if (minutes == 10000)
                {
                    area10k = (char[,])area.Clone();
                    value10k = GetResourceValue(area, width, height);
                }
                else if (minutes > 10000 && answerMinute < 0)
                {
                    int value = GetResourceValue(area, width, height);
                    if (value == value10k && AreAreasEqual(area, area10k, width, height))
                    {
                        int interval = minutes - 10000;
                        int lastRepeat = minutes;
                        while (lastRepeat < MaxMinutes) lastRepeat += interval;
                        answerMinute = minutes + interval - (lastRepeat - MaxMinutes);

                        Console.WriteLine($"Repeat found at minute {minutes}, same as 10000. Interval {interval}. Answer will be found at {answerMinute}.");
                    }
                }
                else if (minutes == answerMinute)
                {
                    break;
                }
            }
        }

        Console.WriteLine(GetResourceValue(area, width, height));

        Console.ReadLine();
    }

    private static bool AreAreasEqual(char[,] area1, char[,] area2, int width, int height)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (area1[x, y] != area2[x, y]) return false;
            }
        }

        return true;
    }

    private static char GetAcreType(char[,] area, int x, int y, int width, int height)
    {
        int lumberyards = 0, trees = 0, open = 0;
        var offsets = new (int dx, int dy)[] { (-1, -1), (0, -1), (1, -1),
                                               (-1, 0),           (1, 0),
                                               (-1, 1),  (0, 1),  (1, 1) };

        foreach (var (dx, dy) in offsets)
        {
            if ((x + dx) >= 0 && (x + dx) < width && (y + dy) >= 0 && (y + dy) < height)
            {
                switch (area[x + dx, y + dy])
                {
                    case '#': lumberyards++; break;
                    case '|': trees++; break;
                    case '.': open++; break;
                }
            }
        }

        if (area[x, y] == '.' && trees >= 3)
        {
            return '|';
        }
        else if (area[x, y] == '|' && lumberyards >= 3)
        {
            return '#';
        }
        else if (area[x, y] == '#')
        {
            return lumberyards > 0 && trees > 0 ? '#' : '.';
        }
        else
        {
            return area[x, y];
        }
    }

    private static int GetResourceValue(char[,] area, int width, int height)
    {
        int lumberyards = 0, trees = 0;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (area[x, y] == '#') lumberyards++;
                else if (area[x, y] == '|') trees++;
            }
        }

        return lumberyards * trees;
    }

    static void DrawGrid(char[,] grid, int width, int height)
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Console.Write(grid[x, y]);
                Console.Write(' ');
            }

            Console.WriteLine();
        }
    }
    

}
