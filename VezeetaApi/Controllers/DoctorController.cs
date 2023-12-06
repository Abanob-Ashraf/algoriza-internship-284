using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VezeetaApi.Domain;
using VezeetaApi.Domain.Dtos;
using VezeetaApi.Domain.Models;

namespace VezeetaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        readonly IUnitOfWork UnitOfWork;
        readonly IMapper Mapper;

        public DoctorController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            UnitOfWork = unitOfWork;
            Mapper = mapper;
        }

        [HttpGet("GetAllDoctors")]
        public async Task<IActionResult> GetAllAsync()
        {
            var doctors =await UnitOfWork.GetRepository<Doctor>().GetAllAsync();
            if (doctors == null)
                return NotFound();
            var result = Mapper.Map<IEnumerable<DoctorDTO>>(doctors);
            return Ok(result);
        }

        [HttpGet("GetDoctor/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var oneDoctor = await UnitOfWork.GetRepository<Doctor>().GetByIdAsync(id);
            if (oneDoctor is null)
                return NotFound();
            var result = Mapper.Map<DoctorDTO>(oneDoctor);
            return Ok(result);
        }

        [HttpPost("AddNewDoctor")]
        public async Task<IActionResult> AddAsync(DoctorDTO doctorDTO)
        {
            var newDoctor = Mapper.Map<Doctor>(doctorDTO);
            await UnitOfWork.GetRepository<Doctor>().AddAsync(newDoctor);
            await UnitOfWork.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("UpdateDoctor")]
        public async Task<IActionResult> UpdateAsync(DoctorDTO doctorDTO)
        {
            var doctor = await UnitOfWork.GetRepository<Doctor>().FindAsync(c => c.Id == doctorDTO.Id);

            if (doctor is null)
                return NotFound();

            doctor.DocImage = doctorDTO.DocImage;
            doctor.DocFirstName = doctorDTO.DocFirstName;
            doctor.DocLastName = doctorDTO.DocLastName;
            doctor.DocBirthDate = doctorDTO.DocBirthDate;
            doctor.DocGender = doctorDTO.DocGender;
            doctor.DocPhone = doctorDTO.DocPhone;
            doctor.DocEmail = doctorDTO.DocEmail;
            doctor.DocPassword = doctorDTO.DocPassword;
            doctor.SpecializationId = doctorDTO.SpecializationId;

            UnitOfWork.GetRepository<Doctor>().Update(doctor);

            await UnitOfWork.SaveChangesAsync();

            var result = Mapper.Map<DoctorDTO>(doctor);
            return Ok(result);
        }

        [HttpDelete("DeleteDoctor")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var doctor = UnitOfWork.GetRepository<Doctor>().Delete(id);

            if (doctor is null)
                return NotFound();

            await UnitOfWork.SaveChangesAsync();

            var result = Mapper.Map<DoctorDTO>(doctor);
            return Ok(result);
        }
    }
}
