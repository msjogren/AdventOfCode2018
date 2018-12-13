using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    enum Direction { Up, Right, Down, Left }

    class Cart
    {
        public Direction Direction;
        public int Turns;
        public int LastMoveTick;
    }

    static void Main(string[] args)
    {
        const bool Part1 = false;
        HashSet<Cart> allCarts = new HashSet<Cart>(); 

        string[] input = File.ReadAllLines("input.txt");

        int width = input[0].Length;
        int height = input.Length;

        char[,] tracks = new char[width, height];
        Cart[,] carts = new Cart[width, height];

        for (int line = 0; line < height; line++)
        {
            for (int col = 0; col < input[line].Length; col++)
            {
                bool hasCart = false;
                Direction dir = Direction.Up;

                char c = input[line][col];
                switch (c)
                {
                    case '>':
                        hasCart = true;
                        dir = Direction.Right;
                        c = '-';
                        break;
                    case '<':
                        hasCart = true;
                        dir = Direction.Left;
                        c = '-';
                        break;
                    case 'v':
                        hasCart = true;
                        dir = Direction.Down;
                        c = '|';
                        break;
                    case '^':
                        hasCart = true;
                        dir = Direction.Up;
                        c = '|';
                        break;
                }

                tracks[col, line] = c;

                if (hasCart)
                {
                    carts[col, line] = new Cart() { Direction = dir, LastMoveTick = -1 };
                    if (!Part1) allCarts.Add(carts[col, line]);
                }
            }
        }

        bool done = false;
        for (int tick = 0; !done; tick++)
        {
            for (int y = 0; y < height && !done; y++)
            {
                for (int x = 0; x < width && !done; x++)
                {
                    Cart cart = carts[x, y];
                    if (cart == null || cart.LastMoveTick == tick) continue;
                    int newX = -1, newY = -1;

                    switch (cart.Direction)
                    {
                        case Direction.Up: newX = x; newY = y - 1; break;
                        case Direction.Right: newX = x + 1; newY = y; break;
                        case Direction.Down: newX = x; newY = y + 1; break;
                        case Direction.Left: newX = x - 1; newY = y; break;
                    }

                    if (carts[newX, newY] != null)
                    {
                        if (Part1)
                        {
                            Console.WriteLine($"{newX},{newY} after {tick} ticks");
                            done = true;
                        }
                        else
                        {
                            allCarts.Remove(carts[x, y]);
                            allCarts.Remove(carts[newX, newY]);
                            carts[x, y] = null;
                            carts[newX, newY] = null;
                        }
                        break;
                    }
                    else
                    {
                        carts[newX, newY] = cart;
                        carts[x, y] = null;
                    }

                    Direction newDirection = cart.Direction;
                    switch (tracks[newX, newY])
                    {
                        case '\\':
                            switch (cart.Direction)
                            {
                                case Direction.Up: newDirection = Direction.Left; break;
                                case Direction.Right: newDirection = Direction.Down; break;
                                case Direction.Down: newDirection = Direction.Right; break;
                                case Direction.Left: newDirection = Direction.Up; break;
                            }
                            break;

                        case '/':
                            switch (cart.Direction)
                            {
                                case Direction.Up: newDirection = Direction.Right; break;
                                case Direction.Right: newDirection = Direction.Up; break;
                                case Direction.Down: newDirection = Direction.Left; break;
                                case Direction.Left: newDirection = Direction.Down; break;
                            }
                            break;

                        case '+':
                            switch (cart.Turns % 3)
                            {
                                case 0: // Turn left
                                    switch (cart.Direction)
                                    {
                                        case Direction.Up: newDirection = Direction.Left; break;
                                        case Direction.Right: newDirection = Direction.Up; break;
                                        case Direction.Down: newDirection = Direction.Right; break;
                                        case Direction.Left: newDirection = Direction.Down; break;
                                    }
                                    break;
                                case 1: // Go straight
                                    break;
                                case 2: // Turn right
                                    switch (cart.Direction)
                                    {
                                        case Direction.Up: newDirection = Direction.Right; break;
                                        case Direction.Right: newDirection = Direction.Down; break;
                                        case Direction.Down: newDirection = Direction.Left; break;
                                        case Direction.Left: newDirection = Direction.Up; break;
                                    }
                                    break;

                            }
                            cart.Turns++;
                            break;
                    }

                    cart.Direction = newDirection;
                    cart.LastMoveTick = tick;
                }
            }

            if (!Part1 && allCarts.Count == 1)
            {
                for (int y = 0; y < height && !done; y++)
                {
                    for (int x = 0; x < width && !done; x++)
                    {
                        if (carts[x, y] != null)
                        {
                            Console.WriteLine($"{x},{y} after {tick} ticks");
                            done = true;
                        }
                    }
                }
            }

        }

        Console.ReadLine();
    }
}
