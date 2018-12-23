using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Collections.Generic;


class Program
{
    class Unit
    {
        public int HitPoints = 200;
        public int AttackPower = 3;
        public int X;
        public int Y;
        public char Type;
    }

    class Node
    {
        public char Type;

        public bool IsOpen { get => Type == '.'; }
        public Unit Unit;
    }

    static void Main(string[] args)
    {
        bool Part1 = false;
        string[] lines = File.ReadAllLines("input.txt");
        int width = lines[0].Length;
        int height = lines.Length;
        Node[,] cave = new Node[width, height];
        List<Unit> units = new List<Unit>();

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Node n = cave[x, y] = new Node() { Type = lines[y][x] };
                if (n.Type == 'E' || n.Type == 'G')
                {
                    n.Unit = new Unit() { X = x, Y = y, Type = n.Type };
                    if (!Part1 && n.Type == 'E') n.Unit.AttackPower = 12;   // 12 determined through trial and error
                    units.Add(n.Unit);
                }
            }
        }

        bool combatEnded = false;
        int outcome = 0;
        int deadElves = 0;

        for (int round = 1; !combatEnded; round++)
        {
            Queue<Unit> unitsToMove = new Queue<Unit>(units.OrderBy(u => u.Y).ThenBy(u => u.X));

            while (unitsToMove.Count > 0)
            {
                Unit currentUnit = unitsToMove.Dequeue();
                if (currentUnit.HitPoints <= 0 || combatEnded) continue;

                IEnumerable<Unit> targets = units.Where(u => u.Type != currentUnit.Type);

                if (!targets.Any())
                {
                    combatEnded = true;
                    Console.WriteLine($"{round - 1} completed rounds, {units.Sum(u => u.HitPoints)} remaining points");
                    outcome = (round - 1) * units.Sum(u => u.HitPoints);
                    break;
                }

                bool alreadyInRange = targets.SelectMany(t => AdjacentPoints(t.X, t.Y)).Any(p => p.X == currentUnit.X && p.Y == currentUnit.Y);
 
                if (!alreadyInRange)
                {
                    var pointsInRange = targets.SelectMany(t => AdjacentOpenPoints(cave, t.X, t.Y)).Distinct();

                    List<Point> bestPath = BreadthFirstShortestPath(cave, new Point(currentUnit.X, currentUnit.Y), pointsInRange);

                    if (bestPath != null)
                    {
                        cave[currentUnit.X, currentUnit.Y].Unit = null;
                        cave[currentUnit.X, currentUnit.Y].Type = '.';
                        Point moveTo = bestPath.First();
                        currentUnit.X = moveTo.X;
                        currentUnit.Y = moveTo.Y;
                        cave[currentUnit.X, currentUnit.Y].Unit = currentUnit;
                        cave[currentUnit.X, currentUnit.Y].Type = currentUnit.Type;
                    }
                }

                Unit attackTarget =
                    AdjacentPoints(currentUnit.X, currentUnit.Y).
                        Select(p => cave[p.X, p.Y].Unit).
                        Where(u => u != null && u.Type != currentUnit.Type).
                        OrderBy(u => u.HitPoints).
                        ThenBy(u => u.Y).
                        ThenBy(u => u.X).
                        FirstOrDefault();

                if (attackTarget != null)
                {
                    attackTarget.HitPoints -= currentUnit.AttackPower;
                    if (attackTarget.HitPoints <= 0)
                    {
                        if (!Part1 && attackTarget.Type == 'E') deadElves++;

                        cave[attackTarget.X, attackTarget.Y].Unit = null;
                        cave[attackTarget.X, attackTarget.Y].Type = '.';
                        units.Remove(attackTarget);
                    }
                }

            }

        }

        Console.WriteLine(outcome);
        if (!Part1) Console.WriteLine($"{deadElves} dead elves");

        Console.ReadLine();
    }


    static IEnumerable<Point> AdjacentPoints(int x, int y)
    {
        // In reading order
        yield return new Point(x, y - 1);
        yield return new Point(x - 1, y);
        yield return new Point(x + 1, y);
        yield return new Point(x, y + 1);
    }

    static IEnumerable<Point> AdjacentOpenPoints(Node[,] cave, int x, int y) => AdjacentPoints(x, y).Where(p => cave[p.X, p.Y].IsOpen);

    static List<Point> BreadthFirstShortestPath(Node[,] cave, Point from, IEnumerable<Point> targets)
    {
        int shortestPathLength = int.MaxValue;
        Queue<Point> queue = new Queue<Point>();
        List<List<Point>> solutionPaths = new List<List<Point>>();
        Dictionary<Point, Point> visited = new Dictionary<Point, Point>();

        queue.Enqueue(from);
        visited.Add(from, Point.Empty);

        while (queue.Count > 0)
        {
            Point pt = queue.Dequeue();

            if (targets.Contains(pt))
            {
                List<Point> path = new List<Point>() { pt };
                Point current = pt;
                while (visited.TryGetValue(current, out Point prev) && prev != from)
                {
                    path.Insert(0, prev);
                    current = prev;
                }
                
                if (path.Count == shortestPathLength)
                {
                    solutionPaths.Add(path);
                }
                else if (path.Count < shortestPathLength)
                {
                    solutionPaths.Clear();
                    solutionPaths.Add(path);
                    shortestPathLength = path.Count;
                }
            }
            else
            {
                foreach (Point nextpt in AdjacentOpenPoints(cave, pt.X, pt.Y))
                {
                    if (!visited.ContainsKey(nextpt))
                    {
                        queue.Enqueue(nextpt);
                        visited.Add(nextpt, pt);
                    }
                }
            }

        }

        return solutionPaths.OrderBy(path => path.First().Y).ThenBy(path => path.First().X).FirstOrDefault();
    }

    static void DrawCave(Node[,] cave, int width, int height)
    {
        List<Unit> units = new List<Unit>();

        for (int y = 0; y < height; y++)
        {
            units.Clear();

            for (int x = 0; x < width; x++)
            {
                Console.Write(cave[x, y].Type);
                if (cave[x, y].Unit != null) units.Add(cave[x, y].Unit);
            }

            Console.Write('\t');
            foreach (Unit u in units)
            {
                Console.Write($"\t{u.Type}({u.HitPoints})");
            }

            Console.WriteLine();
        }

        Console.WriteLine();
    }
}