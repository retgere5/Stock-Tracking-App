using System;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using DesktopApp.Core.Entities;
using DesktopApp.Core.Services;
using DesktopApp.Common.Constants;
using DesktopApp.Infrastructure.Logging;
using DesktopApp.Presentation.Controls;

namespace DesktopApp.Presentation.Forms;

public class StockMovementForm : Form
{
    private readonly Product _product;
    private readonly StockMovementService _stockService;

    // Form controls
    private Panel _headerPanel = null!;
    private Label _titleLabel = null!;
    private ComboBox _typeBox = null!;
    private TextBox _quantityBox = null!;
    private TextBox _referenceBox = null!;
    private TextBox _notesBox = null!;
    private Button _addButton = null!;
    private Button _closeButton = null!;
    private DataGridView _movementGrid = null!;

    // Modern color scheme
    private static readonly Color PrimaryColor = Color.FromArgb(52, 152, 219);  // Blue
    private static readonly Color SecondaryColor = Color.FromArgb(41, 128, 185); // Darker Blue
    private static readonly Color AccentColor = Color.FromArgb(46, 204, 113);   // Green
    private static readonly Color BackgroundColor = Color.FromArgb(236, 240, 241); // Light Gray
    private static readonly Color TextColor = Color.FromArgb(52, 73, 94);       // Dark Gray

    public StockMovementForm(Product product, StockMovementService stockService)
    {
        _product = product ?? throw new ArgumentNullException(nameof(product));
        _stockService = stockService ?? throw new ArgumentNullException(nameof(stockService));

        InitializeComponents();
        LoadMovements();
    }

    private void InitializeComponents()
    {
        // Form properties
        Text = string.Format(AppStrings.StockMovementForm.Title, _product.Name);
        Size = new Size(800, 600);
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;

        // Create header panel
        _headerPanel = new Panel
        {
            Dock = DockStyle.Top,
            Height = 60,
            BackColor = ThemeColors.Current.PrimaryColor
        };

        // Create title label
        _titleLabel = new Label
        {
            Text = string.Format(AppStrings.StockMovementForm.Title, _product.Name),
            Font = new Font("Segoe UI", 16, FontStyle.Bold),
            ForeColor = Color.White,
            AutoSize = true,
            Location = new Point(Constants.UI.PADDING, 15)
        };
        _headerPanel.Controls.Add(_titleLabel);

        // Create product info panel
        var productInfoPanel = new Panel
        {
            Dock = DockStyle.Top,
            Height = 80,
            Padding = new Padding(Constants.UI.PADDING)
        };

        // Add product info labels
        var productNameLabel = new Label
        {
            Text = $"{AppStrings.ProductForm.ProductName} {_product.Name}",
            Font = new Font("Segoe UI", 10F, FontStyle.Bold),
            AutoSize = true,
            Location = new Point(Constants.UI.PADDING, 10)
        };

        var productStockLabel = new Label
        {
            Text = $"{AppStrings.ProductForm.CurrentStock} {_product.CurrentStock}",
            Font = new Font("Segoe UI", 10F),
            AutoSize = true,
            Location = new Point(Constants.UI.PADDING, 35)
        };

        productInfoPanel.Controls.AddRange(new Control[] { productNameLabel, productStockLabel });

        // Create input panel
        var inputPanel = new Panel
        {
            Dock = DockStyle.Top,
            Height = 180,
            Padding = new Padding(Constants.UI.PADDING * 2)
        };

        // Create input fields
        var y = Constants.UI.PADDING;
        CreateInputFields(inputPanel, ref y);

        // Create buttons panel
        var buttonPanel = new Panel
        {
            Dock = DockStyle.Bottom,
            Height = 60,
            Padding = new Padding(Constants.UI.PADDING)
        };

        // Create buttons
        _addButton = new ModernButton
        {
            Text = AppStrings.StockMovementForm.AddMovement,
            Size = new Size(120, 40),
            Anchor = AnchorStyles.Right,
            Location = new Point(buttonPanel.ClientSize.Width - 260, 10),
            BackColor = ThemeColors.Current.AccentColor
        };
        _addButton.Click += AddButton_Click!;

        _closeButton = new ModernButton
        {
            Text = AppStrings.StockMovementForm.Close,
            Size = new Size(120, 40),
            Anchor = AnchorStyles.Right,
            Location = new Point(buttonPanel.ClientSize.Width - 140, 10),
            BackColor = ThemeColors.Current.SecondaryColor
        };
        _closeButton.Click += (s, e) => Close();

        buttonPanel.Controls.AddRange(new Control[] { _addButton, _closeButton });

        // Create and style movement grid
        _movementGrid = new DataGridView
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
            AllowUserToResizeRows = false,
            BackgroundColor = ThemeColors.Current.SurfaceColor
        };

        // Configure grid columns
        ConfigureMovementGrid();

        // Create main container panel
        var mainContainer = new Panel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(Constants.UI.PADDING)
        };
        mainContainer.Controls.Add(_movementGrid);

        // Add all panels to form
        Controls.AddRange(new Control[] {
            buttonPanel,
            mainContainer,
            inputPanel,
            productInfoPanel,
            _headerPanel
        });

        ApplyTheme();
        LoadMovements();
    }

    private void ApplyTheme()
    {
        // Update form colors
        BackColor = ThemeColors.Current.BackgroundColor;
        ForeColor = ThemeColors.Current.TextColor;

        // Update header
        _headerPanel.BackColor = ThemeColors.Current.PrimaryColor;
        _titleLabel.ForeColor = Color.White;

        // Update panels
        foreach (Control control in Controls)
        {
            if (control is Panel panel)
            {
                panel.BackColor = ThemeColors.Current.SurfaceColor;
            }
        }

        // Update buttons
        _addButton.BackColor = ThemeColors.Current.AccentColor;
        _closeButton.BackColor = ThemeColors.Current.SecondaryColor;

        // Update grid colors
        _movementGrid.BackgroundColor = ThemeColors.Current.SurfaceColor;
        _movementGrid.DefaultCellStyle.BackColor = ThemeColors.Current.SurfaceColor;
        _movementGrid.DefaultCellStyle.ForeColor = ThemeColors.Current.TextColor;
        _movementGrid.DefaultCellStyle.SelectionBackColor = ThemeColors.Current.PrimaryColor;
        _movementGrid.DefaultCellStyle.SelectionForeColor = Color.White;
        _movementGrid.ColumnHeadersDefaultCellStyle.BackColor = ThemeColors.Current.PrimaryColor;
        _movementGrid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        _movementGrid.AlternatingRowsDefaultCellStyle.BackColor = 
            ThemeColors.Current.IsDarkMode ? Color.FromArgb(45, 51, 59) : Color.FromArgb(245, 247, 249);

        // Update input fields
        foreach (Control control in Controls)
        {
            if (control is Panel panel)
            {
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
                    else if (child is ComboBox comboBox)
                    {
                        comboBox.BackColor = ThemeColors.Current.BackgroundColor;
                        comboBox.ForeColor = ThemeColors.Current.TextColor;
                    }
                }
            }
        }

        Refresh();
    }

    private void CreateInputFields(Panel container, ref int y)
    {
        // Movement Type
        var typeLabel = new Label
        {
            Text = AppStrings.StockMovementForm.MovementType,
            Font = new Font("Segoe UI", 10F),
            ForeColor = ThemeColors.Current.TextColor,
            Location = new Point(0, y),
            Size = new Size(120, 20)
        };

        _typeBox = new ComboBox
        {
            Location = new Point(130, y),
            Size = new Size(200, 25),
            Font = new Font("Segoe UI", 10F),
            DropDownStyle = ComboBoxStyle.DropDownList,
            BackColor = ThemeColors.Current.BackgroundColor,
            ForeColor = ThemeColors.Current.TextColor
        };

        // Hareket tiplerini enum değerlerinden alıyoruz
        _typeBox.Items.Add(MovementType.In);
        _typeBox.Items.Add(MovementType.Out);
        _typeBox.Items.Add(MovementType.Adjustment);
        _typeBox.SelectedIndex = 0;

        // ComboBox için özel format
        _typeBox.Format += (s, e) =>
        {
            if (e.DesiredType != typeof(string)) return;
            if (e.Value is MovementType type)
            {
                e.Value = type switch
                {
                    MovementType.In => AppStrings.StockMovementForm.Types.In,
                    MovementType.Out => AppStrings.StockMovementForm.Types.Out,
                    MovementType.Adjustment => AppStrings.StockMovementForm.Types.Adjustment,
                    _ => type.ToString()
                };
            }
        };

        container.Controls.AddRange(new Control[] { typeLabel, _typeBox });
        y += 40;

        // Quantity
        CreateLabelAndTextBox(container, AppStrings.StockMovementForm.Quantity, ref _quantityBox, ref y);

        // Reference
        CreateLabelAndTextBox(container, AppStrings.StockMovementForm.Reference, ref _referenceBox, ref y);

        // Notes
        CreateLabelAndTextBox(container, AppStrings.StockMovementForm.Notes, ref _notesBox, ref y);
        _notesBox.Multiline = true;
        _notesBox.Height = 40;
    }

    private void CreateLabelAndTextBox(Panel container, string labelText, ref TextBox textBox, ref int y)
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
        y += 40;
    }

    private async void LoadMovements()
    {
        try
        {
            var movements = await _stockService.GetByProductAsync(_product.Id);
            var movementsList = movements.ToList();

            // DataGridView'a bağlamadan önce görüntüleme için bir kopya oluştur
            var displayData = movementsList.Select(m => new
            {
                m.Id,
                Type = GetMovementTypeDisplay(m.Type),
                m.Quantity,
                m.Reference,
                m.Notes,
                m.CreatedBy,
                m.CreatedAt
            }).ToList();

            _movementGrid.DataSource = displayData;

            // Color coding for movement types
            foreach (DataGridViewRow row in _movementGrid.Rows)
            {
                var typeText = row.Cells["Type"].Value?.ToString();
                if (typeText == AppStrings.StockMovementForm.Types.In)
                {
                    row.DefaultCellStyle.ForeColor = ThemeColors.Current.AccentColor;
                }
                else if (typeText == AppStrings.StockMovementForm.Types.Out)
                {
                    row.DefaultCellStyle.ForeColor = ThemeColors.Current.DangerColor;
                }
                else if (typeText == AppStrings.StockMovementForm.Types.Adjustment)
                {
                    row.DefaultCellStyle.ForeColor = ThemeColors.Current.SecondaryColor;
                }
            }
        }
        catch (Exception ex)
        {
            Logger.LogError($"Error loading movements: {ex.Message}");
            MessageBox.Show(
                AppStrings.IsEnglish ? $"Error loading stock movements: {ex.Message}" : $"Stok hareketleri yüklenirken hata oluştu: {ex.Message}",
                AppStrings.MainForm.Error,
                MessageBoxButtons.OK, 
                MessageBoxIcon.Error);
        }
    }

    private string GetMovementTypeDisplay(MovementType type)
    {
        return type switch
        {
            MovementType.In => AppStrings.StockMovementForm.Types.In,
            MovementType.Out => AppStrings.StockMovementForm.Types.Out,
            MovementType.Adjustment => AppStrings.StockMovementForm.Types.Adjustment,
            _ => type.ToString()
        };
    }

    private MovementType GetMovementTypeFromDisplay(string displayText)
    {
        if (displayText == AppStrings.StockMovementForm.Types.In)
            return MovementType.In;
        if (displayText == AppStrings.StockMovementForm.Types.Out)
            return MovementType.Out;
        if (displayText == AppStrings.StockMovementForm.Types.Adjustment)
            return MovementType.Adjustment;
            
        throw new ArgumentException($"Invalid movement type: {displayText}");
    }

    private async void AddButton_Click(object sender, EventArgs e)
    {
        if (!ValidateInput()) return;

        try
        {
            var movement = new StockMovement
            {
                ProductId = _product.Id,
                Type = (MovementType)_typeBox.SelectedItem,
                Quantity = int.Parse(_quantityBox.Text),
                Reference = _referenceBox.Text.Trim(),
                Notes = _notesBox.Text.Trim(),
                CreatedBy = Environment.UserName
            };

            await _stockService.AddAsync(movement);
            ClearInputs();
            LoadMovements();
            DialogResult = DialogResult.OK;
        }
        catch (Exception ex)
        {
            Logger.LogError($"Error adding movement: {ex.Message}");
            MessageBox.Show(
                AppStrings.IsEnglish ? $"Error adding stock movement: {ex.Message}" : $"Stok hareketi eklenirken hata oluştu: {ex.Message}",
                AppStrings.MainForm.Error,
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void ClearInputs()
    {
        _typeBox.SelectedIndex = 0;
        _quantityBox.Clear();
        _referenceBox.Clear();
        _notesBox.Clear();
    }

    private bool ValidateInput()
    {
        if (!int.TryParse(_quantityBox.Text, out int quantity) || quantity <= 0)
        {
            MessageBox.Show(AppStrings.StockMovementForm.QuantityPositive, 
                AppStrings.ProductForm.ValidationError,
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        return true;
    }

    private void ConfigureMovementGrid()
    {
        _movementGrid.AutoGenerateColumns = false;
        _movementGrid.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = "Type",
            HeaderText = AppStrings.StockMovementForm.Grid.MovementType,
            Name = "Type",
            MinimumWidth = 120
        });
        _movementGrid.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = "Quantity",
            HeaderText = AppStrings.StockMovementForm.Grid.Quantity,
            Name = "Quantity",
            MinimumWidth = 80,
            DefaultCellStyle = new DataGridViewCellStyle
            {
                Alignment = DataGridViewContentAlignment.MiddleCenter
            }
        });
        _movementGrid.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = "Reference",
            HeaderText = AppStrings.StockMovementForm.Grid.Reference,
            Name = "Reference",
            MinimumWidth = 100
        });
        _movementGrid.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = "Notes",
            HeaderText = AppStrings.StockMovementForm.Grid.Notes,
            Name = "Notes"
        });
        _movementGrid.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = "CreatedBy",
            HeaderText = AppStrings.StockMovementForm.Grid.CreatedBy,
            Name = "CreatedBy",
            MinimumWidth = 100
        });
        _movementGrid.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = "CreatedAt",
            HeaderText = AppStrings.StockMovementForm.Grid.CreatedAt,
            Name = "CreatedAt",
            MinimumWidth = 150,
            DefaultCellStyle = new DataGridViewCellStyle
            {
                Format = "g"
            }
        });
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _headerPanel?.Dispose();
            _titleLabel?.Dispose();
            _typeBox?.Dispose();
            _quantityBox?.Dispose();
            _referenceBox?.Dispose();
            _notesBox?.Dispose();
            _addButton?.Dispose();
            _closeButton?.Dispose();
            _movementGrid?.Dispose();
        }
        base.Dispose(disposing);
    }
} 