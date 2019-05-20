using System;
using System.Collections.Generic;
using System.Threading;

namespace FibersManager
{
    public class Process
    {
        public int Priority { get; }

        private Random random = new Random(DateTime.Now.Millisecond);

        private int maxPauseTime = 1000;
        private int minPauseTime = 100;
        private int maxWorkingTime = 1000;
        private int maxPriorityLevel = 10;
        private int maxNumberOfWorkingIntervals = 10;

        private List<int> workingIntervals = new List<int>();
        private List<int> pauseIntervals = new List<int>();

        public Process()
        {
            Priority = random.Next(maxPriorityLevel);
            int numberOfIntervals = random.Next(maxNumberOfWorkingIntervals);
            
            for (int i = 0; i < numberOfIntervals; i++)
            {
                workingIntervals.Add(random.Next(maxWorkingTime));
                pauseIntervals.Add(random.Next(minPauseTime,maxPauseTime));
            }
        }

        public void Run()
        {
            for (int i = 0; i < workingIntervals.Count; ++i)
            {
                Thread.Sleep(workingIntervals[i]);
                var pauseStartTime = DateTime.Now;
                bool isInPause = true;
                while (isInPause)
                {
                    ProcessManager.Switch(false);
                    if ((DateTime.Now - pauseStartTime).TotalMilliseconds > pauseIntervals[i])
                    {
                        isInPause = false;
                    }
                }
                ProcessManager.Switch(true);
            }
        }
    }
}