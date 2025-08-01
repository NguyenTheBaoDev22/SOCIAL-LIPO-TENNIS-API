using Applications.Features.Shops.Products.Commands;
using Applications.Features.Shops.Products.DTOs;
using Applications.Features.Shops.Products.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Results;

namespace API.Controllers
{
    [ApiController]
    [Route("api/product-categories")]
    public class ProductCategoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductCategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductCategoryCommand command)
            => Ok(await _mediator.Send(command));

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateProductCategoryCommand command)
        {
            command.Id = id;
            return Ok(await _mediator.Send(command));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
            => Ok(await _mediator.Send(new DeleteProductCategoryCommand { Id = id }));

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
            => Ok(await _mediator.Send(new GetProductCategoryByIdQuery { Id = id }));

        [HttpGet]
        //public async Task<IActionResult> GetPaged([FromQuery] GetProductCategoriesQuery query)
        //    => Ok(await _mediator.Send(query));
        public ActionResult<BaseResponse<List<ProductCategoryDto>>> GetProductCategories()
        {
            var categories = new List<ProductCategoryDto>
            {
                new ProductCategoryDto { Id = Guid.NewGuid(), Code = "001", Name = "Công nghệ" },
                new ProductCategoryDto { Id = Guid.NewGuid(), Code = "002", Name = "Rau củ" },
                new ProductCategoryDto { Id = Guid.NewGuid(), Code = "003", Name = "Giày dép" },
                new ProductCategoryDto { Id = Guid.NewGuid(), Code = "004", Name = "Quần áo" },
                new ProductCategoryDto { Id = Guid.NewGuid(), Code = "005", Name = "Thực uống" },
                new ProductCategoryDto { Id = Guid.NewGuid(), Code = "006", Name = "Điện tử tiêu dùng" },
                new ProductCategoryDto { Id = Guid.NewGuid(), Code = "007", Name = "Văn phòng phẩm" },
                new ProductCategoryDto { Id = Guid.NewGuid(), Code = "008", Name = "Dụng cụ thể thao" },
                new ProductCategoryDto { Id = Guid.NewGuid(), Code = "009", Name = "Nhà cửa" },
                new ProductCategoryDto { Id = Guid.NewGuid(), Code = "010", Name = "Mỹ phẩm" },
                new ProductCategoryDto { Id = Guid.NewGuid(), Code = "011", Name = "Sản phẩm cho trẻ em" },
                new ProductCategoryDto { Id = Guid.NewGuid(), Code = "012", Name = "Đồ gia dụng" },
                new ProductCategoryDto { Id = Guid.NewGuid(), Code = "013", Name = "Đồ dùng nhà bếp" },
                new ProductCategoryDto { Id = Guid.NewGuid(), Code = "014", Name = "Chăm sóc sức khỏe" },
                new ProductCategoryDto { Id = Guid.NewGuid(), Code = "015", Name = "Thực phẩm chế biến sẵn" },
                new ProductCategoryDto { Id = Guid.NewGuid(), Code = "016", Name = "Xe cộ, phụ tùng" },
                new ProductCategoryDto { Id = Guid.NewGuid(), Code = "017", Name = "Phụ kiện điện thoại" },
                new ProductCategoryDto { Id = Guid.NewGuid(), Code = "018", Name = "Thực phẩm chức năng" },
                new ProductCategoryDto { Id = Guid.NewGuid(), Code = "019", Name = "Đồ chơi trẻ em" },
                new ProductCategoryDto { Id = Guid.NewGuid(), Code = "020", Name = "Túi xách, ba lô" }
            };

            return Ok(BaseResponse<List<ProductCategoryDto>>.Success(categories));
        }

    }

}
