using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VezeetaApi.Domain;
using VezeetaApi.Domain.Dtos;
using VezeetaApi.Domain.Models;

namespace VezeetaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorSpecializionController : ControllerBase
    {
        readonly IUnitOfWork UnitOfWork;
        readonly IMapper Mapper;

        public DoctorSpecializionController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            UnitOfWork = unitOfWork;
            Mapper = mapper;
        }

        [HttpGet("GetAllSpecializions")]
        public async Task<IActionResult> GetAllAsync()
        {
            var doctorSpecializions = await UnitOfWork.GetRepository<DoctorSpecializion>().GetAllAsync();
            if (doctorSpecializions is null)
                return NotFound();
            var result = Mapper.Map<IEnumerable<DoctorSpecializionDTO>>(doctorSpecializions);
            return Ok(result);
        }

        [HttpGet("Specializion/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var doctorSpecializion = await UnitOfWork.GetRepository<DoctorSpecializion>().GetByIdAsync(id);
            if (doctorSpecializion is null) 
                return NotFound();
            var result = Mapper.Map<DoctorSpecializionDTO>(doctorSpecializion);
            return Ok(result);
        }

        [HttpPost("AddNewSpecializion")]
        public async Task<IActionResult> AddAsync(DoctorSpecializionDTO doctorSpecializionDTO)
        {
            var newDoctorSpecializion = Mapper.Map<DoctorSpecializion>(doctorSpecializionDTO);
            await UnitOfWork.GetRepository<DoctorSpecializion>().AddAsync(newDoctorSpecializion);
            await UnitOfWork.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("UpdateSpecializion")]
        public async Task<IActionResult> UpdateAsync(DoctorSpecializionDTO doctorSpecializionDTO)
        {
            var doctorSpecializion = await UnitOfWork.GetRepository<DoctorSpecializion>().FindAsync(c => c.Id == doctorSpecializionDTO.Id);

            if(doctorSpecializion is null)
                return NotFound(doctorSpecializionDTO);

            doctorSpecializion.SpecializationName = doctorSpecializionDTO.SpecializationName;

            UnitOfWork.GetRepository<DoctorSpecializion>().Update(doctorSpecializion);
            
            await UnitOfWork.SaveChangesAsync();

            var result = Mapper.Map<DoctorSpecializionDTO>(doctorSpecializion);
            return Ok(result);
        }

        [HttpPut("DeActiveAndActive/{id}")]
        public async Task<IActionResult> DeActiveAndActiveAsync(int id)
        {
            var doctorSpecializion = await UnitOfWork.GetRepository<DoctorSpecializion>().FindAsync(c => c.Id == id);

            if (doctorSpecializion is null)
                return NotFound();

            UnitOfWork.GetRepository<DoctorSpecializion>().DeActiveAndActive(doctorSpecializion);
            await UnitOfWork.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("DeleteSpecializion")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var doctorSpecializion = UnitOfWork.GetRepository<DoctorSpecializion>().Delete(id);

            if (doctorSpecializion is null)
                return NotFound();

            await UnitOfWork.SaveChangesAsync();

            var result = Mapper.Map<DoctorSpecializionDTO>(doctorSpecializion);
            return Ok(result);
        }

    }
}
