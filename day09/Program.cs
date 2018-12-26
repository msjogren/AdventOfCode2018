using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        const bool Step2 = true;
        const int Elves = 431;
        const int LastMarble = 70950 * (Step2 ? 100 : 1);

        long[] score = new long[Elves];
        int currentElf = 0;
        LinkedList<int> circle = new LinkedList<int>();
        LinkedListNode<int> currentMarble = circle.AddFirst(0);

        for (int marble = 1; marble <= LastMarble; marble++)
        {
            if (marble % 23 == 0)
            {
                LinkedListNode<int> marbleToRemove = StepList(currentMarble, -7);
                score[currentElf] += marble + marbleToRemove.Value;
                currentMarble = StepList(marbleToRemove, 1);
                marbleToRemove.List.Remove(marbleToRemove);
            }
            else
            {
                currentMarble = currentMarble.List.AddAfter(StepList(currentMarble, 1), marble);
            }

            currentElf = (currentElf + 1) % Elves;
        }

        long highScore = score.Max();
        for (int e = 0; e < score.Length; e++)
        {
            Console.WriteLine($"Elf {e+1} score {score[e]} {(score[e] == highScore ? "winner" : string.Empty)}");
        }

        Console.ReadLine();

    }

    static LinkedListNode<int> StepList(LinkedListNode<int> current, int steps)
    {
        if (steps < 0)
        {
            while (steps++ < 0)
            {
                current = current.Previous ?? current.List.Last;
            }
        }
        else
        {
            while (steps-- > 0)
            {
                current = current.Next ?? current.List.First;
            }
        }

        return current;
    }
}

