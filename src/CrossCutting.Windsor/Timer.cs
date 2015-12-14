using System;
using System.Diagnostics;

namespace CrossCutting.Diagnostics
{
    static class Timer
    {
        public static TimeSpan Time(Action actionToTime)
        {
            var stopWatch = new Stopwatch();

            stopWatch.Start();

            actionToTime();

            stopWatch.Stop();

            return stopWatch.Elapsed;
        }
    }
}