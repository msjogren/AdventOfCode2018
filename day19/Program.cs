using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        bool Part1 = false;
        string[] input = File.ReadAllLines("input.txt");
        int ipReg = int.Parse(input[0].Substring(4));
        int ip = 0;
        var program = new List<(string op, int v1, int v2, int v3)>();
        int[] regs = new int[6];
        int instructionLimit = int.MaxValue;

        if (!Part1)
        {
            regs[0] = 1;
            instructionLimit = 20;
        }

        foreach (string instr in input.Skip(1))
        {
            string[] parts = instr.Split(' ');
            program.Add((parts[0], int.Parse(parts[1]), int.Parse(parts[2]), int.Parse(parts[3])));
        }

        while (instructionLimit-- > 0)
        {
            regs[ipReg] = ip;
            var instr = program[ip];
            //Console.Write($"ip={ip} [{regs[0]},{regs[1]},{regs[2]},{regs[3]},{regs[4]},{regs[5]}] {instr.op} {instr.v1} {instr.v2} {instr.v3} ");
            regs = EvaluateOpcodes(instr.op, instr.v1, instr.v2, instr.v3, regs);
            //Console.WriteLine($"[{regs[0]},{regs[1]},{regs[2]},{regs[3]},{regs[4]},{regs[5]}]");
            ip = regs[ipReg] + 1;
            if (ip < 0 || ip >= program.Count) break;
        }

        if (!Part1)
        {
            // Assembly code basically calculates sum of all factors of the number in regs[4]. Same slow imlementation as the following

            for (int x = 1; x <= regs[4]; x++)
            {
                for (int y = 1; y <= regs[4]; y++)
                {
                    if (x * y == regs[4]) regs[0] += x;
                }
            }

            // Which is faster to solve manually as
            // Sum of factors of 10 551 378 = 1 + 2 + 3 + 6 + 199 + 398 + 597 + 1194 + 8837 + 17674 + 26511 + 53022 + 1758563 + 3517126 + 5275689 + 10551378 = 21 211 200
        }

        Console.WriteLine(regs[0]);

        Console.ReadLine();
    }   

    static int[] EvaluateOpcodes(string opcode, int a, int b, int c, int[] registers)
    {
        int[] resultRegisters = new int[registers.Length];
        Array.Copy(registers, resultRegisters, registers.Length);

        switch (opcode)
        {
            case "eqri":
                // eqri reg val reg
                resultRegisters[c] = resultRegisters[a] == b ? 1 : 0;
                break;

            case "addr":
                // addr reg reg reg
                resultRegisters[c] = resultRegisters[a] + resultRegisters[b];
                break;

            case "eqrr":
                // eqrr reg reg reg
                resultRegisters[c] = resultRegisters[a] == resultRegisters[b] ? 1 : 0;
                break;

            case "setr":
                // setr reg - reg
                resultRegisters[c] = resultRegisters[a];
                break;

            case "gtir":
                // gtir val reg reg
                resultRegisters[c] = a > resultRegisters[b] ? 1 : 0;
                break;

            case "addi":
                // addi reg val reg
                resultRegisters[c] = resultRegisters[a] + b;
                break;

            case "muli":
                // muli reg val reg
                resultRegisters[c] = resultRegisters[a] * b;
                break;

            case "bani":
                // bani reg val reg
                resultRegisters[c] = resultRegisters[a] & b;
                break;

            case "bori":
                // bori reg val reg
                resultRegisters[c] = resultRegisters[a] | b;
                break;

            case "seti":
                // seti val - reg
                resultRegisters[c] = a;
                break;

            case "mulr":
                // mulr reg reg reg
                resultRegisters[c] = resultRegisters[a] * resultRegisters[b];
                break;

            case "gtri":
                // gtri reg val reg
                resultRegisters[c] = resultRegisters[a] > b ? 1 : 0;
                break;

            case "borr":
                // borr reg reg reg
                resultRegisters[c] = resultRegisters[a] | resultRegisters[b];
                break;

            case "gtrr":
                // gtrr reg reg reg
                resultRegisters[c] = resultRegisters[a] > resultRegisters[b] ? 1 : 0;
                break;

            case "eqir":
                // eqir val reg reg
                resultRegisters[c] = a == resultRegisters[b] ? 1 : 0;
                break;

            case "banr":
                // banr reg reg reg
                resultRegisters[c] = resultRegisters[a] & resultRegisters[b];
                break;
        }

        return resultRegisters;

    }
}
