﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Op
{
    public string Name;
    public int Code;
    public Action<int[], int, int, int> Handler;
}

class Program
{
    static List<Op> opcodeslist = new List<Op>()
    {
        new Op() {Name = "addr", Handler = (reg, a, b, c) => reg[c] = reg[a] + reg[b] },            // addr reg reg reg
        new Op() {Name = "addi", Handler = (reg, a, b, c) => reg[c] = reg[a] + b },                 // addi reg val reg
        new Op() {Name = "mulr", Handler = (reg, a, b, c) => reg[c] = reg[a] * reg[b] },            // mulr reg reg reg
        new Op() {Name = "muli", Handler = (reg, a, b, c) => reg[c] = reg[a] * b },                 // muli reg val reg
        new Op() {Name = "banr", Handler = (reg, a, b, c) => reg[c] = reg[a] & reg[b] },            // banr reg reg reg
        new Op() {Name = "bani", Handler = (reg, a, b, c) => reg[c] = reg[a] & b },                 // bani reg val reg
        new Op() {Name = "borr", Handler = (reg, a, b, c) => reg[c] = reg[a] | reg[b] },            // borr reg reg reg
        new Op() {Name = "bori", Handler = (reg, a, b, c) => reg[c] = reg[a] | b },                 // bori reg val reg
        new Op() {Name = "setr", Handler = (reg, a, b, c) => reg[c] = reg[a] },                     // setr reg - reg
        new Op() {Name = "seti", Handler = (reg, a, b, c) => reg[c] = a },                          // seti val - reg
        new Op() {Name = "gtir", Handler = (reg, a, b, c) => reg[c] = a > reg[b] ? 1 : 0 },         // gtir val reg reg
        new Op() {Name = "gtri", Handler = (reg, a, b, c) => reg[c] = reg[a] > b ? 1 : 0 },         // gtri reg val reg
        new Op() {Name = "gtrr", Handler = (reg, a, b, c) => reg[c] = reg[a] > reg[b] ? 1 : 0 },    // gtrr reg reg reg
        new Op() {Name = "eqir", Handler = (reg, a, b, c) => reg[c] = a == reg[b] ? 1 : 0 },        // eqir val reg reg
        new Op() {Name = "eqri", Handler = (reg, a, b, c) => reg[c] = reg[a] == b ? 1 : 0 },        // eqri reg val reg
        new Op() {Name = "eqrr", Handler = (reg, a, b, c) => reg[c] = reg[a] == reg[b] ? 1 : 0 },   // eqrr reg reg reg
    };

    static void Main(string[] args)
    {
        bool Part1 = false;
        string[] input = File.ReadAllLines("input.txt");
        int ipReg = int.Parse(input[0].Substring(4));
        int ip = 0;
        var program = new List<(string op, int v1, int v2, int v3)>();
        int[] regs = new int[6];
        int instructionLimit = int.MaxValue;

        Dictionary<string, Op> opcodes = new Dictionary<string, Op>();
        foreach (var op in opcodeslist) opcodes.Add(op.Name, op);

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
            opcodes[instr.op].Handler(regs, instr.v1, instr.v2, instr.v3);
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
}
