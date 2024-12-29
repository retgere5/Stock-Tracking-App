using System;
using System.Drawing;
using System.Windows.Forms;
using System.Globalization;
using DesktopApp.Core.Entities;
using DesktopApp.Core.Services;
using DesktopApp.Common.Constants;
using DesktopApp.Infrastructure.Logging;
using DesktopApp.Presentation.Controls;

namespace DesktopApp.Presentation.Forms;

/// <summary>
/// Form for adding or editing products
/// </summary>
public class ProductForm : Form
{
    private readonly ProductService _productService;
    private readonly bool _isEditMode;

    // Form controls
    private Panel _headerPanel = null!;
    private Label _titleLabel = null!;
    private TextBox _nameBox = null!;
    private TextBox _descriptionBox = null!;
    private TextBox _priceBox = null!;
    private TextBox _stockBox = null!;
    private ModernButton _saveButton = null!;
    private ModernButton _cancelButton = null!;

    // Public property to access the product after form is closed
    public Product Product { get; private set; }

    public ProductForm(Product? product, ProductService productService)
    {
        _productService = productService ?? throw new ArgumentNullException(nameof(productService));
        Product = product ?? new Product();
        _isEditMode = product != null;

        InitializeComponents();
        if (_isEditMode)
        {
            LoadProductData();
        }
        ApplyTheme();
    }

    private void InitializeComponents()
    {
        // Form properties
        Text = _isEditMode ? AppStrings.ProductForm.EditTitle : AppStrings.ProductForm.AddTitle;
        Size = new Size(500, 450);
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;

        // Create header panel
        _headerPanel = new Panel
        {
            Dock = DockStyle.Top,
            Height = 60
        };

        // Create title label
        _titleLabel = new Label
        {
            Text = _isEditMode ? AppStrings.ProductForm.EditTitle : AppStrings.ProductForm.AddTitle,
            Font = new Font("Segoe UI", 16, FontStyle.Bold),
            AutoSize = true,
            Location = new Point(Constants.UI.PADDING, 15)
        };
        _headerPanel.Controls.Add(_titleLabel);

        // Create main panel
        var mainPanel = new Panel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(Constants.UI.PADDING * 2)
        };

        // Create input fields
        var y = Constants.UI.PADDING;
        CreateInputFields(mainPanel, ref y);

        // Create buttons panel
        var buttonPanel = new Panel
        {
            Dock = DockStyle.Bottom,
            Height = 60,
            Padding = new Padding(Constants.UI.PADDING)
        };

        // Create buttons
        _saveButton = new ModernButton
        {
            Text = AppStrings.ProductForm.Save,
            Size = new Size(120, 40),
            Anchor = AnchorStyles.Right,
            Location = new Point(buttonPanel.ClientSize.Width - 260, 10)
        };
        _saveButton.Click += SaveButton_Click!;

        _cancelButton = new ModernButton
        {
            Text = AppStrings.ProductForm.Cancel,
            Size = new Size(120, 40),
            Anchor = AnchorStyles.Right,
            Location = new Point(buttonPanel.ClientSize.Width - 140, 10),
            DialogResult = DialogResult.Cancel
        };

        buttonPanel.Controls.AddRange(new Control[] { _saveButton, _cancelButton });
        buttonPanel.Resize += (s, e) =>
        {
            _saveButton.Location = new Point(buttonPanel.ClientSize.Width - 260, 10);
            _cancelButton.Location = new Point(buttonPanel.ClientSize.Width - 140, 10);
        };

        // Add all panels to form
        Controls.AddRange(new Control[] { buttonPanel, mainPanel, _headerPanel });
    }

    private void ApplyTheme()
    {
        // Update form colors
        BackColor = ThemeColors.Current.BackgroundColor;
        ForeColor = ThemeColors.Current.TextColor;

        // Update header
        _headerPanel.BackColor = ThemeColors.Current.PrimaryColor;
        _titleLabel.ForeColor = Color.White;

        // Update buttons
        _saveButton.BackColor = ThemeColors.Current.AccentColor;
        _cancelButton.BackColor = ThemeColors.Current.SecondaryColor;

        // Update input fields
        foreach (Control control in Controls)
        {
            if (control is Panel panel)
            {
                panel.BackColor = ThemeColors.Current.SurfaceColor;
                foreach (Control child in panel.Controls)
                {
                    if (child is Label label)
                    {
                        label.ForeColor = ThemeColors.Current.TextColor;
                    }
                    else if (child is TextBox textBox)
                    {
                        textBox.BackColor = ThemeColors.Current.BackgroundColor;
                        textBox.ForeColor = ThemeColors.Current.TextColor;
                    }
                }
            }
        }

        Refresh();
    }

    private void CreateInputFields(Panel container, ref int y)
    {
        // Name
        CreateLabelAndTextBox(container, AppStrings.ProductForm.ProductName, ref _nameBox, ref y, true);

        // Description
        CreateLabelAndTextBox(container, AppStrings.ProductForm.Description, ref _descriptionBox, ref y, true, 80);
        _descriptionBox.Multiline = true;
        _descriptionBox.Height = 60;

        // Price
        CreateLabelAndTextBox(container, AppStrings.ProductForm.Price, ref _priceBox, ref y, true);
        _priceBox.TextChanged += (s, e) => FormatPriceInput();

        // Stock Quantity
        CreateLabelAndTextBox(container, AppStrings.ProductForm.CurrentStock, ref _stockBox, ref y, true);
        if (_isEditMode)
        {
            _stockBox.ReadOnly = true;
            _stockBox.BackColor = SystemColors.Control;
            var stockLabel = new Label
            {
                Text = AppStrings.IsEnglish ? "(Use stock movements to change stock)" : "(Stok değişimi için stok hareketlerini kullanın)",
                Font = new Font("Segoe UI", 8F, FontStyle.Italic),
                ForeColor = ThemeColors.Current.TextColor,
                Location = new Point(130, y),
                AutoSize = true
            };
            container.Controls.Add(stockLabel);
            y += 20;
        }
    }

    private void CreateLabelAndTextBox(Panel container, string labelText, ref TextBox textBox, ref int y, bool addSpacing = true, int spacing = 40)
    {
        var label = new Label
        {
            Text = labelText,
            Font = new Font("Segoe UI", 10F),
            ForeColor = ThemeColors.Current.TextColor,
            Location = new Point(0, y),
            Size = new Size(120, 20)
        };

        textBox = new TextBox
        {
            Location = new Point(130, y),
            Size = new Size(300, 25),
            Font = new Font("Segoe UI", 10F),
            BorderStyle = BorderStyle.FixedSingle
        };

        container.Controls.AddRange(new Control[] { label, textBox });

        if (addSpacing)
        {
            y += spacing;
        }
    }

    private void LoadProductData()
    {
        _nameBox.Text = Product.Name;
        _descriptionBox.Text = Product.Description;
        _priceBox.Text = Product.Price.ToString("N2", CultureInfo.GetCultureInfo("tr-TR"));
        _stockBox.Text = Product.CurrentStock.ToString();
    }

    private void FormatPriceInput()
    {
        if (string.IsNullOrWhiteSpace(_priceBox.Text)) return;

        string text = _priceBox.Text.Replace("TL", "").Replace(" ", "").Trim();
        
        if (decimal.TryParse(text, NumberStyles.Currency, CultureInfo.GetCultureInfo("tr-TR"), out decimal value))
        {
            int cursorFromEnd = _priceBox.TextLength - _priceBox.SelectionStart;
            _priceBox.Text = value.ToString("N2", CultureInfo.GetCultureInfo("tr-TR"));
            int newPosition = _priceBox.TextLength - cursorFromEnd;
            if (newPosition >= 0 && newPosition <= _priceBox.TextLength)
            {
                _priceBox.SelectionStart = newPosition;
            }
        }
    }

    private async void SaveButton_Click(object sender, EventArgs e)
    {
        if (!ValidateInput()) return;

        try
        {
            UpdateProductFromForm();

            if (_isEditMode)
            {
                await _productService.UpdateAsync(Product);
            }
            else
            {
                await _productService.AddAsync(Product);
            }

            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex)
        {
            Logger.LogError($"Error saving product: {ex.Message}");
            MessageBox.Show($"Error saving product: {ex.Message}", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void UpdateProductFromForm()
    {
        Product.Name = _nameBox.Text.Trim();
        Product.Description = _descriptionBox.Text.Trim();

        string priceText = _priceBox.Text.Replace("TL", "").Replace(" ", "").Trim();
        Product.Price = decimal.Parse(priceText, NumberStyles.Currency, CultureInfo.GetCultureInfo("tr-TR"));
        
        if (!_isEditMode)
        {
            Product.CurrentStock = int.Parse(_stockBox.Text);
        }
    }

    private bool ValidateInput()
    {
        if (string.IsNullOrWhiteSpace(_nameBox.Text))
        {
            MessageBox.Show(AppStrings.ProductForm.NameRequired, 
                AppStrings.ProductForm.ValidationError,
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        string priceText = _priceBox.Text.Replace("TL", "").Replace(" ", "").Trim();
        if (!decimal.TryParse(priceText, NumberStyles.Currency, CultureInfo.GetCultureInfo("tr-TR"), out decimal price) || price < 0)
        {
            MessageBox.Show(AppStrings.ProductForm.PricePositive, 
                AppStrings.ProductForm.ValidationError,
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        if (!_isEditMode)
        {
            if (!int.TryParse(_stockBox.Text, out int stock) || stock < 0)
            {
                MessageBox.Show(AppStrings.ProductForm.StockPositive, 
                    AppStrings.ProductForm.ValidationError,
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
        }

        return true;
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _headerPanel?.Dispose();
            _titleLabel?.Dispose();
            _nameBox?.Dispose();
            _descriptionBox?.Dispose();
            _priceBox?.Dispose();
            _stockBox?.Dispose();
            _saveButton?.Dispose();
            _cancelButton?.Dispose();
        }
        base.Dispose(disposing);
    }
} 