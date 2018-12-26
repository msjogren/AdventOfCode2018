using System;
using System.IO;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        Span<int> input = File.ReadAllText("input.txt").Split(' ').Select(s => int.Parse(s)).ToArray();
        (int sum, _) = SumMetadata(input);
        Console.WriteLine($"Part 1: {sum}");

        (int rootValue, _) = GetValue(input);
        Console.WriteLine($"Part 2: {rootValue}");

        Console.ReadLine();
    }

    static (int sum, int consumed) SumMetadata(Span<int> node)
    {
        int childNodes = node[0];
        int metadataNodes = node[1];
        int offset = 2;
        int sum = 0;

        while (childNodes-- > 0)
        {
            var (childSum, childConsumed) = SumMetadata(node.Slice(offset));
            offset += childConsumed;
            sum += childSum;
        }

        while (metadataNodes-- > 0)
        {
            sum += node[offset++];
        }

        return (sum, offset);
    }

    static (int value, int consumed) GetValue(Span<int> node)
    {
        int childNodes = node[0];
        int metadataNodes = node[1];
        int offset = 2;
        int sum = 0;

        if (childNodes == 0)
        {
            while (metadataNodes-- > 0)
            {
                sum += node[offset++];
            }
            return (sum, offset);
        }
        else
        {
            int[] childValues = new int[childNodes];
            for (int c = 0; c < childNodes; c++)
            {
                var (childValue, childConsumed) = GetValue(node.Slice(offset));
                offset += childConsumed;
                childValues[c] = childValue;
            }

            while (metadataNodes-- > 0)
            {
                int childIdx = node[offset++];
                if (childIdx < 1 || childIdx > childNodes) continue;
                sum += childValues[childIdx - 1];
            }

            return (sum, offset);
            
        }
    }

}

