using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using DesktopApp.Common.Constants;

namespace DesktopApp.Presentation.Controls;

public class ModernButton : Button
{
    private bool _isHovered = false;
    private bool _isPressed = false;
    private readonly int _cornerRadius = 10;
    private readonly float _borderSize = 0;
    private readonly int _hoverColorShift = 20;
    private readonly int _pressedColorShift = 40;

    public ModernButton()
    {
        FlatStyle = FlatStyle.Flat;
        FlatAppearance.BorderSize = 0;
        Font = new Font("Segoe UI", 10F, FontStyle.Regular);
        Cursor = Cursors.Hand;
        BackColor = ThemeColors.Current.PrimaryColor;
        ForeColor = Color.White;
        Size = new Size(120, 40);
        
        SetStyle(
            ControlStyles.UserPaint |
            ControlStyles.AllPaintingInWmPaint |
            ControlStyles.OptimizedDoubleBuffer,
            true);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);

        var graphics = e.Graphics;
        graphics.SmoothingMode = SmoothingMode.AntiAlias;

        var rectSurface = ClientRectangle;
        var rectBorder = Rectangle.Inflate(rectSurface, -1, -1);

        // Get the button's current color based on state
        var buttonColor = GetButtonColor();

        using (var pathSurface = GetFigurePath(rectSurface, _cornerRadius))
        using (var pathBorder = GetFigurePath(rectBorder, _cornerRadius - 1))
        using (var penSurface = new Pen(Parent?.BackColor ?? ThemeColors.Current.BackgroundColor, 2))
        using (var penBorder = new Pen(buttonColor, _borderSize))
        {
            // Draw surface
            graphics.DrawPath(penSurface, pathSurface);

            // Draw the button background
            using (var brush = new SolidBrush(buttonColor))
            {
                graphics.FillPath(brush, pathSurface);
            }

            // Draw border
            if (_borderSize > 0)
                graphics.DrawPath(penBorder, pathBorder);

            // Draw text
            var textRect = rectSurface;
            TextRenderer.DrawText(graphics, Text, Font, textRect,
                ForeColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }
    }

    private GraphicsPath GetFigurePath(Rectangle rect, int radius)
    {
        var path = new GraphicsPath();
        float curveSize = radius * 2F;

        path.StartFigure();
        path.AddArc(rect.X, rect.Y, curveSize, curveSize, 180, 90);
        path.AddArc(rect.Right - curveSize, rect.Y, curveSize, curveSize, 270, 90);
        path.AddArc(rect.Right - curveSize, rect.Bottom - curveSize, curveSize, curveSize, 0, 90);
        path.AddArc(rect.X, rect.Bottom - curveSize, curveSize, curveSize, 90, 90);
        path.CloseFigure();

        return path;
    }

    private Color GetButtonColor()
    {
        var baseColor = BackColor;
        if (_isPressed)
            return ShiftColor(baseColor, -_pressedColorShift);
        if (_isHovered)
            return ShiftColor(baseColor, _hoverColorShift);
        return baseColor;
    }

    private Color ShiftColor(Color color, int shift)
    {
        return Color.FromArgb(
            color.A,
            Math.Max(0, Math.Min(255, color.R + shift)),
            Math.Max(0, Math.Min(255, color.G + shift)),
            Math.Max(0, Math.Min(255, color.B + shift))
        );
    }

    protected override void OnMouseEnter(EventArgs e)
    {
        _isHovered = true;
        Invalidate();
        base.OnMouseEnter(e);
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        _isHovered = false;
        Invalidate();
        base.OnMouseLeave(e);
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        _isPressed = true;
        Invalidate();
        base.OnMouseDown(e);
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        _isPressed = false;
        Invalidate();
        base.OnMouseUp(e);
    }

    protected override void OnHandleCreated(EventArgs e)
    {
        base.OnHandleCreated(e);
        if (Parent != null)
        {
            Parent.BackColorChanged += (s, ev) => Invalidate();
        }
    }
} 