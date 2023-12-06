using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VezeetaApi.Domain;
using VezeetaApi.Domain.Dtos;
using VezeetaApi.Domain.Models;

namespace VezeetaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorScheduleController : ControllerBase
    {
        readonly IUnitOfWork UnitOfWork;
        readonly IMapper Mapper;

        public DoctorScheduleController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            UnitOfWork = unitOfWork;
            Mapper = mapper;
        }

        [HttpGet("GetAllDoctorSchedules")]
        public async Task<IActionResult> GetAllAsync()
        {
            var doctorSchedules = await UnitOfWork.GetRepository<DoctorSchedule>().GetAllAsync();
            if (doctorSchedules == null)
                return NotFound();
            var result = Mapper.Map<IEnumerable<DoctorScheduleDTO>>(doctorSchedules);
            return Ok(result);
        }

        [HttpGet("GetDoctorSchedule/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var doctorSchedule = await UnitOfWork.GetRepository<DoctorSchedule>().GetByIdAsync(id);
            if (doctorSchedule is null)
                return NotFound();
            var result = Mapper.Map<DoctorScheduleDTO>(doctorSchedule);
            return Ok(result);
        }

        [HttpPost("AddNewDoctorSchedule")]
        public async Task<IActionResult> AddAsync(DoctorScheduleDTO doctorScheduleDTO)
        {
            var newDoctorSchedule = Mapper.Map<DoctorSchedule>(doctorScheduleDTO);
            await UnitOfWork.GetRepository<DoctorSchedule>().AddAsync(newDoctorSchedule);
            await UnitOfWork.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("UpdateDoctorSchedule")]
        public async Task<IActionResult> UpdateAsync(DoctorScheduleDTO doctorScheduleDTO)
        {
            var doctorSchedule = await UnitOfWork.GetRepository<DoctorSchedule>().FindAsync(c => c.Id == doctorScheduleDTO.Id);

            if (doctorSchedule is null)
                return NotFound();

            doctorSchedule.Amount = doctorScheduleDTO.Amount;
            doctorSchedule.ScheduleDay = doctorScheduleDTO.ScheduleDay;
            doctorSchedule.ScheduleTime = doctorScheduleDTO.ScheduleTime;
            doctorSchedule.DoctorId = doctorScheduleDTO.DoctorId;

            UnitOfWork.GetRepository<DoctorSchedule>().Update(doctorSchedule);

            await UnitOfWork.SaveChangesAsync();

            var result = Mapper.Map<DoctorScheduleDTO>(doctorSchedule);
            return Ok(result);
        }

        [HttpPut("DeActiveAndActive/{id}")]
        public async Task<IActionResult> DeActiveAndActiveAsync(int id)
        {
            var doctorSchedule = await UnitOfWork.GetRepository<DoctorSchedule>().FindAsync(c => c.Id == id);

            if (doctorSchedule is null)
                return NotFound();

            UnitOfWork.GetRepository<DoctorSchedule>().DeActiveAndActive(doctorSchedule);
            await UnitOfWork.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("DeleteDoctorSchedule")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var doctor = UnitOfWork.GetRepository<DoctorSchedule>().Delete(id);

            if (doctor is null)
                return NotFound();

            await UnitOfWork.SaveChangesAsync();

            var result = Mapper.Map<DoctorScheduleDTO>(doctor);
            return Ok(result);
        }
    }
}
