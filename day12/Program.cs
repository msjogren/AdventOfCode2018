using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        const bool Part1 = false;
        const long Generations = Part1 ? 20 : 1000; // 50_000_000_000;
        string[] input = File.ReadAllLines("input.txt");
        string currentState = input[0].Substring(input[0].IndexOf(':') + 2);
        Dictionary<string, string> rules = new Dictionary<string, string>();

        for (int i = 2; i < input.Length; i++)
        {
            string[] ruleParts = input[i].Split(" => ");
            rules.Add(ruleParts[0], ruleParts[1]);
        }

        int zeroPos = 0;

        Console.WriteLine($"00: {currentState} (0 = {zeroPos})");

        for (long g = 1; g <= Generations; g++)
        {
            string tmpState = $"....{currentState}....";
            StringBuilder newState = new StringBuilder();
            for (int c = 2; c < tmpState.Length - 2; c++)
            {
                string key = tmpState.Substring(c - 2, 5);
                if (!rules.TryGetValue(key, out string pot)) { pot = "."; }
                newState.Append(pot);

                if (pot == "#" && c < 4) zeroPos += (4 - c);
            }

            if (newState[0] == '.') newState.Remove(0, 1);
            if (newState[0] == '.') newState.Remove(0, 1);
            if (newState[newState.Length - 1] == '.') newState.Remove(newState.Length - 1, 1);
            if (newState[newState.Length - 1] == '.') newState.Remove(newState.Length - 1, 1);

            currentState = newState.ToString();
            Console.WriteLine($"{g:d2}: {currentState} (0 = {zeroPos}) (sum = {CalculatePotSum(currentState, zeroPos)})");
        }

        // Part2: The same pattern repeats (shifted right) after around 130 iterations. From there on, each additional generation adds 78 to the sum.
        // 1000 generations => 80212. 
        // 50 000 000 000 => 80 212 + (50 000 000 000 - 1 000) * 78 = 3 900 000 002 212

        Console.ReadLine();
    }

    static int CalculatePotSum(string state, int zeroOffset)
    {
        int sum = 0;
        for (int i = 0; i < state.Length; i++)
        {
            if (state[i] == '#')
            {
                sum += (i - zeroOffset);
            }
        }

        return sum;
    }
}
