using System;
using System.Drawing;
using System.Windows.Forms;
using System.Globalization;
using DesktopApp.Core.Services;
using DesktopApp.Core.Entities;
using DesktopApp.Common.Constants;
using DesktopApp.Infrastructure.Logging;
using DesktopApp.Presentation.Controls;

namespace DesktopApp.Presentation.Forms;

public class MainForm : Form
{
    private readonly ProductService _productService;
    private readonly StockMovementService _stockMovementService;
    private ModernButton _addProductButton = null!;
    private ModernButton _addStockMovementButton = null!;
    private ModernButton _deleteProductButton = null!;
    private ModernButton _toggleThemeButton = null!;
    private ModernButton _toggleLanguageButton = null!;
    private DataGridView _productGrid = null!;
    private Panel _headerPanel = null!;
    private Label _titleLabel = null!;

    public MainForm(
        ProductService productService,
        StockMovementService stockMovementService)
    {
        _productService = productService ?? throw new ArgumentNullException(nameof(productService));
        _stockMovementService = stockMovementService ?? throw new ArgumentNullException(nameof(stockMovementService));

        InitializeComponents();
        LoadProducts();
        ApplyTheme();
    }

    private void InitializeComponents()
    {
        // Form properties
        Text = AppStrings.MainForm.Title;
        Size = new Size(1024, 768);
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;

        // Create header panel
        _headerPanel = new Panel
        {
            Dock = DockStyle.Top,
            Height = 60
        };

        // Create title label
        _titleLabel = new Label
        {
            Text = AppStrings.MainForm.Title,
            Font = new Font("Segoe UI", 16, FontStyle.Bold),
            AutoSize = true,
            Location = new Point(Constants.UI.PADDING, 15)
        };
        _headerPanel.Controls.Add(_titleLabel);

        // Create theme toggle button
        _toggleThemeButton = new ModernButton
        {
            Text = "🌙",
            Size = new Size(40, 40),
            Location = new Point(_headerPanel.Width - 100, 10),
            Anchor = AnchorStyles.Right | AnchorStyles.Top
        };
        _toggleThemeButton.Click += (s, e) =>
        {
            ThemeColors.Current.IsDarkMode = !ThemeColors.Current.IsDarkMode;
            ApplyTheme();
        };
        _headerPanel.Controls.Add(_toggleThemeButton);

        // Create language toggle button
        _toggleLanguageButton = new ModernButton
        {
            Text = AppStrings.IsEnglish ? "TR" : "EN",
            Size = new Size(40, 40),
            Location = new Point(_headerPanel.Width - 50, 10),
            Anchor = AnchorStyles.Right | AnchorStyles.Top
        };
        _toggleLanguageButton.Click += (s, e) =>
        {
            AppStrings.IsEnglish = !AppStrings.IsEnglish;
            ApplyLanguage();
        };
        _headerPanel.Controls.Add(_toggleLanguageButton);

        // Create button panel
        var buttonPanel = new Panel
        {
            Dock = DockStyle.Top,
            Height = 60,
            Padding = new Padding(Constants.UI.PADDING)
        };

        // Create buttons
        _addProductButton = new ModernButton
        {
            Text = AppStrings.MainForm.AddProduct,
            Size = new Size(150, 40),
            Location = new Point(Constants.UI.PADDING, 10)
        };
        _addProductButton.Click += AddProductButton_Click!;

        _addStockMovementButton = new ModernButton
        {
            Text = AppStrings.MainForm.AddStockMovement,
            Size = new Size(150, 40),
            Location = new Point(Constants.UI.PADDING * 2 + 150, 10)
        };
        _addStockMovementButton.Click += AddStockMovementButton_Click!;

        _deleteProductButton = new ModernButton
        {
            Text = AppStrings.MainForm.DeleteProduct,
            Size = new Size(150, 40),
            Location = new Point(Constants.UI.PADDING * 3 + 300, 10),
            Enabled = false
        };
        _deleteProductButton.Click += DeleteProductButton_Click!;

        buttonPanel.Controls.AddRange(new Control[] { _addProductButton, _addStockMovementButton, _deleteProductButton });

        // Create and style product grid
        _productGrid = new DataGridView
        {
            Dock = DockStyle.Fill,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            MultiSelect = false,
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            ReadOnly = true,
            BorderStyle = BorderStyle.None,
            RowHeadersVisible = false,
            EnableHeadersVisualStyles = false,
            AllowUserToResizeRows = false
        };

        // Configure grid columns and events
        _productGrid.AutoGenerateColumns = false;
        _productGrid.SelectionChanged += ProductGrid_SelectionChanged!;
        _productGrid.CellDoubleClick += ProductGrid_CellDoubleClick!;
        _productGrid.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = "Name",
            HeaderText = AppStrings.MainForm.Grid.ProductName,
            Name = "Name",
            MinimumWidth = 150
        });
        _productGrid.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = "Description",
            HeaderText = AppStrings.MainForm.Grid.Description,
            Name = "Description"
        });
        _productGrid.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = "Price",
            HeaderText = AppStrings.MainForm.Grid.Price,
            Name = "Price",
            MinimumWidth = 100,
            DefaultCellStyle = new DataGridViewCellStyle
            {
                Format = "N2",
                FormatProvider = CultureInfo.GetCultureInfo("tr-TR"),
                Alignment = DataGridViewContentAlignment.MiddleRight
            }
        });
        _productGrid.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = "CurrentStock",
            HeaderText = AppStrings.MainForm.Grid.CurrentStock,
            Name = "CurrentStock",
            MinimumWidth = 100,
            DefaultCellStyle = new DataGridViewCellStyle
            {
                Alignment = DataGridViewContentAlignment.MiddleCenter
            }
        });

        // Create main container panel
        var mainContainer = new Panel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(Constants.UI.PADDING)
        };
        mainContainer.Controls.Add(_productGrid);

        // Add all panels to form
        Controls.AddRange(new Control[] {
            mainContainer,
            buttonPanel,
            _headerPanel
        });
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
        _addProductButton.BackColor = ThemeColors.Current.AccentColor;
        _addStockMovementButton.BackColor = ThemeColors.Current.SecondaryColor;
        _deleteProductButton.BackColor = ThemeColors.Current.DangerColor;
        _toggleThemeButton.BackColor = ThemeColors.Current.SecondaryColor;
        _toggleThemeButton.Text = ThemeColors.Current.IsDarkMode ? "☀️" : "🌙";

        // Update grid colors
        _productGrid.BackgroundColor = ThemeColors.Current.SurfaceColor;
        _productGrid.DefaultCellStyle.BackColor = ThemeColors.Current.SurfaceColor;
        _productGrid.DefaultCellStyle.ForeColor = ThemeColors.Current.TextColor;
        _productGrid.DefaultCellStyle.SelectionBackColor = ThemeColors.Current.PrimaryColor;
        _productGrid.DefaultCellStyle.SelectionForeColor = Color.White;
        _productGrid.ColumnHeadersDefaultCellStyle.BackColor = ThemeColors.Current.PrimaryColor;
        _productGrid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        _productGrid.AlternatingRowsDefaultCellStyle.BackColor = 
            ThemeColors.Current.IsDarkMode ? Color.FromArgb(45, 51, 59) : Color.FromArgb(245, 247, 249);

        Refresh();
    }

    private async void LoadProducts()
    {
        try
        {
            var products = await _productService.GetAllAsync();
            _productGrid.DataSource = products;

            // Format price column to show TL
            if (_productGrid.Columns["Price"] is DataGridViewColumn priceColumn)
            {
                foreach (DataGridViewRow row in _productGrid.Rows)
                {
                    if (row.Cells["Price"].Value != null)
                    {
                        var price = Convert.ToDecimal(row.Cells["Price"].Value);
                        row.Cells["Price"].Value = price;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Logger.LogError($"Error loading products: {ex.Message}");
            MessageBox.Show(
                AppStrings.IsEnglish ? $"Error loading products: {ex.Message}" : $"Ürünler yüklenirken hata oluştu: {ex.Message}",
                AppStrings.MainForm.Error,
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void AddProductButton_Click(object sender, EventArgs e)
    {
        try
        {
            using var form = new ProductForm(null, _productService);
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadProducts();
            }
        }
        catch (Exception ex)
        {
            Logger.LogError($"Error showing product form: {ex.Message}");
            MessageBox.Show($"Error showing product form: {ex.Message}", 
                "Error", 
                MessageBoxButtons.OK, 
                MessageBoxIcon.Error);
        }
    }

    private void AddStockMovementButton_Click(object sender, EventArgs e)
    {
        try
        {
            if (_productGrid.SelectedRows.Count == 0)
            {
                MessageBox.Show(AppStrings.MainForm.SelectProduct, 
                    AppStrings.MainForm.Warning, 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Warning);
                return;
            }

            var product = (Product)_productGrid.SelectedRows[0].DataBoundItem;
            using var form = new StockMovementForm(product, _stockMovementService);
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadProducts();
            }
        }
        catch (Exception ex)
        {
            Logger.LogError($"Error showing stock movement form: {ex.Message}");
            MessageBox.Show($"Error showing stock movement form: {ex.Message}", 
                "Error", 
                MessageBoxButtons.OK, 
                MessageBoxIcon.Error);
        }
    }

    private void ProductGrid_SelectionChanged(object sender, EventArgs e)
    {
        _deleteProductButton.Enabled = _productGrid.SelectedRows.Count > 0;
        _addStockMovementButton.Enabled = _productGrid.SelectedRows.Count > 0;
    }

    private async void DeleteProductButton_Click(object sender, EventArgs e)
    {
        if (_productGrid.SelectedRows.Count == 0) return;

        var product = _productGrid.SelectedRows[0].DataBoundItem as Product;
        if (product == null) return;

        var result = MessageBox.Show(
            string.Format(AppStrings.MainForm.ConfirmDelete, product.Name),
            AppStrings.MainForm.ConfirmDeleteTitle,
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning);

        if (result == DialogResult.Yes)
        {
            try
            {
                await _productService.DeleteAsync(product.Id);
                LoadProducts();
            }
            catch (Exception ex)
            {
                Logger.LogError($"Error deleting product: {ex.Message}");
                MessageBox.Show($"Error deleting product: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }

    private void ApplyLanguage()
    {
        // Update form title
        Text = AppStrings.MainForm.Title;
        _titleLabel.Text = AppStrings.MainForm.Title;

        // Update buttons
        _addProductButton.Text = AppStrings.MainForm.AddProduct;
        _addStockMovementButton.Text = AppStrings.MainForm.AddStockMovement;
        _deleteProductButton.Text = AppStrings.MainForm.DeleteProduct;
        _toggleLanguageButton.Text = AppStrings.IsEnglish ? "TR" : "EN";

        // Update grid columns
        if (_productGrid.Columns["Name"] is DataGridViewColumn nameColumn)
            nameColumn.HeaderText = AppStrings.MainForm.Grid.ProductName;
        if (_productGrid.Columns["Description"] is DataGridViewColumn descColumn)
            descColumn.HeaderText = AppStrings.MainForm.Grid.Description;
        if (_productGrid.Columns["Price"] is DataGridViewColumn priceColumn)
            priceColumn.HeaderText = AppStrings.MainForm.Grid.Price;
        if (_productGrid.Columns["CurrentStock"] is DataGridViewColumn currentStockColumn)
            currentStockColumn.HeaderText = AppStrings.MainForm.Grid.CurrentStock;

        // Refresh the form
        Refresh();
    }

    private void ProductGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;

        try
        {
            var product = (Product)_productGrid.Rows[e.RowIndex].DataBoundItem;
            using var form = new ProductForm(product, _productService);
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadProducts();
            }
        }
        catch (Exception ex)
        {
            Logger.LogError($"Error showing product form: {ex.Message}");
            MessageBox.Show(
                AppStrings.IsEnglish ? $"Error showing product form: {ex.Message}" : $"Ürün formu gösterilirken hata oluştu: {ex.Message}",
                AppStrings.MainForm.Error,
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _addProductButton?.Dispose();
            _addStockMovementButton?.Dispose();
            _deleteProductButton?.Dispose();
            _productGrid?.Dispose();
            _headerPanel?.Dispose();
            _titleLabel?.Dispose();
        }
        base.Dispose(disposing);
    }
}
