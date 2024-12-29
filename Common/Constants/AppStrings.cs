namespace DesktopApp.Common.Constants;

public static class AppStrings
{
    public static bool IsEnglish { get; set; } = false;

    public static class MainForm
    {
        public static string Title => IsEnglish ? "Inventory Management System" : "Stok Yönetim Sistemi";
        public static string AddProduct => IsEnglish ? "Add Product" : "Ürün Ekle";
        public static string AddStockMovement => IsEnglish ? "Add Stock Movement" : "Stok Hareketi Ekle";
        public static string DeleteProduct => IsEnglish ? "Delete Product" : "Ürünü Sil";
        public static string ConfirmDelete => IsEnglish ? "Are you sure you want to delete the product '{0}'?" : "'{0}' ürününü silmek istediğinizden emin misiniz?";
        public static string SelectProduct => IsEnglish ? "Please select a product first." : "Lütfen önce bir ürün seçin.";
        public static string Error => IsEnglish ? "Error" : "Hata";
        public static string Warning => IsEnglish ? "Warning" : "Uyarı";
        public static string ConfirmDeleteTitle => IsEnglish ? "Confirm Delete" : "Silme Onayı";

        public static class Grid
        {
            public static string ProductName => IsEnglish ? "Product Name" : "Ürün Adı";
            public static string Description => IsEnglish ? "Description" : "Açıklama";
            public static string Price => IsEnglish ? "Price (TL)" : "Fiyat (TL)";
            public static string MinimumStock => IsEnglish ? "Minimum Stock" : "Minimum Stok";
            public static string CurrentStock => IsEnglish ? "Current Stock" : "Mevcut Stok";
        }
    }

    public static class ProductForm
    {
        public static string AddTitle => IsEnglish ? "Add Product" : "Ürün Ekle";
        public static string EditTitle => IsEnglish ? "Edit Product" : "Ürün Düzenle";
        public static string ProductName => IsEnglish ? "Product Name:" : "Ürün Adı:";
        public static string Description => IsEnglish ? "Description:" : "Açıklama:";
        public static string Price => IsEnglish ? "Price (TL):" : "Fiyat (TL):";
        public static string CurrentStock => IsEnglish ? "Current Stock:" : "Mevcut Stok:";
        public static string MinStockLevel => IsEnglish ? "Min Stock Level:" : "Min. Stok Seviyesi:";
        public static string Save => IsEnglish ? "Save" : "Kaydet";
        public static string Cancel => IsEnglish ? "Cancel" : "İptal";
        public static string ValidationError => IsEnglish ? "Validation Error" : "Doğrulama Hatası";
        public static string NameRequired => IsEnglish ? "Name is required" : "İsim gereklidir";
        public static string PricePositive => IsEnglish ? "Price must be a positive number" : "Fiyat pozitif bir sayı olmalıdır";
        public static string StockPositive => IsEnglish ? "Stock quantity must be a positive number" : "Stok miktarı pozitif bir sayı olmalıdır";
        public static string MinStockPositive => IsEnglish ? "Minimum stock level must be a positive number" : "Minimum stok seviyesi pozitif bir sayı olmalıdır";
    }

    public static class StockMovementForm
    {
        public static string Title => IsEnglish ? "Stock Movements - {0}" : "Stok Hareketleri - {0}";
        public static string MovementType => IsEnglish ? "Movement Type:" : "Hareket Tipi:";
        public static string Quantity => IsEnglish ? "Quantity:" : "Miktar:";
        public static string Reference => IsEnglish ? "Reference:" : "Referans:";
        public static string Notes => IsEnglish ? "Notes:" : "Notlar:";
        public static string AddMovement => IsEnglish ? "Add Movement" : "Hareket Ekle";
        public static string Close => IsEnglish ? "Close" : "Kapat";
        public static string QuantityPositive => IsEnglish ? "Quantity must be a positive number" : "Miktar pozitif bir sayı olmalıdır";

        public static class Grid
        {
            public static string MovementType => IsEnglish ? "Movement Type" : "Hareket Tipi";
            public static string Quantity => IsEnglish ? "Quantity" : "Miktar";
            public static string Reference => IsEnglish ? "Reference" : "Referans";
            public static string Notes => IsEnglish ? "Notes" : "Notlar";
            public static string CreatedBy => IsEnglish ? "Created By" : "Oluşturan";
            public static string CreatedAt => IsEnglish ? "Created At" : "Oluşturma Tarihi";
        }

        public static class Types
        {
            public static string In => IsEnglish ? "Stock In" : "Stok Girişi";
            public static string Out => IsEnglish ? "Stock Out" : "Stok Çıkışı";
            public static string Adjustment => IsEnglish ? "Adjustment" : "Düzeltme";
        }
    }
} 