using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        const bool Part1 = true;
        const int Input = 919901;
        const int Elves = 2;

        List<int> recipes = new List<int>() { 3, 7 };
        int[] elfCurrent = Enumerable.Range(0, Elves).ToArray();
        bool done = false;

        int checkIdx = 0;
        int[] inputDigits = Input.ToString().ToCharArray().Select(c => c - '0').ToArray();

        while (!done)
        {
            int newRecipe = elfCurrent.Sum(c => recipes[c]);
            foreach (char digit in newRecipe.ToString().ToCharArray())
            {
                recipes.Add(digit - '0');                
            }

            for (int i = 0; i < Elves; i++)
            {
                int moves = (1 + recipes[elfCurrent[i]]) % recipes.Count;
                elfCurrent[i] = (elfCurrent[i] + moves) % recipes.Count;
            } 

            if (Part1)
            {
                done = recipes.Count >= (Input + 10);
            }
            else
            {
                while ((checkIdx + inputDigits.Length) <= recipes.Count)
                {
                    bool match = true;
                    for (int k = 0; k < inputDigits.Length && match; k++)
                    {
                        if (recipes[checkIdx + k] != inputDigits[k])
                        {
                            match = false;
                            checkIdx++;
                            break;
                        }
                    }

                    if (match)
                    {
                        done = true;
                        Console.WriteLine(checkIdx);
                        break;
                    }
                }
            }
        }

        if (Part1)
        {
            for (int i = Input; i < Input + 10; i++)
            {
                Console.Write(recipes[i]);
            }
            Console.WriteLine();
        }

        Console.ReadLine();
    }
}
