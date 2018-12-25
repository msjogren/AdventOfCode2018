using System;
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
    static List<Op> opcodes = new List<Op>()
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
        new Op() {Name = "gtir", Handler = (reg, a, b, c) => reg[c] = reg[a] > b ? 1 : 0 },         // gtri reg val reg
        new Op() {Name = "gtrr", Handler = (reg, a, b, c) => reg[c] = reg[a] > reg[b] ? 1 : 0 },    // gtrr reg reg reg
        new Op() {Name = "eqir", Handler = (reg, a, b, c) => reg[c] = a == reg[b] ? 1 : 0 },        // eqir val reg reg
        new Op() {Name = "eqri", Handler = (reg, a, b, c) => reg[c] = reg[a] == b ? 1 : 0 },        // eqri reg val reg
        new Op() {Name = "eqrr", Handler = (reg, a, b, c) => reg[c] = reg[a] == reg[b] ? 1 : 0 },   // eqrr reg reg reg
    };

    static void Main(string[] args)
    {
        const bool Part1 = false;
        string[] input = File.ReadAllLines("input_part1.txt");
        int answerPart1 = 0;

        foreach (Op op in opcodes) op.Code = -1;

        for (int i = 0; i < input.Length; i += 4)
        {
            int[] beforeRegisters = ParseRegisters(input[i]);
            int[] ops = input[i + 1].Split(' ').Select(s => int.Parse(s)).ToArray();
            int[] afterRegisters = ParseRegisters(input[i + 2]);

            if (Part1)
            {
                var possibleOps = opcodes.Count(op =>
                {
                    int[] regs = (int[])beforeRegisters.Clone();
                    op.Handler(regs, ops[1], ops[2], ops[3]);
                    return regs.SequenceEqual(afterRegisters);
                });

                if (possibleOps >= 3) answerPart1++;
            }
            else
            {
                var possibleOps = opcodes.Where(op => op.Code < 0).Where(op =>
                {
                    int[] regs = (int[])beforeRegisters.Clone();
                    op.Handler(regs, ops[1], ops[2], ops[3]);
                    return regs.SequenceEqual(afterRegisters);
                });

                if (possibleOps.Count() == 1)
                {
                    var op = possibleOps.First();
                    op.Code = ops[0];
                    Console.WriteLine($"Opcode {ops[0]} is instruction {op.Name}.");
                }
            }
        }

        if (Part1)
        {
            Console.WriteLine(answerPart1);
        }
        else
        {
            opcodes.Sort((op1, op2) => op1.Code - op2.Code);

            int[] regs = new int[4];
            foreach (string line in File.ReadAllLines("input_part2.txt"))
            {
                int[] ops = line.Split(' ').Select(s => int.Parse(s)).ToArray();
                opcodes[ops[0]].Handler(regs, ops[1], ops[2], ops[3]);
            }
            Console.WriteLine(regs[0]);
        }

        Console.ReadLine();
    }

    static int[] ParseRegisters(string input)
    {
        int openBracket = input.IndexOf('[');
        int closeBracket = input.IndexOf(']', openBracket);
        return input.Substring(openBracket + 1, closeBracket - openBracket - 1)
                .Split(',')
                .Select(s => int.Parse(s.Trim()))
                .ToArray();
    }
}
