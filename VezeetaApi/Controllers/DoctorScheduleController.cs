using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Linq;
using System.Numerics;
using VezeetaApi.Domain;
using VezeetaApi.Domain.Dtos;
using VezeetaApi.Domain.Models;

namespace VezeetaApi.Controllers
{
    [Authorize(Roles = "Admin, Doctor")]
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
            var doctorSchedule = UnitOfWork.GetRepository<DoctorSchedule>();
            
            var updatedDoctorScheduleList = (await doctorSchedule.FindAllAsyncPaginated(c => c.Id == doctorScheduleDTO.Id)).ToList();

            if (updatedDoctorScheduleList is null)
                return NotFound();

            var data = updatedDoctorScheduleList
                .Where(c => c.DoctorIdNavigation.Appointments.Select(v => v.Status).Contains(Status.pending)).Any();


            if (!data)
            {
                foreach (var updatedDoctorSchedule in updatedDoctorScheduleList)
                {
                    updatedDoctorSchedule.Amount = doctorScheduleDTO.Amount;
                    updatedDoctorSchedule.ScheduleDay = doctorScheduleDTO.ScheduleDay;
                    updatedDoctorSchedule.ScheduleTime = doctorScheduleDTO.ScheduleTime;
                    updatedDoctorSchedule.DoctorId = doctorScheduleDTO.DoctorId;

                    doctorSchedule.Update(updatedDoctorSchedule);
                }

                await UnitOfWork.SaveChangesAsync();
                return Ok();
            }
            return BadRequest();

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
            var doctorSchedule = UnitOfWork.GetRepository<DoctorSchedule>();
            var deletedDoctorSchedule = await doctorSchedule.FindAllAsyncPaginated(c => c.Id == id);

            if (deletedDoctorSchedule is null)
                return NotFound();

            var data = deletedDoctorSchedule
                .Where(c => c.DoctorIdNavigation.Appointments.Select(v => v.Status).Contains(Status.pending)).Any();

            if (!data)
            {
                doctorSchedule.Delete(id);
                await UnitOfWork.SaveChangesAsync();
                return Ok();
            }
            return BadRequest();
        }
    }
}
