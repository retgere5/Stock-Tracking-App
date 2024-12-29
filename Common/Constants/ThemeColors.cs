using System.Drawing;

namespace DesktopApp.Common.Constants;

public static class ThemeColors
{
    public static class Light
    {
        public static readonly Color PrimaryColor = Color.FromArgb(52, 152, 219);  // Blue
        public static readonly Color SecondaryColor = Color.FromArgb(41, 128, 185); // Darker Blue
        public static readonly Color AccentColor = Color.FromArgb(46, 204, 113);   // Green
        public static readonly Color BackgroundColor = Color.FromArgb(236, 240, 241); // Light Gray
        public static readonly Color TextColor = Color.FromArgb(52, 73, 94);       // Dark Gray
        public static readonly Color SurfaceColor = Color.White;
        public static readonly Color DangerColor = Color.FromArgb(231, 76, 60);    // Red
    }

    public static class Dark
    {
        public static readonly Color PrimaryColor = Color.FromArgb(41, 128, 185);  // Darker Blue
        public static readonly Color SecondaryColor = Color.FromArgb(52, 152, 219); // Blue
        public static readonly Color AccentColor = Color.FromArgb(46, 204, 113);   // Green
        public static readonly Color BackgroundColor = Color.FromArgb(34, 40, 49); // Dark Gray
        public static readonly Color TextColor = Color.FromArgb(236, 240, 241);    // Light Gray
        public static readonly Color SurfaceColor = Color.FromArgb(57, 62, 70);    // Lighter Dark Gray
        public static readonly Color DangerColor = Color.FromArgb(231, 76, 60);    // Red
    }

    public static class Current
    {
        public static bool IsDarkMode { get; set; } = false;

        public static Color PrimaryColor => IsDarkMode ? Dark.PrimaryColor : Light.PrimaryColor;
        public static Color SecondaryColor => IsDarkMode ? Dark.SecondaryColor : Light.SecondaryColor;
        public static Color AccentColor => IsDarkMode ? Dark.AccentColor : Light.AccentColor;
        public static Color BackgroundColor => IsDarkMode ? Dark.BackgroundColor : Light.BackgroundColor;
        public static Color TextColor => IsDarkMode ? Dark.TextColor : Light.TextColor;
        public static Color SurfaceColor => IsDarkMode ? Dark.SurfaceColor : Light.SurfaceColor;
        public static Color DangerColor => IsDarkMode ? Dark.DangerColor : Light.DangerColor;
    }
} 