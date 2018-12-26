using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Op
{
    public string Name;
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
        string[] input = File.ReadAllLines("input.txt");
        int ipReg = int.Parse(input[0].Substring(4));
        int ip = 0;
        var program = new List<(string op, int v1, int v2, int v3)>();
        int[] regs = new int[6];
        Dictionary<string, Op> opcodes = new Dictionary<string, Op>();
        foreach (var op in opcodeslist) opcodes.Add(op.Name, op);

        //regs[0] = 15883666;

        HashSet<int> haltingValues = new HashSet<int>();
        int prev = 0;

        foreach (string instr in input.Skip(1))
        {
            string[] parts = instr.Split(' ');
            program.Add((parts[0], int.Parse(parts[1]), int.Parse(parts[2]), int.Parse(parts[3])));
        }

        while (true)
        {
            regs[ipReg] = ip;
            var instr = program[ip];

            // Instruction 28 is the only place register 0 is being used. Printing the state tells us which value the eqrr instruction (eqrr 1 0 3) compares to first.
            // Find first halting value (part 1) and last before they start repeating (part 2).
            if (ip == 28)
            {
                int r1 = regs[1];

                if (haltingValues.Contains(r1))
                {
                    Console.WriteLine("Halting values repeating. Last value: " + prev);
                    break;
                }
                else
                {
                    if (haltingValues.Count == 0)
                    {
                        Console.WriteLine("First halting value: " + r1);
                    }
                    haltingValues.Add(r1);
                }
                //Console.Write($"ip={ip} [{regs[0]},{regs[1]},{regs[2]},{regs[3]},{regs[4]},{regs[5]}] {instr.op} {instr.v1} {instr.v2} {instr.v3} ");
                prev = r1;
            }
            opcodes[instr.op].Handler(regs, instr.v1, instr.v2, instr.v3);
            //if (ip == 28) Console.WriteLine($"[{regs[0]},{regs[1]},{regs[2]},{regs[3]},{regs[4]},{regs[5]}]");
            ip = regs[ipReg] + 1;
            if (ip < 0 || ip >= program.Count) break;
        }

        Console.WriteLine("halted");
        Console.ReadLine();
    }
}
