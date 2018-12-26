using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Point4D
{
    public int X;
    public int Y;
    public int Z;
    public int Q;
}

class Program
{
    static void Main(string[] args)
    {

        List<Point4D> points = new List<Point4D>();
        List<HashSet<Point4D>> constellations = new List<HashSet<Point4D>>();

        foreach (string line in File.ReadAllLines("input.txt"))
        {
            int[] coords = line.Split(',').Select(s => int.Parse(s)).ToArray();
            points.Add(new Point4D() { X = coords[0], Y = coords[1], Z = coords[2], Q = coords[3] });
        }

        foreach (Point4D pt in points)
        {
            var sets = constellations.Where(c => c.Any(pt2 => ManhattanDistance(pt, pt2) <= 3));
            if (!sets.Any())
            {
                constellations.Add(new HashSet<Point4D>() { pt });
            }
            else if (sets.Count() == 1)
            {
                sets.First().Add(pt);
            }
            else
            {
                var copy = sets.ToList();
                copy[0].Add(pt);
                for (int i = 1; i < copy.Count; i++)
                {
                    foreach (Point4D pt2 in copy[i]) copy[0].Add(pt2);
                    constellations.Remove(copy[i]);
                }
            }
        }

        Console.WriteLine(constellations.Count());

        Console.ReadLine();
    }

    static int ManhattanDistance(Point4D p1, Point4D p2)
    {
        return Math.Abs(p1.X - p2.X) + Math.Abs(p1.Y - p2.Y) + Math.Abs(p1.Z - p2.Z) + Math.Abs(p1.Q - p2.Q);
    }
}
