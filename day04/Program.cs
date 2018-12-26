using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    enum LogEntryKind { StartsShift, FallsAsleep, WakesUp }

    class GuardLogEntry
    {
        public DateTimeOffset Time { get; set; }
        public int Guard { get; set; }
        public LogEntryKind Kind { get; set; }
    }

    static void Main(string[] args)
    {
        List<GuardLogEntry> log = new List<GuardLogEntry>();

        foreach (string line in File.ReadAllLines("input.txt"))
        {
            GuardLogEntry entry = new GuardLogEntry() { Time = DateTimeOffset.Parse(line.Substring(1, 16)) };
            log.Add(entry);

            if (line.Contains("wakes up"))
            {
                entry.Kind = LogEntryKind.WakesUp;
            }
            else if (line.Contains("falls asleep"))
            {
                entry.Kind = LogEntryKind.FallsAsleep;
            }
            else
            {
                entry.Kind = LogEntryKind.StartsShift;
                int pound = line.IndexOf('#');
                int space = line.IndexOf(' ', pound);
                entry.Guard = int.Parse(line.Substring(pound + 1, space - pound - 1));
            }
        }

        log.Sort((e1, e2) => e1.Time.CompareTo(e2.Time));

        int currentGuard = -1;
        foreach (GuardLogEntry entry in log)
        {
            if (entry.Kind == LogEntryKind.StartsShift)
            {
                currentGuard = entry.Guard;
            }
            else
            {
                entry.Guard = currentGuard;
            }
        }

        Dictionary<int, int[]> minutesByGuard = new Dictionary<int, int[]>();
        for (int i = 2; i < log.Count; i++)
        {
            if (log[i].Kind == LogEntryKind.WakesUp)
            {
                int[] minutes;
                if (!minutesByGuard.TryGetValue(log[i].Guard, out minutes))
                {
                    minutes = new int[60];
                    minutesByGuard.Add(log[i].Guard, minutes);
                }

                for (int minute = log[i-1].Time.Minute; minute < log[i].Time.Minute; minute++)
                {
                    minutes[minute]++;
                }
            }
        }

        var guardMostAsleep = minutesByGuard.OrderByDescending(kvp => kvp.Value.Sum()).First();
        var mostAsleepMinute = Array.IndexOf(guardMostAsleep.Value, guardMostAsleep.Value.Max());
        
        Console.WriteLine($"[Part 1] Guard: A={guardMostAsleep.Key} Minute most asleep: B={mostAsleepMinute} AxB={guardMostAsleep.Key * mostAsleepMinute}");

        int part2Guard = -1;
        int part2Max = -1;
        int part2Minute = -1;

        foreach (var kvp in minutesByGuard)
        {
            int max = kvp.Value.Max();
            if (max > part2Max)
            {
                part2Max = max;
                part2Minute = Array.IndexOf(kvp.Value, max);
                part2Guard = kvp.Key;
            }
        }

        Console.WriteLine($"[Part 2] Guard: C={part2Guard} Minute most asleep: D={part2Minute} CxD={part2Guard * part2Minute}");
    }
}
