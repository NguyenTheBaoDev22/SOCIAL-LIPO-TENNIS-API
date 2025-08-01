using Applications.Features.CQRS.Bases;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shared.Results;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        // API thêm sản phẩm mới (mock trực tiếp trong controller)
        [HttpPost("add")]
        public ActionResult<BaseResponse<ProductResponse>> AddProduct([FromBody] CreateProductCommand command)
        {
            // Mock tạo sản phẩm mới
            var newProduct = new ProductResponse
            {
                Id = Guid.NewGuid(),
                MerchantBranchId = command.MerchantBranchId,
                ProductName = command.ProductName,
                ProductCategoryId = command.ProductCategoryId,
                ImportPrice = command.ImportPrice,
                SellingPrice = command.SellingPrice,
                StockQuantity = command.StockQuantity,
                ProductCode = command.ProductCode ?? "AUTO_GENERATED_CODE", // Nếu không có mã sản phẩm, tự tạo
                ProductImagesUrl = command.ProductImagesUrl
            };

            // Trả về kết quả
            return CreatedAtAction(nameof(AddProduct), new { id = newProduct.Id }, BaseResponse<ProductResponse>.Success(newProduct));
        }
        // Lớp command cho đầu vào (API nhận vào body)


        // API GET danh sách sản phẩm có phân trang và lọc theo MerchantBranchId
        [HttpGet("get-all")]
        public ActionResult<BaseResponse<PaginatedResult<ProductResponse>>> GetProducts(
            [FromQuery] Guid merchantBranchId,
            [FromQuery] int pageIndex = 1,  // Mặc định pageIndex là 1
            [FromQuery] int pageSize = 20   // Mặc định pageSize là 20
        )
        {
            // Mock danh sách sản phẩm
            var allProducts = new List<ProductResponse>
            {
                new ProductResponse
                {
                    Id = Guid.NewGuid(),
                    MerchantBranchId = Guid.NewGuid(),
                    ProductName = "Laptop Dell XPS 13",
                    ProductCategoryId = Guid.NewGuid(),
                    ImportPrice = 25000,
                    SellingPrice = 28000,
                    StockQuantity = 50,
                    ProductCode = "LAPTOP123",
                    ProductImagesUrl = "https://example.com/product-image1.jpg"
                },
                new ProductResponse
                {
                    Id = Guid.NewGuid(),
                    MerchantBranchId = Guid.NewGuid(),
                    ProductName = "Smartphone iPhone 13",
                    ProductCategoryId = Guid.NewGuid(),
                    ImportPrice = 12000,
                    SellingPrice = 13000,
                    StockQuantity = 100,
                    ProductCode = "IPHONE13",
                    ProductImagesUrl = "https://example.com/product-image2.jpg"
                },
                new ProductResponse
                {
                    Id = Guid.NewGuid(),
                    MerchantBranchId = Guid.NewGuid(),
                    ProductName = "Tablet Samsung Galaxy Tab",
                    ProductCategoryId = Guid.NewGuid(),
                    ImportPrice = 8000,
                    SellingPrice = 8500,
                    StockQuantity = 75,
                    ProductCode = "TBLTSAMSUNG",
                    ProductImagesUrl = "https://example.com/product-image3.jpg"
                },
                new ProductResponse
                {
                    Id = Guid.NewGuid(),
                    MerchantBranchId = Guid.NewGuid(),
                    ProductName = "Headphones Sony WH-1000XM4",
                    ProductCategoryId = Guid.NewGuid(),
                    ImportPrice = 5000,
                    SellingPrice = 5500,
                    StockQuantity = 120,
                    ProductCode = "HEADPHONES1",
                    ProductImagesUrl = "https://example.com/product-image4.jpg"
                },
                new ProductResponse
                {
                    Id = Guid.NewGuid(),
                    MerchantBranchId = Guid.NewGuid(),
                    ProductName = "Smartwatch Garmin Venu 2",
                    ProductCategoryId = Guid.NewGuid(),
                    ImportPrice = 7000,
                    SellingPrice = 7500,
                    StockQuantity = 90,
                    ProductCode = "SMARTWATCHGARMIN",
                    ProductImagesUrl = "https://example.com/product-image5.jpg"
                }
                // Thêm nhiều sản phẩm vào đây...
            };

            //// Lọc theo MerchantBranchId
            //var filteredProducts = allProducts.Where(p => p.MerchantBranchId == merchantBranchId).ToList();

            // Áp dụng phân trang thủ công
            var skipCount = (pageIndex - 1) * pageSize;
            var paginatedProducts = allProducts.Skip(skipCount).Take(pageSize).ToList();
            var totalCount = allProducts.Count;

            // Tạo PaginatedResult và trả về kết quả
            var paginatedResult = PaginatedResult<ProductResponse>.Create(paginatedProducts, pageIndex, pageSize, totalCount);

            return Ok(BaseResponse<PaginatedResult<ProductResponse>>.Success(paginatedResult));
        }

        // API tìm kiếm sản phẩm ngẫu nhiên
        // API tìm kiếm sản phẩm theo tên hoặc mã sản phẩm (productName hoặc productCode)
        [HttpGet("search")]
        public ActionResult<BaseResponse<List<ProductResponse>>> SearchProducts([FromQuery] string searchTerm, [FromQuery] Guid merchantBranchId)
        {
            // Mock danh sách sản phẩm
            var allProducts = new List<ProductResponse>
            {
                new ProductResponse
                {
                    Id = Guid.NewGuid(),
                    MerchantBranchId = Guid.NewGuid(),
                    ProductName = "Laptop Dell XPS 13",
                    ProductCategoryId = Guid.NewGuid(),
                    ImportPrice = 25000,
                    SellingPrice = 28000,
                    StockQuantity = 50,
                    ProductCode = "LAPTOP123",
                    ProductImagesUrl = "https://example.com/product-image1.jpg"
                },
                new ProductResponse
                {
                    Id = Guid.NewGuid(),
                    MerchantBranchId = Guid.NewGuid(),
                    ProductName = "Smartphone iPhone 13",
                    ProductCategoryId = Guid.NewGuid(),
                    ImportPrice = 12000,
                    SellingPrice = 13000,
                    StockQuantity = 100,
                    ProductCode = "IPHONE13",
                    ProductImagesUrl = "https://example.com/product-image2.jpg"
                },
                new ProductResponse
                {
                    Id = Guid.NewGuid(),
                    MerchantBranchId = Guid.NewGuid(),
                    ProductName = "Tablet Samsung Galaxy Tab",
                    ProductCategoryId = Guid.NewGuid(),
                    ImportPrice = 8000,
                    SellingPrice = 8500,
                    StockQuantity = 75,
                    ProductCode = "TBLTSAMSUNG",
                    ProductImagesUrl = "https://example.com/product-image3.jpg"
                },
                new ProductResponse
                {
                    Id = Guid.NewGuid(),
                    MerchantBranchId = Guid.NewGuid(),
                    ProductName = "Headphones Sony WH-1000XM4",
                    ProductCategoryId = Guid.NewGuid(),
                    ImportPrice = 5000,
                    SellingPrice = 5500,
                    StockQuantity = 120,
                    ProductCode = "HEADPHONES1",
                    ProductImagesUrl = "https://example.com/product-image4.jpg"
                },
                new ProductResponse
                {
                    Id = Guid.NewGuid(),
                    MerchantBranchId = Guid.NewGuid(),
                    ProductName = "Smartwatch Garmin Venu 2",
                    ProductCategoryId = Guid.NewGuid(),
                    ImportPrice = 7000,
                    SellingPrice = 7500,
                    StockQuantity = 90,
                    ProductCode = "SMARTWATCHGARMIN",
                    ProductImagesUrl = "https://example.com/product-image5.jpg"
                }
                // Thêm nhiều sản phẩm vào đây...
            };

            // Tạo ngẫu nhiên số lượng sản phẩm muốn trả về: có thể 1, 2 hoặc 5 sản phẩm.
            var random = new Random();
            int numberOfProducts = random.Next(1, 6);  // Lấy số ngẫu nhiên từ 1 đến 5

            // Lấy ngẫu nhiên số lượng sản phẩm từ danh sách
            var randomProducts = allProducts.OrderBy(x => random.Next()).Take(numberOfProducts).ToList();

            return Ok(BaseResponse<List<ProductResponse>>.Success(randomProducts));
        }

        // API kiểm kho - Cập nhật số lượng tồn kho (mock kết quả thành công)
        [HttpPost("update-stock")]
        public ActionResult<BaseResponse<string>> UpdateStock([FromBody] UpdateStockCommand command)
        {
            // Mock việc cập nhật tồn kho thành công
            var successMessage = $"Stock for product {command.ProductId} at branch {command.MerchantBranchId} updated to {command.StockQuantity}.";

            // Trả về thông báo thành công mà không cần data
            return Ok(BaseResponse<string>.Success(successMessage, "Stock updated successfully"));
        }












        // Lớp command nhận đầu vào cho API kiểm kho
        public class UpdateStockCommand
        {
            public Guid MerchantBranchId { get; set; }
            public Guid ProductId { get; set; }
            public int StockQuantity { get; set; }
        }

        public class CreateProductCommand
        {
            public Guid MerchantBranchId { get; set; }
            public string ProductName { get; set; }
            public Guid ProductCategoryId { get; set; }
            public decimal ImportPrice { get; set; }
            public decimal SellingPrice { get; set; }
            public int StockQuantity { get; set; }
            public string? ProductCode { get; set; } // Không bắt buộc
            public string ProductImagesUrl { get; set; }
        }
        // Lớp trả về thông tin sản phẩm
        public class ProductResponse
        {
            public Guid Id { get; set; }
            public Guid MerchantBranchId { get; set; }
            public string ProductName { get; set; }
            public Guid ProductCategoryId { get; set; }
            public decimal ImportPrice { get; set; }
            public decimal SellingPrice { get; set; }
            public int StockQuantity { get; set; }
            public string? ProductCode { get; set; }
            public string ProductImagesUrl { get; set; }
        }
    }
}
