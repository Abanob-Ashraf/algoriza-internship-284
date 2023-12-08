using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VezeetaApi.Domain.Dtos;
using VezeetaApi.Domain.Models;
using VezeetaApi.Domain;

namespace VezeetaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        readonly IUnitOfWork UnitOfWork;
        readonly IMapper Mapper;

        public AppointmentController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            UnitOfWork = unitOfWork;
            Mapper = mapper;
        }

        [HttpGet("GetAllAppointments")]
        public async Task<IActionResult> GetAllAsync()
        {
            var appointments = await UnitOfWork.GetRepository<Appointment>().GetAllAsync();
            if (appointments is null)
                return NotFound();
            var result = Mapper.Map<IEnumerable<AppointmentDTO>>(appointments);
            return Ok(result);
        }

        [HttpGet("Appointment/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            //var appointment = await UnitOfWork.GetRepository<Appointment>().GetByIdAsync(id);
            var appointment = await UnitOfWork.AppointmentRepo.FindAsync(c => c.Id == id);
            if (appointment is null)
                return NotFound();
            //var result = Mapper.Map<AppointmentDTO>(appointment);
            return Ok(appointment);
        }

        [HttpPost("AddNewAppointment")]
        public async Task<IActionResult> AddAsync(AppointmentDTO appointmentDTO)
        {
            var newAppointment = Mapper.Map<Appointment>(appointmentDTO);
            await UnitOfWork.GetRepository<Appointment>().AddAsync(newAppointment);
            await UnitOfWork.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("UpdateAppointment")]
        public async Task<IActionResult> UpdateAsync(AppointmentDTO appointmentDTO)
        {
            var appointment = await UnitOfWork.GetRepository<Appointment>().FindAsync(c => c.Id == appointmentDTO.Id);

            if (appointment is null)
                return NotFound(appointmentDTO);

            appointment.ResevationDate = appointmentDTO.ResevationDate;
            appointment.Status = appointmentDTO.Status;
            appointment.PatientId = appointmentDTO.PatientId;
            appointment.DoctorId = appointmentDTO.DoctorId;
            appointment.DiscountId = appointmentDTO.DiscountId;

            UnitOfWork.GetRepository<Appointment>().Update(appointment);

            await UnitOfWork.SaveChangesAsync();

            var result = Mapper.Map<AppointmentDTO>(appointment);
            return Ok(result);
        }

        [HttpPut("DeActiveAndActive/{id}")]
        public async Task<IActionResult> DeActiveAndActiveAsync(int id)
        {
            var appointment = await UnitOfWork.GetRepository<Appointment>().FindAsync(c => c.Id == id);

            if (appointment is null)
                return NotFound();

            UnitOfWork.GetRepository<Appointment>().DeActiveAndActive(appointment);
            await UnitOfWork.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("DeleteAppointment")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var appointment = UnitOfWork.GetRepository<Appointment>().Delete(id);

            if (appointment is null)
                return NotFound();

            await UnitOfWork.SaveChangesAsync();

            var result = Mapper.Map<AppointmentDTO>(appointment);
            return Ok(result);
        }
    }
}
