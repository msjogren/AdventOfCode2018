using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        const int FabricSize = 1000;
        List<Rectangle> claims = new List<Rectangle>();

        int[,] fabric = new int[FabricSize, FabricSize];

        foreach (string claim in File.ReadAllLines("input.txt"))
        {
            Rectangle rect = ParseClaim(claim);
            claims.Add(rect);

            for (int x = rect.Left; x < rect.Right; x++)
                for (int y = rect.Top; y < rect.Bottom; y++)
                    fabric[x, y]++;            
        }

        int sum = 0;

        for (int x = 0; x < FabricSize; x++)
            for (int y = 0; y < FabricSize; y++)
                sum += fabric[x, y] > 1 ? 1 : 0; 

        Console.WriteLine(sum);

        int id = 0;
        foreach (Rectangle rect in claims)
        {
            id++;
            bool onlyOnes = true;
            for (int x = rect.Left; x < rect.Right && onlyOnes; x++)
                for (int y = rect.Top; y < rect.Bottom && onlyOnes; y++)
                    if (fabric[x, y] != 1) onlyOnes = false;

            if (onlyOnes) 
            {
                Console.WriteLine(id);
                break;
            }
        }
    }

    static Rectangle ParseClaim(string claim)
    {
        // #IDID @ XXX,YYY: WWxHH
        int at = claim.IndexOf('@');
        int comma = claim.IndexOf(',');
        int colon = claim.IndexOf(':');
        int x = claim.IndexOf('x');

        return new Rectangle(
            int.Parse(claim.Substring(at + 2, comma - at - 2)),
            int.Parse(claim.Substring(comma + 1, colon - comma - 1)),
            int.Parse(claim.Substring(colon + 2, x - colon - 2)),
            int.Parse(claim.Substring(x + 1))
        );
    }
}