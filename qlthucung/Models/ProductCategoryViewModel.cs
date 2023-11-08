using System.Collections.Generic;

namespace qlthucung.Models
{
    public class ProductCategoryViewModel
    {
        public List<DanhMuc> Categories { get; set; }
        public List<SanPham> Products { get; set; }

        public int TotalProducts { get; set; } // Total number of products
        public int PageSize { get; set; } // Number of products to display per page
        public int PageNumber { get; set; } // Current page number
    }
}
