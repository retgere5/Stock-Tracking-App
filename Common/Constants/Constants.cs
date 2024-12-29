namespace DesktopApp.Common.Constants;

/// <summary>
/// Application-wide constants
/// </summary>
public static class Constants
{
    public static class UI
    {
        public const int PADDING = 10;
        public const int CONTROL_HEIGHT = 23;
        public const int BUTTON_HEIGHT = 30;
        public const int SPACING = 35;
        public const int LABEL_WIDTH = 120;
        public const int CONTROL_WIDTH = 200;
    }

    public static class Validation
    {
        public const decimal MIN_PRICE = 0.01M;
        public const int MIN_STOCK = 0;
        public const int MIN_STOCK_LEVEL = 1;
    }

    public static class Messages
    {
        public const string CONFIRM_DELETE = "Are you sure you want to delete {0}?";
        public const string ERROR_LOADING = "Error loading products";
        public const string ERROR_SAVING = "Error saving product: {0}";
        public const string ERROR_DELETING = "Error deleting product";
        public const string SELECT_TO_EDIT = "Please select a product to edit";
        public const string SELECT_TO_DELETE = "Please select a product to delete";
    }
} 