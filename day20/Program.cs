using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace day20
{
    enum WallType
    {
        Unknown,
        Door,
        Wall
    }

    enum Direction
    {
        Up,
        Right,
        Down,
        Left
    }

    class Node
    {
        public int X;
        public int Y;
        public int DistanceFromStart;
        public WallType RightWallType;
        public WallType BottomWallType;
    }

    class Space
    {
        private const int GridSize = 400;
        private Node[,] _space = new Node[GridSize, GridSize];
        private HashSet<Node> _allNodes = new HashSet<Node>();
        private const int OffsetX = GridSize / 2;
        private const int OffsetY = GridSize / 2;

        public Node GetNodeAt(int x, int y)
        {
            if (_space[x + OffsetX, y + OffsetY] == null)
            {
                var node = new Node() { X = x, Y = y, DistanceFromStart = (x == 0 && y == 0) ? 0 : int.MaxValue };
                _space[x + OffsetX, y + OffsetY] = node;
                _allNodes.Add(node);
            }

            return _space[x + OffsetX, y + OffsetY];
        }

        public Node Move(Direction dir, Node from)
        {
            Node to = null;

            switch (dir)
            {
                case Direction.Up:
                    to = GetNodeAt(from.X, from.Y - 1);
                    to.BottomWallType = WallType.Door;
                    break;

                case Direction.Right:
                    to = GetNodeAt(from.X + 1, from.Y);
                    from.RightWallType = WallType.Door;
                    break;

                case Direction.Down:
                    to = GetNodeAt(from.X, from.Y + 1);
                    from.BottomWallType = WallType.Door;
                    break;

                case Direction.Left:
                    to = GetNodeAt(from.X - 1, from.Y);
                    to.RightWallType = WallType.Door;
                    break;
            }

            to.DistanceFromStart = Math.Min(to.DistanceFromStart, from.DistanceFromStart + 1);

            return to;
        }

        public int CountNodesWithMinDistance(int distance)
        {
            return _allNodes.Count(n => n.DistanceFromStart >= distance);
        }

        public Node GetFurthestAway()
        {
            Node furthest = null;

            foreach (Node node in _allNodes)
            {
                if (furthest == null || furthest.DistanceFromStart < node.DistanceFromStart)
                {
                    furthest = node;
                }
            }

            return furthest;
        }

        public void MakeUnknownsWalls()
        {
            foreach (Node node in _allNodes)
            {
                if (node.RightWallType == WallType.Unknown) node.RightWallType = WallType.Wall;
                if (node.BottomWallType == WallType.Unknown) node.BottomWallType = WallType.Wall;
            }
        }

        public void Draw()
        {
            int minX = int.MaxValue, minY = int.MaxValue, maxX = int.MinValue, maxY = int.MinValue;

            for (int y = 0; y < GridSize; y++)
            {
                for (int x = 0; x < GridSize; x++)
                {
                    if (_space[x, y] != null)
                    {
                        minX = Math.Min(minX, x);
                        minY = Math.Min(minY, y);
                        maxX = Math.Max(maxX, x);
                        maxY = Math.Max(maxY, y);
                    }
                }
            }

            for (int y = minY; y <= maxY; y++)
            {
                if (y == minY) Console.WriteLine(new string('#', 2 * (maxX - minX + 1) + 1));

                for (int x = minX; x <= maxX; x++)
                {
                    if (x == minX) Console.Write('#');
                    if (_space[x, y] != null)
                    {
                        Console.Write(x == OffsetX && y == OffsetY ? 'X' : '.');
                        switch (_space[x, y].RightWallType)
                        {
                            case WallType.Door: Console.Write('|'); break;
                            case WallType.Wall: Console.Write('#'); break;
                            case WallType.Unknown: Console.Write('?'); break;
                        }
                    }
                    else
                    {
                        Console.Write(' ');
                        Console.Write('?');
                    }
                }
                Console.WriteLine();

                for (int x = minX; x <= maxX; x++)
                {
                    if (x == minX) Console.Write('#');
                    if (_space[x, y] != null)
                    {
                        switch (_space[x, y].BottomWallType)
                        {
                            case WallType.Door: Console.Write('-'); break;
                            case WallType.Wall: Console.Write('#'); break;
                            case WallType.Unknown: Console.Write('?'); break;
                        }
                    }
                    else
                    {
                        Console.Write('#');
                    }

                    Console.Write('#');
                }
                Console.WriteLine();
            }
        }
    }

    class Program
    {
        private static Space _space = new Space();

        static void Main(string[] args)
        {
            //string input = "^ENNWSWW(NEWS|)SSSEEN(WNSE|)EE(SWEN|)NNN$";
            //string input = "^WSSEESWWWNW(S|NENNEEEENN(ESSSSW(NWSW|SSEN)|WSWWN(E|WWS(E|SS))))$";
            //string input = "^ESSWWN(E|NNENN(EESS(WNSE|)SSS|WWWSSSSE(SW|NNNE)))$";
            string input = File.ReadAllText("input.txt");

            Console.WriteLine(input);
            Console.WriteLine();

            WalkRegex(input.Substring(1, input.Length - 2), 0, 0);
            _space.MakeUnknownsWalls();
            _space.Draw();
            Console.WriteLine(_space.GetFurthestAway().DistanceFromStart);
            Console.WriteLine(_space.CountNodesWithMinDistance(1000));
            Console.ReadLine();
        }

        static void WalkRegex(string exp, int x, int y)
        {
            var workQueue = new Queue<(string exp, int x, int y)>();
            workQueue.Enqueue((exp, x , y));

            while (workQueue.Count > 0)
            {
                var state = workQueue.Dequeue();
                Node currentNode = _space.GetNodeAt(state.x, state.y);

                bool branched = false;

                for (int i = 0; i < state.exp.Length && !branched; i++)
                {
                    switch (state.exp[i])
                    {
                        case '(':
                            var (options, consumed) = ParseBranch(state.exp.Substring(i));
                            string remaining = state.exp.Substring(i + consumed);
                            if (options.Contains(""))
                            {
                                // Detour, all options come back to current node
                                foreach (string detour in options.Where(d => d.Length > 0))
                                {
                                    workQueue.Enqueue((detour, currentNode.X, currentNode.Y));
                                }
                                i += (consumed - 1);
                            }
                            else
                            {
                                foreach (string prefix in options)
                                {
                                    workQueue.Enqueue((prefix + remaining, currentNode.X, currentNode.Y));
                                }
                                branched = true;
                            }
                            break;

                        case 'N':
                            currentNode = _space.Move(Direction.Up, currentNode);
                            break;

                        case 'E':
                            currentNode = _space.Move(Direction.Right, currentNode);
                            break;

                        case 'S':
                            currentNode = _space.Move(Direction.Down, currentNode);
                            break;

                        case 'W':
                            currentNode = _space.Move(Direction.Left, currentNode);
                            break;
                    }
                }
            }
        }

        static (IList<string> options, int consumed) ParseBranch(string branch)
        {
            int nestedBranches = 0;
            int lastSeparatorPos = 0;
            List<string> options = new List<string>();

            for (int i = 1; i < branch.Length; i++)
            {
                switch (branch[i])
                {
                    case '(':
                        nestedBranches++;
                        break;

                    case ')':
                        if (nestedBranches-- == 0)
                        {
                            options.Add(new string(branch.Substring(lastSeparatorPos + 1, i - lastSeparatorPos - 1)));
                            return (options, i + 1);
                        }
                        break;

                    case '|':
                        if (nestedBranches == 0)
                        {
                            options.Add(new string(branch.Substring(lastSeparatorPos + 1, i - lastSeparatorPos - 1)));
                            lastSeparatorPos = i;
                        }
                        break;
                }
            }

            throw new ArgumentException("No closing parenthesis found");
        }
    }
}
