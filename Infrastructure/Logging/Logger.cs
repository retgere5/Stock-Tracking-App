namespace DesktopApp.Infrastructure.Logging;

/// <summary>
/// Logging class for application-wide use
/// </summary>
public static class Logger
{
    private static readonly string LogPath = "app.log";

    /// <summary>
    /// Log information message
    /// </summary>
    public static void LogInfo(string message)
    {
        Log("INFO", message);
    }

    /// <summary>
    /// Log error message
    /// </summary>
    public static void LogError(string message)
    {
        Log("ERROR", message);
    }

    /// <summary>
    /// Write to log file
    /// </summary>
    private static void Log(string level, string message)
    {
        string logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{level}] {message}";
        File.AppendAllText(LogPath, logMessage + Environment.NewLine);
    }
} 