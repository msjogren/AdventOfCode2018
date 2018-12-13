using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

class Program
{
    class Node
    {
        public char NearestCoord;
        public int ShortestDistance;
    }

    static void Main(string[] args)
    {
        const bool Part1 = false;

        Dictionary<char, Point> coords = new Dictionary<char, Point>();

        char c = 'A';
        foreach (string line in File.ReadAllLines("input.txt"))
        {
            int comma = line.IndexOf(',');
            Point p = new Point(int.Parse(line.Substring(0, comma)), int.Parse(line.Substring(comma + 2)));
            coords[c++] = p;
            if (c == '[') c = 'a';
        }

        if (Part1)
            SolvePart1(coords);
        else
            SolvePart2(coords);

        Console.ReadLine();
    }

    static void SolvePart1(Dictionary<char, Point> coords)
    {
        int fromX = coords.Values.Min(p => p.X) - 1;
        int fromY = coords.Values.Min(p => p.Y) - 1;
        int toX = coords.Values.Max(p => p.X) + 1;
        int toY = coords.Values.Max(p => p.Y) + 1;

        var grid = new Node[(toX - fromX + 1), (toY - fromY + 1)];
        Console.WriteLine($"grid w{(toX - fromX + 1)} h{(toY - fromY + 1)}");

        for (int x = fromX, xx = 0; x <= toX; x++, xx++)
        {
            for (int y = fromY, yy = 0; y <= toY; y++, yy++)
            {
                grid[xx, yy] = new Node() { ShortestDistance = int.MaxValue };

                foreach (var coord in coords)
                {
                    int distance = ManhattanDistance(x, y, coord.Value);
                    if (distance < grid[xx, yy].ShortestDistance)
                    {
                        grid[xx, yy].ShortestDistance = distance;
                        grid[xx, yy].NearestCoord = coord.Key;
                    }
                    else if (distance == grid[xx, yy].ShortestDistance)
                    {
                        grid[xx, yy].NearestCoord = '.';
                    }
                }
            }
        }

        // Remove infinite areas (along edges)
        for (int xx = 0; xx <= (toX - fromX); xx++)
        {
            coords.Remove(grid[xx, 0].NearestCoord);
            coords.Remove(grid[xx, (toY - fromY)].NearestCoord);
        }
        for (int yy = 0; yy <= (toY - fromY); yy++)
        {
            coords.Remove(grid[0, yy].NearestCoord);
            coords.Remove(grid[(toX - fromX), yy].NearestCoord);
        }

        Dictionary<char, int> areaSizes = new Dictionary<char, int>();
        for (int x = fromX + 1, xx = 1; x < toX; x++, xx++)
        {
            for (int y = fromY + 1, yy = 1; y < toY; y++, yy++)
            {
                if (!areaSizes.TryGetValue(grid[xx, yy].NearestCoord, out int currentSize)) currentSize = 0;
                areaSizes[grid[xx, yy].NearestCoord] = currentSize + 1;
            }
        }

        foreach (var kvp in areaSizes)
            Console.WriteLine($"{kvp.Key} : {kvp.Value}");

        Console.WriteLine(areaSizes.Where(s => coords.ContainsKey(s.Key)).Select(s => s.Value).Max());
    }

    static void SolvePart2(Dictionary<char, Point> coords)
    {
        // Part 2
        const int RegionDistance = 10000;

        int fromX = coords.Values.Min(p => p.X);
        int fromY = coords.Values.Min(p => p.Y);
        int toX = coords.Values.Max(p => p.X);
        int toY = coords.Values.Max(p => p.Y);
        int regionSize = 0;

        for (int x = fromX; x <= toX; x++)
        {
            for (int y = fromY; y <= toY; y++)
            {
                if (coords.Values.Sum(p => ManhattanDistance(x, y, p)) < RegionDistance)
                {
                    regionSize++;
                }
            }
        }

        Console.WriteLine(regionSize);
    }

    private static int ManhattanDistance(int x, int y, Point p) => Math.Abs(x - p.X) + Math.Abs(y - p.Y);
}
