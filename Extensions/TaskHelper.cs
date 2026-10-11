using System;
using System.Threading.Tasks;

namespace StarQ.Shared.Extensions
{
    public static class TaskHelper
    {
        public static async void DelayAndAction(Action action, int sec = 5000)
        {
            await DelaySec(sec);
            action?.Invoke();
        }

        public static async Task DelaySec(int sec = 5000)
        {
            await Task.Delay(sec);
        }
    }
}
