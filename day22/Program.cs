using System;
using System.Collections.Generic;

enum RegionType
{
    Rocky,
    Wet,
    Narrow
}

enum Tool
{
    Neither,
    Torch,
    ClimbingGear
}

class Region
{
    public static int Depth;

    public int X;
    public int Y;
    public int GeologicalIndex;
    public int ErosionLevel => (GeologicalIndex + Depth) % 20183;
    public RegionType Type => (RegionType)(ErosionLevel % 3);

    public bool IsProhibitedTool(Tool tool)
    {
        switch (Type)
        {
            case RegionType.Rocky: return tool == Tool.Neither;
            case RegionType.Wet: return tool == Tool.Torch;
            case RegionType.Narrow: return tool == Tool.ClimbingGear;
        }

        return false;
    }
}

struct RegionWithTool
{
    public Region Region;
    public Tool Tool;
}

class Program
{
    const int Depth = 6084;
    const int TargetX = 14;
    const int TargetY = 709;
    const int Width = TargetX + 20;
    const int Height = TargetY + 20;

    static void Main(string[] args)
    {
        Region.Depth = Depth;
        Region[,] cave = new Region[Width, Height];

        int GeoIndex(int x, int y)
        {
            if (x == 0 && y == 0) return 0;
            else if (x == TargetX && y == TargetY) return 0;
            else if (y == 0) return x * 16807;
            else if (x == 0) return y * 48271;
            else return cave[x - 1, y].ErosionLevel * cave[x, y - 1].ErosionLevel;
        }

        for (int y = 0; y < Height; y++)
        { 
            for (int x = 0; x <  Width; x++) 
            {
                cave[x, y] = new Region() { X = x, Y = y, GeologicalIndex = GeoIndex(x, y) };
            }
        }

        int riskLevel = 0;

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                char ch = '?';
                if (x == 0 && y == 0) ch = 'M';
                else if (x == TargetX && y == TargetY) ch = 'T';
                else if (cave[x, y].Type == RegionType.Rocky) ch = '.';
                else if (cave[x, y].Type == RegionType.Wet) ch = '=';
                else if (cave[x, y].Type == RegionType.Narrow) ch = '|';

                if (x <= TargetX && y <= TargetY) riskLevel += (int)cave[x, y].Type;

                Console.Write(ch);
            }

            Console.WriteLine();
        }

        Console.WriteLine("Risk level: " + riskLevel);

        int shortestPath = DijkstraShortestPath(cave, 0, 0, TargetX, TargetY);
        Console.WriteLine(shortestPath + " minutes");

        Console.ReadLine();
    }

    private static int DijkstraShortestPath(Region[,] cave, int fromX, int fromY, int toX, int toY)
    {
        var previous = new Dictionary<RegionWithTool, RegionWithTool>();
        var distances = new Dictionary<(int x, int y, Tool tool), int>();
        var remaining = new List<(int x, int y, Tool tool)>();


        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                for (Tool tool = Tool.Neither; tool <= Tool.ClimbingGear; tool++)
                {
                    distances[(x, y, tool)] = int.MaxValue;
                }
            }
        }

        remaining.Add((0, 0, Tool.Torch));
        distances[(0, 0, Tool.Torch)] = 0;


        var neighbors = new(int dx, int dy)[] { (-1, 0), (0, -1), (1, 0), (0, 1) };
        var tools = new[] { Tool.Neither, Tool.Torch, Tool.ClimbingGear };

        while (remaining.Count > 0)
        {
            remaining.Sort((r1, r2) => distances[r1] - distances[r2]);
            var current = remaining[0];
            remaining.RemoveAt(0);
            int currentDist = distances[(current.x, current.y, current.tool)];

            foreach ((int dx, int dy) in neighbors)
            {
                if ((current.x + dx) < 0 || (current.y + dy) < 0 || (current.x + dx >= Width) || (current.y + dy >= Height)) continue;

                Region r = cave[current.x + dx, current.y + dy];
                if (!r.IsProhibitedTool(current.tool))
                {
                    if (distances[(r.X, r.Y, current.tool)] > (currentDist + 1))
                    {
                        distances[(r.X, r.Y, current.tool)] = currentDist + 1;
                        remaining.Add((current.x + dx, current.y + dy, current.tool));
                    }
                }
            }

            foreach (Tool newtool in tools)
            {
                if (current.tool == newtool) continue;

                Region r = cave[current.x, current.y];
                if (!r.IsProhibitedTool(newtool))
                {
                    if (distances[(r.X, r.Y, newtool)] > (currentDist + 7))
                    {
                        distances[(r.X, r.Y, newtool)] = currentDist + 7;
                        remaining.Add((current.x, current.y, newtool));
                    }
                }
            }
        }

        return distances[(toX, toY, Tool.Torch)];
    }
}
