using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using VezeetaApi.Domain;
using VezeetaApi.Domain.Dtos;
using VezeetaApi.Domain.Dtos.CustomDtos;
using VezeetaApi.Domain.Models;
using VezeetaApi.Domain.Services;
using VezeetaApi.Infrastructure.Repositories;

namespace VezeetaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        readonly IUnitOfWork UnitOfWork;
        readonly IMapper Mapper;
        private readonly UserManager<AppUser> UserManager;
        private readonly ISendingEmailService SendingEmailService;

        public DoctorController(IUnitOfWork unitOfWork, IMapper mapper, UserManager<AppUser> userManager, ISendingEmailService sendingEmailService)
        {
            UnitOfWork = unitOfWork;
            Mapper = mapper;
            UserManager = userManager;
            SendingEmailService = sendingEmailService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("getAllDoctors")]
        public async Task<IActionResult> GetAllDoctors(SearchDTO d)
        {
            var doctor = await UnitOfWork.GetRepository<Doctor>()
                .FindAllAsyncPaginated(c =>
                    (d.FullName == null ? true : c.DoctorFullName.Contains(d.FullName)) &&
                    (d.SpecializationName == null ? true : c.SpecializationIdNavigation.SpecializationName == d.SpecializationName) &&
                    (d.Email == null ? true : c.DocEmail == d.Email));

            var data = doctor.Select(c => new
            {
                c.Id,
                c.DocImage,
                c.DoctorFullName,
                c.DocEmail,
                c.DocPhone,
                c.DocGender,
                c.SpecializationIdNavigation.SpecializationName,
            }).ToList();

            return Ok(data);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetDoctor/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var doctor = await UnitOfWork.GetRepository<Doctor>().FindAllAsyncPaginated(c => c.Id == id);

            var data = doctor.Select(c => new
            {
                c.Id,
                c.DocImage,
                c.DoctorFullName,
                c.DocEmail,
                c.DocPhone,
                c.DocGender,
                c.SpecializationIdNavigation.SpecializationName,
            }).Single();

            return Ok(data);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("AddNewDoctor")]
        public async Task<IActionResult> AddAsync(DoctorDTO doctorDTO)
        {
            Random random = new Random();
            long newRandomNum = Convert.ToInt64(random.Next(1, 50) + doctorDTO.DocPhone) / 350000 * 2;
            var genericPassword = $"{doctorDTO.DocFirstName}#{newRandomNum}";
            string FinalFormOfPassword = char.ToUpper(genericPassword[0]) + genericPassword.Substring(1);
            doctorDTO.DocPassword = FinalFormOfPassword;

            var aspUser = new AppUser()
            {
                FirstName = doctorDTO.DocFirstName,
                LastName = doctorDTO.DocLastName,
                UserName = doctorDTO.DocEmail,
                Email = doctorDTO.DocEmail,
                EmailConfirmed = true,
                PhoneNumber = doctorDTO.DocPhone,
                PasswordHash = FinalFormOfPassword
            };

            var result = await UserManager.CreateAsync(aspUser, FinalFormOfPassword);

            if (!result.Succeeded)
            {
                var errors = string.Empty;
                foreach (var error in result.Errors)
                {
                    errors += $"{error.Description},";
                }
                return BadRequest(errors);
            }

            doctorDTO.DocPassword = aspUser.PasswordHash;
            var newDoctor = Mapper.Map<Doctor>(doctorDTO);
            await UnitOfWork.GetRepository<Doctor>().AddAsync(newDoctor);
            await UnitOfWork.SaveChangesAsync();
            await UserManager.AddToRoleAsync(aspUser, "Doctor");

            var message = new MailMessageDTO(

                new string[] { aspUser.Email },
                "Your email And Password",
                $"<br>" +
                $"<label style='font - size: 16px; border - radius: 0.25rem; color: blue;' >Your Email: </label> " +
                $"<label style='font - size: 16px; border - radius: 0.25rem;' >{doctorDTO.DocEmail}</label> <br> " +
                $"<label style='font - size: 16px; border - radius: 0.25rem; color: blue;' >Your PassWord:</label> " +
                $"<label style='font - size: 16px; border - radius: 0.25rem;' >{FinalFormOfPassword}</label> <br>" +
                $"<label style='font-size: 16px; border-radius: 0.25rem; color: red;'>Please Don't Share it with anyOne</label> ",
                null);

            await SendingEmailService.SendEmailAsync(message);
            var finalResult = Mapper.Map<DoctorDTO>(newDoctor);
            return Ok(finalResult);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateDoctor")]
        public async Task<IActionResult> UpdateAsync(DoctorDTO doctorDTO)
        {
            var doctor = UnitOfWork.GetRepository<Doctor>();

            var updatedDoctor = await doctor.FindAsync(c => c.Id == doctorDTO.Id);

            if (updatedDoctor is null)
                return NotFound();

            Random random = new Random();
            long newRandomNum = Convert.ToInt64(random.Next(1, 50) + doctorDTO.DocPhone) / 350000 * 2;
            var genericPassword = $"{doctorDTO.DocFirstName}#{newRandomNum}";
            string FinalFormOfPassword = char.ToUpper(genericPassword[0]) + genericPassword.Substring(1);
            doctorDTO.DocPassword = FinalFormOfPassword;


            var user = await UserManager.FindByEmailAsync(doctorDTO.DocEmail);

            if (user is null)
                return NotFound();

            user.FirstName = doctorDTO.DocFirstName;
            user.LastName = doctorDTO.DocLastName;
            user.UserName = doctorDTO.DocEmail;
            user.Email = doctorDTO.DocEmail;
            user.PhoneNumber = doctorDTO.DocPhone;

            var newPasswordHash = UserManager.PasswordHasher.HashPassword(user, FinalFormOfPassword);
            user.PasswordHash = newPasswordHash;
            doctorDTO.DocPassword = user.PasswordHash;

            var result = await UserManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                var errors = string.Empty;
                foreach (var error in result.Errors)
                {
                    errors += $"{error.Description},";
                }
                return BadRequest(errors);
            }


            updatedDoctor.DocImage = doctorDTO.DocImage;
            updatedDoctor.DocFirstName = doctorDTO.DocFirstName;
            updatedDoctor.DocLastName = doctorDTO.DocLastName;
            updatedDoctor.DocBirthDate = doctorDTO.DocBirthDate;
            updatedDoctor.DocGender = doctorDTO.DocGender;
            updatedDoctor.DocPhone = doctorDTO.DocPhone;
            updatedDoctor.DocEmail = doctorDTO.DocEmail;
            updatedDoctor.DocPassword = doctorDTO.DocPassword;
            updatedDoctor.SpecializationId = doctorDTO.SpecializationId;
            doctor.Update(updatedDoctor);
            await UnitOfWork.SaveChangesAsync();

            var message = new MailMessageDTO(

                new string[] { user.Email },
                "Your new email And Password",
                $"<br>" +
                $"<label style='font - size: 16px; border - radius: 0.25rem; color: blue;' >Your Email: </label> " +
                $"<label style='font - size: 16px; border - radius: 0.25rem;' >{doctorDTO.DocEmail}</label> <br> " +
                $"<label style='font - size: 16px; border - radius: 0.25rem; color: blue;' >Your PassWord:</label> " +
                $"<label style='font - size: 16px; border - radius: 0.25rem;' >{FinalFormOfPassword}</label> <br>" +
                $"<label style='font-size: 16px; border-radius: 0.25rem; color: red;'>Please Don't Share it with anyOne</label> ",
                null);

            await SendingEmailService.SendEmailAsync(message);

            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("DeActiveAndActive/{id}")]
        public async Task<IActionResult> DeActiveAndActiveAsync(int id)
        {
            var doctor = UnitOfWork.GetRepository<Doctor>();

            var deActivetedDoctor = await doctor.FindAsync(c => c.Id == id);

            if (deActivetedDoctor is null)
                return NotFound();

            doctor.DeActiveAndActive(deActivetedDoctor);
            await UnitOfWork.SaveChangesAsync();
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteDoctor")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var doctor = UnitOfWork.GetRepository<Doctor>();
            var deletedDoctor = await doctor.FindAllAsyncPaginated(c => c.Id == id);

            if (deletedDoctor is null)
                return NotFound();

            var data = deletedDoctor.Where(c => c.Appointments.Select(v => v.Status).Contains(Status.pending)).Any();

            if (!data)
            {
                doctor.Delete(id);
                await UnitOfWork.SaveChangesAsync();
                return Ok();
            }
            return BadRequest();
        }

        [Authorize(Roles = "Admin, Doctor")]
        [HttpPut("DoctorAppointments")]
        public async Task<IActionResult> DoctorAppointments(SearchDTO d)
        {
            var userId = User.Claims.Where(c => c.Type == "DbUserId").FirstOrDefault()?.Value;
            var DoctorId = int.Parse(userId);

            var appointments = await UnitOfWork.GetRepository<Appointment>().FindAllAsyncPaginated(c =>
                (c.DoctorId == DoctorId) &&
                (d.ResevationDate == null ? true : c.ResevationDate == d.ResevationDate));

            var data = appointments.Select(c => new
            {
                c.PatientIdNavigation.PatientFullName,
                c.PatientIdNavigation.PatientImage,
                age = DateTime.Now.Year - c.PatientIdNavigation.PatientBirthDate.Year, 
                c.PatientIdNavigation.PatientGender,
                c.PatientIdNavigation.PatientPhone,
                c.PatientIdNavigation.PatientEmail,
                Appointment = new
                {
                    c.ResevationDate,
                    price = c.DoctorIdNavigation.DoctorSchedules.Where(v =>
                                (c.ResevationDate.DayOfWeek.Equals((DayOfWeek)v.ScheduleDay)) &&
                                (c.ResevationDate.TimeOfDay == v.ScheduleTime)).Select(v => v.Amount).SingleOrDefault(),
                }
            });
            return Ok(data);
        }

        [Authorize(Roles = "Admin, Doctor")]
        [HttpPut("Confirm")]
        public async Task<IActionResult> ConfirmingAppointment(int AppointmentId)
        {
            var appointment = UnitOfWork.GetRepository<Appointment>();

            var UpdatedAppointment = await appointment.FindAsync(c => c.Id == AppointmentId);

            if (UpdatedAppointment is null)
                return NotFound();

            UpdatedAppointment.Status = Status.completed;

            appointment.Update(UpdatedAppointment);

            await UnitOfWork.SaveChangesAsync();

            return Ok();
        }
    }
}
