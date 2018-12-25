using System;
using System.Collections.Generic;
using System.Linq;

class Group
{
    public string Army;
    public int Index;
    public int Units;
    public int HitPoints;
    public int AttackDamage;
    public string AttackType;
    public int Initiative;
    public string[] Weaknesses = new string[0];
    public string[] Immunities = new string[0];

    public int EffectivePower => Units * AttackDamage;
}

class Program
{
    static void Main(string[] args)
    {
        const bool Part1 = false;
        const int Boost = Part1 ? 0 : 29;   // Part 2 boost found by trial and error / manual binary search

        /*
        Group[] immunes = new[] {
            new Group() { Army = "Immune System", Index = 1, Units = 17, HitPoints = 5390, AttackDamage = 4507, AttackType = "fire", Initiative = 2, Weaknesses = new[] { "radiation", "bludgeoning" } },
            new Group() { Army = "Immune System", Index = 2, Units = 989, HitPoints = 1274 , AttackDamage = 25, AttackType = "slashing", Initiative = 3, Weaknesses = new[] { "bludgeoning", "slashing" }, Immunities = new[] { "fire" } }
        };

        Group[] infections = new[] {
            new Group() { Army = "Infection", Index = 1, Units = 801, HitPoints = 4706, AttackDamage = 116, AttackType = "bludgeoning", Initiative = 1, Weaknesses = new[] { "radiation" } },
            new Group() { Army = "Infection", Index = 2, Units = 4485, HitPoints = 2961 , AttackDamage = 12, AttackType = "slashing", Initiative = 4, Weaknesses = new[] { "fire", "cold" }, Immunities = new[] { "radiation" } }
        };
        */

        Group[] immunes = new[] {
            new Group() { Army = "Immune System", Index = 1, Units = 2321, HitPoints = 10326, AttackDamage = 42 + Boost, AttackType = "fire", Initiative = 4, Immunities = new[] { "slashing" } },
            new Group() { Army = "Immune System", Index = 2, Units = 2899, HitPoints = 9859, AttackDamage = 32 + Boost, AttackType = "slashing", Initiative = 11},
            new Group() { Army = "Immune System", Index = 3, Units = 4581, HitPoints = 7073, AttackDamage = 11 + Boost, AttackType = "radiation", Initiative = 9, Weaknesses = new[] { "slashing" } },
            new Group() { Army = "Immune System", Index = 4, Units = 5088, HitPoints = 7917, AttackDamage = 15 + Boost, AttackType = "fire", Initiative = 17, Weaknesses = new[] { "slashing" }, Immunities = new[] { "bludgeoning", "fire", "radiation" } },
            new Group() { Army = "Immune System", Index = 5, Units = 786, HitPoints = 1952, AttackDamage = 23 + Boost, AttackType = "slashing", Initiative = 16, Immunities = new[] { "fire", "bludgeoning", "slashing", "cold" } },
            new Group() { Army = "Immune System", Index = 6, Units = 3099, HitPoints = 7097, AttackDamage = 17 + Boost, AttackType = "radiation", Initiative = 8, Weaknesses = new[] { "bludgeoning" } },
            new Group() { Army = "Immune System", Index = 7, Units = 4604, HitPoints = 4901, AttackDamage = 8 + Boost, AttackType = "fire", Initiative = 13 },
            new Group() { Army = "Immune System", Index = 8, Units = 7079, HitPoints = 10328, AttackDamage = 14 + Boost, AttackType = "bludgeoning", Initiative = 18 },
            new Group() { Army = "Immune System", Index = 9, Units = 51, HitPoints = 11243, AttackDamage = 1872 + Boost, AttackType = "cold", Initiative = 15 },
            new Group() { Army = "Immune System", Index = 10, Units = 4910, HitPoints = 5381, AttackDamage = 10 + Boost, AttackType = "slashing", Initiative = 19, Weaknesses = new[] { "radiation" }, Immunities = new[] { "fire" } }
        };

        Group[] infections = new[] {
            new Group() { Army = "Infection", Index = 1, Units = 1758, HitPoints = 23776, AttackDamage = 24, AttackType = "radiation", Initiative = 2 },
            new Group() { Army = "Infection", Index = 2, Units = 4000, HitPoints = 12869, AttackDamage = 5, AttackType = "cold", Initiative = 14 },
            new Group() { Army = "Infection", Index = 3, Units = 2319, HitPoints = 43460, AttackDamage = 33, AttackType = "radiation", Initiative = 3, Weaknesses = new[] { "bludgeoning", "cold" } },
            new Group() { Army = "Infection", Index = 4, Units = 1898, HitPoints = 44204, AttackDamage = 39, AttackType = "radiation", Initiative = 1, Weaknesses = new[] { "radiation" }, Immunities = new[] { "cold" } },
            new Group() { Army = "Infection", Index = 5, Units = 2764, HitPoints = 50667, AttackDamage = 31, AttackType = "radiation", Initiative = 5, Weaknesses = new[] { "slashing", "radiation" } },
            new Group() { Army = "Infection", Index = 6, Units = 3046, HitPoints = 27907, AttackDamage = 16, AttackType = "slashing", Initiative = 7, Immunities = new[] { "radiation", "fire" } },
            new Group() { Army = "Infection", Index = 7, Units = 1379, HitPoints = 8469, AttackDamage = 8, AttackType = "cold", Initiative = 20, Immunities = new[] { "cold" } },
            new Group() { Army = "Infection", Index = 8, Units = 1824, HitPoints = 25625, AttackDamage = 23, AttackType = "radiation", Initiative = 6, Immunities = new[] { "bludgeoning" } },
            new Group() { Army = "Infection", Index = 9, Units = 115, HitPoints = 41114, AttackDamage = 686, AttackType = "slashing", Initiative = 10, Weaknesses = new[] { "slashing", "bludgeoning" }, Immunities = new[] { "fire" } },
            new Group() { Army = "Infection", Index = 10, Units = 4054, HitPoints = 51210, AttackDamage = 22, AttackType = "cold", Initiative = 12, Immunities = new[] { "radiation", "cold", "fire" } }
        };

        do
        {
            /*Console.WriteLine("Immune System:");
            foreach (Group g in immunes) Console.WriteLine($"Group {g.Index} contains {g.Units} units");
            Console.WriteLine("Infection:");
            foreach (Group g in infections) Console.WriteLine($"Group {g.Index} contains {g.Units} units");
            Console.WriteLine();*/

            var targets = TargetSelection(infections, immunes);
            var targets2 = TargetSelection(immunes, infections);
            //Console.WriteLine();

            foreach (var pair in targets.Concat(targets2).OrderByDescending(kvp => kvp.Key.Initiative))
            {
                int damage = Damage(pair.Key, pair.Value);
                pair.Value.Units = Math.Max(0, pair.Value.Units - (damage / pair.Value.HitPoints));
                //Console.WriteLine($"{pair.Key.Army} group {pair.Key.Index} attacks defending group {pair.Value.Index}, killing {damage / pair.Value.HitPoints} units");
            }
            //Console.WriteLine();
        }
        while (immunes.Any(g => g.Units > 0) && infections.Any(g => g.Units > 0));

        if (immunes.Any(g => g.Units > 0))
        {
            Console.WriteLine("Winning army (Immune System) units: " + immunes.Sum(g => g.Units));
        }
        else
        {
            Console.WriteLine("Winning army (Infection) units: " + infections.Sum(g => g.Units));
        }

        Console.ReadLine();
    }

    static Dictionary<Group, Group> TargetSelection(Group[] attackers, Group[] defenders)
    {
        var targets = new Dictionary<Group, Group>();
        var defenderSet = new HashSet<Group>(defenders);
        
        foreach (Group attacker in attackers.OrderByDescending(g => g.EffectivePower).ThenByDescending(g => g.Initiative))
        {
            //foreach (var def in defenders) if (attacker.Units > 0 && def.Units > 0) Console.WriteLine($"{attacker.Army} group {attacker.Index} would deal defending group {def.Index} {Damage(attacker, def)} damage");

            Group defender = defenderSet.Where(g => g.Units > 0).OrderByDescending(g => Damage(attacker, g)).ThenByDescending(g => g.EffectivePower).ThenByDescending(g => g.Initiative).FirstOrDefault(g => Damage(attacker, g) > 0);
            if (defender != null)
            {
                targets.Add(attacker, defender);
                defenderSet.Remove(defender);
            }
        }

        return targets;
    }

    static int Damage(Group attacker, Group defender)
    {
        if (defender.Immunities.Contains(attacker.AttackType))
        {
            return 0;
        }
        else if (defender.Weaknesses.Contains(attacker.AttackType))
        {
            return 2 * attacker.EffectivePower;
        }
        else
        {
            return attacker.EffectivePower;
        }
    }
}
