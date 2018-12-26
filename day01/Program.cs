using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        const bool Part1 = false;
        
        IEnumerable<int> changes = File.ReadAllLines("input.txt").Select(n => int.Parse(n));

        if (Part1)
        {
            Console.WriteLine(changes.Sum());
        }
        else
        {
            IEnumerable<T> RepeatList<T>(IEnumerable<T> list) { while (true) foreach (T t in list) yield return t; }

            int frequency = 0;
            HashSet<int> seen = new HashSet<int>() { frequency };

            foreach (int change in RepeatList(changes))
            {
                frequency += change;
                if (seen.Contains(frequency))
                {
                    Console.WriteLine(frequency);
                    break;
                }
                else
                {
                    seen.Add(frequency);
                }
            }
        }
    }
}
