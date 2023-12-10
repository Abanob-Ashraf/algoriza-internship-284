using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using VezeetaApi.Domain;
using VezeetaApi.Domain.Dtos;
using VezeetaApi.Domain.Dtos.CustomDtos;
using VezeetaApi.Domain.Models;
using VezeetaApi.Domain.Services;

namespace VezeetaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        readonly IUnitOfWork UnitOfWork;
        readonly IMapper Mapper;

        public PatientController(IUnitOfWork unitOfWork, IMapper mapper, IAuthService authService)
        {
            UnitOfWork = unitOfWork;
            Mapper = mapper;
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("getAllPatients")]
        public async Task<IActionResult> GetAllPatients(SearchDTO d)
        {
            var patient = await UnitOfWork.GetRepository<Patient>()
                .FindAllAsyncPaginated(c =>
                    (d.FullName == null ? true : c.PatientFullName.Contains(d.FullName)) &&
                    (d.Email == null ? true : c.PatientEmail == d.Email));

            var data = patient.Select(c => new
            {
                c.Id,
                c.PatientImage,
                c.PatientFullName,
                c.PatientEmail,
                c.PatientPhone,
                c.PatientGender,
                c.PatientBirthDate,
            }).ToList();

            return Ok(data);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetPatient/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var patient = await UnitOfWork.GetRepository<Patient>().GetByIdAsync(id);
            if (patient is null)
                return NotFound();
            var result = Mapper.Map<PatientDTO>(patient);
            return Ok(result);
        }

        [Authorize(Roles = "Admin, Patient")]
        [HttpPut("getAllDoctors")]
        public async Task<IActionResult> GetAllDoctors(SearchDTO d)
        {
            var doctors = await UnitOfWork.GetRepository<Doctor>()
                .FindAllAsyncPaginated(c =>
                    (d.FullName == null ? true : c.DoctorFullName.Contains(d.FullName)) &&
                    (d.Email == null ? true : c.DocEmail == d.Email) &&
                    (d.SpecializationName == null ? true : c.SpecializationIdNavigation.SpecializationName == d.SpecializationName));

            var data = doctors.Select(c => new
            {
                c.Id,
                c.DocImage,
                c.DoctorFullName,
                c.DocEmail,
                c.DocPhone,
                c.DocGender,
                c.SpecializationIdNavigation.SpecializationName,
                DoctorSchedules = c.DoctorSchedules
                    .GroupBy(v => v.ScheduleDay)
                    .Select(b => new
                    {
                        Day = b.Key,
                        Schedules = b.Select(v => new
                        {
                            Time = v.ScheduleTime,
                            v.Amount,
                            v.IsActive
                        }).ToList()
                    }).ToList()
            }).ToList();

            return Ok(data);
        }

        [Authorize(Roles = "Admin, Patient")]
        [HttpPost("Booking")]
        public async Task<IActionResult> BookingAppointment(AppointmentDTO appointmentDTO)
        {
            var userId = User.Claims.Where(c => c.Type == "DbUserId").FirstOrDefault()?.Value;
            var PatientId = int.Parse(userId);

            appointmentDTO.PatientId = PatientId;

            var patient = await UnitOfWork.GetRepository<Patient>().FindAllAsyncPaginated(c => c.Id == PatientId);

            var patientAppointmentCompleted = patient.Where(c => c.Appointments.Select(v => v.Status).Contains(Status.completed)).Count();

            var UsedDiscountCode = patient.SingleOrDefault().Appointments.Any();

            var discount = await UnitOfWork.GetRepository<Discount>().FindAllAsyncPaginated(c => c.Id == appointmentDTO.DiscountId);

            var numOfReq = discount.Select(c => c.NumOfCompletedRequests).SingleOrDefault();

            var newAppointment = Mapper.Map<Appointment>(appointmentDTO);

            if (!UsedDiscountCode && patientAppointmentCompleted >= numOfReq)
            {
                await UnitOfWork.GetRepository<Appointment>().AddAsync(newAppointment);
                await UnitOfWork.SaveChangesAsync();
                return Ok();
            }

            appointmentDTO.DiscountId = null;
            await UnitOfWork.GetRepository<Appointment>().AddAsync(newAppointment);
            await UnitOfWork.SaveChangesAsync();
            return Ok();
           
        }

        [Authorize(Roles = "Admin, Patient")]
        [HttpGet("PatientAppointments")]
        public async Task<IActionResult> PatientAppointments()
        {
            var userId = User.Claims.Where(c => c.Type == "DbUserId").FirstOrDefault()?.Value;
            var PatientId = int.Parse(userId);

            var appointments = await UnitOfWork.GetRepository<Appointment>()
                .FindAllAsyncPaginated(c => c.PatientId == PatientId);

            var data = appointments.Select(c =>
            {
                var price = c.DoctorIdNavigation.DoctorSchedules
                    .Where(v =>
                    (c.ResevationDate.DayOfWeek.Equals((DayOfWeek)v.ScheduleDay)) &&
                    (c.ResevationDate.TimeOfDay == v.ScheduleTime))
                    .Select(v => v.Amount).SingleOrDefault();


                var finalPrice = c.DiscountIdNavigation.DiscountType.Equals(DiscountType.Value) ?
                                     price - c.DiscountIdNavigation.DiscountValue :
                                    price - (price * c.DiscountIdNavigation.DiscountValue / 100);

                return new
                {
                    c.DoctorIdNavigation.DocImage,
                    c.DoctorIdNavigation.DoctorFullName,
                    c.DoctorIdNavigation.SpecializationIdNavigation.SpecializationName,
                    c.ResevationDate.Year,
                    c.ResevationDate.Month,
                    c.ResevationDate.Day,
                    c.ResevationDate.DayOfWeek,
                    c.ResevationDate.TimeOfDay,
                    price,
                    c.DiscountIdNavigation.DiscountCode,
                    finalPrice,
                    c.Status,
                };
            }).ToList();

            return Ok(data);
        }

        [Authorize(Roles = "Admin, Patient")]
        [HttpPut("Cancel")]
        public async Task<IActionResult> CancelingAppointment(int AppointmentId)
        {
            var appointment = UnitOfWork.GetRepository<Appointment>();

            var UpdatedAppointment = await appointment.FindAsync(c => c.Id == AppointmentId);

            if (UpdatedAppointment is null)
                return NotFound();

            UpdatedAppointment.Status = Status.cancelled;

            appointment.Update(UpdatedAppointment);
            appointment.DeActiveAndActive(UpdatedAppointment);

            await UnitOfWork.SaveChangesAsync();

            return Ok();
        }
    }
}
