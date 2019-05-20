using System;
using System.Collections.Generic;
using System.Linq;
using Fibers;

namespace FibersManager
{
    static class ProcessManager
    {
        public static DispatchType Priority { get; set; }

        private static Dictionary<Fiber, bool> fibersWithMarks = new Dictionary<Fiber, bool>();
        private static Dictionary<Fiber, int> fibersWithPriorities = new Dictionary<Fiber, int>();
        private static Random switcher = new Random(DateTime.Now.Millisecond);
        private static Fiber currentFiber;
        
        public static void AddProcess(Process process)
        {
            Fiber fiberToAdd = new Fiber(process.Run);
            fibersWithMarks.Add(fiberToAdd, false);
            fibersWithPriorities.Add(fiberToAdd, process.Priority);
        }

        public static void Run()
        {
            currentFiber = fibersWithMarks.Last().Key;
            if (Priority == DispatchType.Priority)
            {
                currentFiber = GetNextPriorityFiber();
            }
            Switch(false);
        }

        public static void Dispose()
        {
            foreach (var fiber in fibersWithMarks)
            {
                if (!fiber.Key.IsPrimary)
                {
                    fiber.Key.Delete();
                }
            }
        }

        public static void Switch(bool isFinished)
        {
            fibersWithMarks[currentFiber] = isFinished;
            if (fibersWithMarks.ContainsValue(false))
            {
                if (Priority == DispatchType.NonPriority)
                {
                    currentFiber = GetNextRandomFiber();
                }
                else
                {
                    currentFiber = GetNextPriorityFiber();
                }
                Fiber.Switch(currentFiber.Id);
            }
            else
            {
                Fiber.Switch(Fiber.PrimaryId);
            }
        }

        private static Fiber GetNextRandomFiber()
        {
            var workingFibers = fibersWithMarks.Where(new Func<KeyValuePair<Fiber, bool>, bool>((f) => !f.Value && f.Key.Id != currentFiber.Id));
            if (workingFibers.Count() == 0)
            {
                return currentFiber;
            }
            else
            {
                return workingFibers.ElementAt(switcher.Next(workingFibers.Count())).Key;
            }
        }

        private static Fiber GetNextPriorityFiber()
        {
            var workingFibers = fibersWithMarks.Where(new Func<KeyValuePair<Fiber, bool>, bool>((f) => !f.Value && fibersWithPriorities[f.Key] != fibersWithPriorities[currentFiber]));
            if (workingFibers.Count() == 0)
            {
                workingFibers = fibersWithMarks.Where(new Func<KeyValuePair<Fiber, bool>, bool>((f) => !f.Value && f.Key.Id != currentFiber.Id));
            }
            if (workingFibers.Count() == 0)
            {
                return currentFiber;
            }
            Fiber result = workingFibers.First().Key;
            foreach (var f in workingFibers)
            {
                if ((fibersWithPriorities[result] < fibersWithPriorities[f.Key]) && !f.Key.Equals(currentFiber))
                {
                    result = f.Key;
                }
            }
            return result;
        }
    }
}
