using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;

class Program
{
    const int MaxWidth = 100;

    static void Main(string[] args)
    {
        List<(Point location, Size velocity)> points = new List<(Point location, Size velocity)>();

        foreach (string line in File.ReadAllLines("input.txt"))
        {
            (int x, int y) = Parse(line, line.IndexOf("position=") + 9);
            (int dx, int dy) = Parse(line, line.IndexOf("velocity=") + 9);
            points.Add((new Point(x, y), new Size(dx, dy)));
        }

        int seconds = 0;

        while (true)
        {
            int spreadx = points.Max(p => p.location.X) - points.Min(p => p.location.X);
            int spready = points.Max(p => p.location.Y) - points.Min(p => p.location.Y);
            if (spreadx < MaxWidth && spready < MaxWidth)
            {
                Thread.Sleep(1000);
                Console.WriteLine($"After {seconds} seconds");
                DrawPoints(points);
            }

            for (int p = 0; p < points.Count; p++)
            {
                var x = points[p];
                x.location += x.velocity;
                points[p] = x;
            }

            seconds++;
        }

        Console.ReadLine();
    }

    private static void DrawPoints(List<(Point location, Size velocity)> points)
    {
        char[][] grid = new char[MaxWidth][];
        for (int i = 0; i < MaxWidth; i++) grid[i] = new String('.', MaxWidth).ToCharArray();

        int xoffset = points.Min(p => p.location.X);
        int yoffset = points.Min(p => p.location.Y);
        foreach (var p in points)
        {
            grid[p.location.Y - yoffset][p.location.X - xoffset] = '#';
        }

        for (int i = 0; i < MaxWidth; i++)
        {
            Console.WriteLine(new String(grid[i]));
        }
    }

    static (int first, int second) Parse(string input, int offset)
    {
        int openingBracket = input.IndexOf('<', offset);
        int closingBracket = input.IndexOf('>', openingBracket);
        string[] parts = input.Substring(openingBracket + 1, closingBracket - openingBracket - 1).Split(',', 2, StringSplitOptions.RemoveEmptyEntries);
        return (int.Parse(parts[0].Trim()), int.Parse(parts[1].Trim()));
    }
}
