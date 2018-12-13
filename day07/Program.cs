using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    class Step
    {
        public string Name;
        public int Duration => Name[0] - 4;
        public List<Step> RunAfter = new List<Step>();
    }

    class Worker
    {
        public Step CurrentStep;
        public int BusyUntil = -1;
    }

    static void Main(string[] args)
    {
        const bool Part1 = false;

        Dictionary<string, Step> allSteps = new Dictionary<string, Step>();

        Step GetStepByName(string name)
        {
            if (!allSteps.TryGetValue(name, out Step n))
            {
                n = new Step() { Name = name };
                allSteps.Add(name, n);
            }

            return n;
        }

        foreach (string line in File.ReadAllLines("input.txt"))
        {
            Step requiredStep = GetStepByName(line.Substring(5, 1));
            Step currentStep = GetStepByName(line.Substring(36, 1));
            currentStep.RunAfter.Add(requiredStep);
        }

        if (Part1)
            SolvePart1(allSteps);
        else
            SolvePart2(allSteps);

        Console.ReadLine();
    }

    static void SolvePart1(Dictionary<string, Step> allSteps)
    {
        List<Step> remainingSteps = new List<Step>(allSteps.Values);
        HashSet<string> completedSteps = new HashSet<string>();
        string result = "";

        while (remainingSteps.Count > 0)
        {
            var availableSteps =
                from s in remainingSteps
                where s.RunAfter.All(a => completedSteps.Contains(a.Name))
                orderby s.Name
                select s;

            Step firstAvailable = availableSteps.First();
            completedSteps.Add(firstAvailable.Name);
            remainingSteps.Remove(firstAvailable);
            result += firstAvailable.Name;
        }

        Console.WriteLine(result);
    }

    static void SolvePart2(Dictionary<string, Step> allSteps)
    {
        List<Step> remainingSteps = new List<Step>(allSteps.Values);
        HashSet<string> completedSteps = new HashSet<string>();
        string result = "";
        int seconds = 0;
        Worker[] workers = { new Worker(), new Worker(), new Worker(), new Worker(), new Worker() };

        while (completedSteps.Count < allSteps.Count)
        {
            foreach (var completedWorker in workers.Where(w => w.BusyUntil == seconds))
            {
                completedSteps.Add(completedWorker.CurrentStep.Name);
                result += completedWorker.CurrentStep.Name;
                completedWorker.CurrentStep = null;
                completedWorker.BusyUntil = -1;
            }

            var freeWorkers = workers.Where(w => w.CurrentStep == null);

            if (freeWorkers.Any())
            {
                var availableSteps =
                    (from s in remainingSteps
                    where s.RunAfter.All(a => completedSteps.Contains(a.Name))
                    orderby s.Name
                    select s).ToList();

                foreach (var assignment in Enumerable.Zip(freeWorkers, availableSteps, (w, s) => (worker: w, step: s)))
                {
                    assignment.worker.CurrentStep = assignment.step;
                    assignment.worker.BusyUntil = seconds + assignment.step.Duration;
                    remainingSteps.Remove(assignment.step);
                }
            }

            seconds++;
        }

        Console.WriteLine($"{seconds - 1} {result}");

    }

}
