using System;
using System.IO;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        const bool Part1 = false;
        string[] input = File.ReadAllLines("input.txt");

        if (Part1)
        {
            int twos = 0, threes = 0;

            foreach (string line in input)
            {
                var (hasTwo, hasThree) = Scan(line);

                if (hasTwo) twos++;
                if (hasThree) threes++;
            }

            Console.WriteLine(twos * threes);
        }
        else
        {
            bool found = false;
            for (int i = 0; i < input.Length - 1 && !found; i++)
            for (int j = i + 1; j < input.Length && !found; j++)
            {
                if (DiffersBySingleChar(input[i], input[j], out string common))
                {
                    found = true;
                    Console.WriteLine(common);
                }
            }
        }
    }

    static (bool hasTwo, bool hasThree) Scan(string s)
    {
        const int maxchars = 'z' - 'a' + 1;
        int[] counts = new int[maxchars];

        foreach (char c in s) counts[c - 'a']++;

        return (counts.Any(i => i == 2), counts.Any(i => i == 3));
    }

    private static bool DiffersBySingleChar(string s1, string s2, out string common)
    {
        int differentChars = 0;
        int diffIdx = -1;

        for (int i = 0; i < s1.Length; i++)
        {
            if (s1[i] != s2[i])
            {
                differentChars++;
                diffIdx = i;
            }
        }

        if (differentChars == 1)
        {
            common = s1.Remove(diffIdx, 1);
            return true;
        }
        else
        {
            common = null;
            return false;
        }
    }
}
