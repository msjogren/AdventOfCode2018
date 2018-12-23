using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


class Nanobot
{
    public int X;
    public int Y;
    public int Z;
    public int R;
}

class Program
{
    static void Main(string[] args)
    {
        List<Nanobot> bots = new List<Nanobot>();

        foreach (string line in File.ReadAllLines("input.txt"))
        {
            int rstart = line.LastIndexOf('=') + 1;
            int openBracket = line.IndexOf('<');
            string[] coords = line.Substring(openBracket + 1, line.IndexOf('>') - openBracket - 1).Split(',');
            Nanobot bot = new Nanobot()
            {
                R = int.Parse(line.Substring(rstart)),
                X = int.Parse(coords[0]),
                Y = int.Parse(coords[1]),
                Z = int.Parse(coords[2])
            };
            bots.Add(bot);
        }

        int maxR = bots.Max(b => b.R);
        Nanobot maxRBot = bots.First(bot => bot.R == maxR);
        int botsInRange = bots.Where(b => ManhattanDistance(b, maxRBot) <= maxR).Count();

        Console.WriteLine(botsInRange);


        // Part 2, inspired by https://www.reddit.com/r/adventofcode/comments/a8s17l/2018_day_23_solutions/ecdqzdg/
        SortedDictionary<int, int> ranges = new SortedDictionary<int, int>();
        foreach (var bot in bots)
        {
            var dist = ManhattanDistance(bot, 0, 0, 0);
            ranges[Math.Max(0, dist - bot.R)] = 1;
            ranges[dist + bot.R] = -1;
        }

        int botsInRangePart2 = 0;
        int maxBotsInRange = 0;
        int distancePart2 = 0;
        foreach (var range in ranges)
        {
            botsInRangePart2 += range.Value;
            if (botsInRangePart2 > maxBotsInRange)
            {
                maxBotsInRange = botsInRange;
                distancePart2 = range.Key;
            }
        }

        Console.WriteLine(distancePart2);


        Console.ReadLine();
    }

    static int ManhattanDistance(Nanobot bot1, Nanobot bot2)
    {
        return Math.Abs(bot1.X - bot2.X) + Math.Abs(bot1.Y - bot2.Y) + Math.Abs(bot1.Z - bot2.Z);
    }

    static int ManhattanDistance(Nanobot bot1, int x, int y, int z)
    {
        return Math.Abs(bot1.X - x) + Math.Abs(bot1.Y - y) + Math.Abs(bot1.Z - z);
    }

}
