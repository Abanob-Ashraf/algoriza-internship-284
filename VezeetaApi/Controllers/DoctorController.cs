using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;
using VezeetaApi.Domain;
using VezeetaApi.Domain.Dtos;
using VezeetaApi.Domain.Models;
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

        public DoctorController(IUnitOfWork unitOfWork, IMapper mapper, UserManager<AppUser> userManager)
        {
            UnitOfWork = unitOfWork;
            Mapper = mapper;
            UserManager = userManager;
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
            var doctor = await UnitOfWork.GetRepository<Doctor>().GetByIdAsync(id);
            if (doctor is null)
                return NotFound();
            var result = Mapper.Map<DoctorDTO>(doctor);
            return Ok(result);
        }

        [HttpPost("AddNewDoctor")]
        public async Task<IActionResult> AddAsync(DoctorDTO doctorDTO)
        {
            //Random random = new Random();
            //long newRandomNum = Convert.ToInt64(random.Next(1, 50) + doctorDTO.DocPhone) / 350000 * 2;

            var genericPassword = $"{doctorDTO.DocFirstName}#{doctorDTO.DocPhone}";
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

            var finalResult = Mapper.Map<DoctorDTO>(newDoctor);
            return Ok(finalResult);
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

        [HttpPut("DeActiveAndActive/{id}")]
        public async Task<IActionResult> DeActiveAndActiveAsync(int id)
        {
            var doctor = await UnitOfWork.GetRepository<Doctor>().FindAsync(c => c.Id == id);

            if (doctor is null)
                return NotFound();

            UnitOfWork.GetRepository<Doctor>().DeActiveAndActive(doctor);
            await UnitOfWork.SaveChangesAsync();
            return Ok();
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
