using System;
using NextCanvas.ViewModels;

namespace NextCanvas
{
    public static class LogManager
    {
        // TODO: Saving to file?
        public static LogViewModel Log { get; } = new LogViewModel();

        public static void AddLogItem(string item)
        {
            Log.LogString += $"[{DateTime.Now.ToLongTimeString()}] {item}";
        }
    }
}
