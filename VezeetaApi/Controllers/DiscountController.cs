using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VezeetaApi.Domain;
using VezeetaApi.Domain.Dtos;
using VezeetaApi.Domain.Models;

namespace VezeetaApi.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountController : ControllerBase
    {
        readonly IUnitOfWork UnitOfWork;
        readonly IMapper Mapper;

        public DiscountController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            UnitOfWork = unitOfWork;
            Mapper = mapper;
        }

        [HttpGet("GetAllDiscount")]
        public async Task<IActionResult> GetAllAsync()
        {
            var discounts = await UnitOfWork.GetRepository<Discount>().GetAllAsync();
            if (discounts is null)
                return NotFound();
            var result = Mapper.Map<IEnumerable<DiscountDTO>>(discounts);
            return Ok(result);
        }

        [HttpGet("Discount/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var discount = await UnitOfWork.GetRepository<Discount>().GetByIdAsync(id);
            if (discount is null)
                return NotFound();
            var result = Mapper.Map<DiscountDTO>(discount);
            return Ok(result);
        }

        [HttpPost("AddNewDiscount")]
        public async Task<IActionResult> AddAsync(DiscountDTO discountDTO)
        {
            var newDiscount = Mapper.Map<Discount>(discountDTO);
            await UnitOfWork.GetRepository<Discount>().AddAsync(newDiscount);
            await UnitOfWork.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("UpdateDiscount")]
        public async Task<IActionResult> UpdateAsync(DiscountDTO discountDTO)
        {
            var discount = UnitOfWork.GetRepository<Discount>();

            var updatedDiscount = await discount.FindAllAsyncPaginated(c => c.Id == discountDTO.Id);

            if (updatedDiscount is null)
                return NotFound(discountDTO);

            var usedDiscount = updatedDiscount.SingleOrDefault().Appointments.Any();

            if (!usedDiscount)
            {
                var data = updatedDiscount.SingleOrDefault();
                data.DiscountCode = discountDTO.DiscountCode;
                data.NumOfCompletedRequests = discountDTO.NumOfCompletedRequests;
                data.DiscountType = discountDTO.DiscountType;
                data.DiscountValue = discountDTO.DiscountValue;

                discount.Update(data);

                await UnitOfWork.SaveChangesAsync();
                return Ok();
            }
            return BadRequest();
        }

        [HttpPut("DeActiveAndActive/{id}")]
        public async Task<IActionResult> DeActiveAndActiveAsync(int id)
        {
            var discount = UnitOfWork.GetRepository<Discount>();

            var deActiveDiscount = await discount.FindAsync(c => c.Id == id);

            if (deActiveDiscount is null)
                return NotFound();

            discount.DeActiveAndActive(deActiveDiscount);
            await UnitOfWork.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("DeleteDiscount")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var discount = UnitOfWork.GetRepository<Discount>().Delete(id);

            if (discount is null)
                return NotFound();

            await UnitOfWork.SaveChangesAsync();

            return Ok();
        }
    }
}
