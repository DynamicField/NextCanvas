using System;
using System.Runtime.CompilerServices;
using NextCanvas.ViewModels;

namespace NextCanvas
{
    public static class LogManager
    {
        // TODO: Saving to file?
        public static LogViewModel Log { get; } = new LogViewModel();

        public static void AddLogItem(string item, [CallerMemberName] string sender = "Unknown", LogEntryStatus status = LogEntryStatus.Information)
        {
            var result = $"[{DateTime.Now:HH:mm:ss.ffff}] [{status}] {sender} -> {item}";
            Log.Append(result);
            if (status != LogEntryStatus.Information)
            {
                Log.Append(Environment.StackTrace);
            }
        }
    }

    public enum LogEntryStatus
    {
        Information,
        Warning,
        Error
    }
}
