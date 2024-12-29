using System.Drawing;
using System.Windows.Forms;
using DesktopApp.Common.Constants;

namespace DesktopApp.Presentation.Helpers;

/// <summary>
/// Helper class for UI operations
/// </summary>
public static class UIHelper
{
    /// <summary>
    /// Shows an error message box
    /// </summary>
    public static void ShowError(string message, string title = "Error")
    {
        MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    /// <summary>
    /// Shows a warning message box
    /// </summary>
    public static void ShowWarning(string message, string title = "Warning")
    {
        MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }

    /// <summary>
    /// Shows an information message box
    /// </summary>
    public static void ShowInfo(string message, string title = "Information")
    {
        MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    /// <summary>
    /// Shows a confirmation dialog
    /// </summary>
    public static bool Confirm(string message, string title = "Confirm")
    {
        return MessageBox.Show(message, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
    }

    /// <summary>
    /// Creates a standard button with common properties
    /// </summary>
    public static Button CreateButton(string text, Point location)
    {
        return new Button
        {
            Text = text,
            Location = location,
            Size = new Size(Constants.UI.CONTROL_WIDTH / 2, Constants.UI.BUTTON_HEIGHT)
        };
    }

    /// <summary>
    /// Creates a standard text box with common properties
    /// </summary>
    public static TextBox CreateTextBox(Point location, string placeholder = "")
    {
        return new TextBox
        {
            Location = location,
            Size = new Size(Constants.UI.CONTROL_WIDTH, Constants.UI.CONTROL_HEIGHT),
            PlaceholderText = placeholder
        };
    }

    /// <summary>
    /// Creates a standard label with common properties
    /// </summary>
    public static Label CreateLabel(string text, Point location)
    {
        return new Label
        {
            Text = text,
            Location = location,
            Size = new Size(Constants.UI.LABEL_WIDTH, Constants.UI.CONTROL_HEIGHT),
            TextAlign = ContentAlignment.MiddleRight
        };
    }

    /// <summary>
    /// Creates a standard data grid view with common properties
    /// </summary>
    public static DataGridView CreateDataGridView(Point location, Size size)
    {
        return new DataGridView
        {
            Location = location,
            Size = size,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            MultiSelect = false,
            AllowUserToAddRows = false,
            ReadOnly = true
        };
    }
} 