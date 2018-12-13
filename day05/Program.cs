using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;


class Program
{
    static void Main(string[] args)
    {
        const bool Part1 = false;

        //string input = "dabAcCaCBAcCcaDA";
        string input = File.ReadAllText("input.txt");

        if (Part1)
        {
            Console.WriteLine(GetLengthAfterReaction(input));
        }
        else
        {
            var results =
                from unitToRemove in input.ToLower().Distinct()
                let outputLength = GetLengthAfterReaction(input.Replace(unitToRemove.ToString(), "", StringComparison.InvariantCultureIgnoreCase))
                orderby outputLength
                select new { unitToRemove, outputLength };
            
            foreach (var result in results)
            {
                Console.WriteLine($"{result.unitToRemove} : {result.outputLength}");
            }
        }

        Console.ReadLine();
    }

    static int GetLengthAfterReaction(string polymer)
    {

        List<char> output = new List<char>();

        foreach (char c in polymer)
        {
            if (output.Count > 0 && AreSameTypeWithOppositePolarity(output[output.Count - 1], c))
            {
                output.RemoveAt(output.Count - 1);
            }
            else
            {
                output.Add(c);
            }
        }

        return output.Count;
    }

    static bool AreSameTypeWithOppositePolarity(char c1, char c2)
    {
        return c1 != c2 && (Char.ToUpper(c1) == c2 || c1 == Char.ToUpper(c2));
    }
}
    