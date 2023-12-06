using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VezeetaApi.Domain;
using VezeetaApi.Domain.Dtos;
using VezeetaApi.Domain.Models;

namespace VezeetaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        readonly IUnitOfWork UnitOfWork;
        readonly IMapper Mapper;

        public PatientController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            UnitOfWork = unitOfWork;
            Mapper = mapper;
        }

        [HttpGet("GetAllPatients")]
        public async Task<IActionResult> GetAllAsync()
        {
            var patients = await UnitOfWork.GetRepository<Patient>().GetAllAsync();
            if (patients == null)
                return NotFound();
            var result = Mapper.Map<IEnumerable<PatientDTO>>(patients);
            return Ok(result);
        }

        [HttpGet("GetPatient/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var onePatient = await UnitOfWork.GetRepository<Patient>().GetByIdAsync(id);
            if (onePatient is null)
                return NotFound();
            var result = Mapper.Map<PatientDTO>(onePatient);
            return Ok(result);
        }

        [HttpPost("AddNewPatient")]
        public async Task<IActionResult> AddAsync(PatientDTO patientDTO)
        {
            var newPatient = Mapper.Map<Patient>(patientDTO);
            await UnitOfWork.GetRepository<Patient>().AddAsync(newPatient);
            await UnitOfWork.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("UpdatePatient")]
        public async Task<IActionResult> UpdateAsync(PatientDTO patientDTO)
        {
            var patient = await UnitOfWork.GetRepository<Patient>().FindAsync(c => c.Id == patientDTO.Id);

            if (patient is null)
                return NotFound();

            patient.PatientImage = patientDTO.PatientImage;
            patient.PatientFirstName = patientDTO.PatientFirstName;
            patient.PatientLastName = patientDTO.PatientLastName;
            patient.PatientBirthDate = patientDTO.PatientBirthDate;
            patient.PatientGender = patientDTO.PatientGender;
            patient.PatientPhone = patientDTO.PatientPhone;
            patient.PatientEmail = patientDTO.PatientEmail;
            patient.PatientPassword = patientDTO.PatientPassword;

            UnitOfWork.GetRepository<Patient>().Update(patient);

            await UnitOfWork.SaveChangesAsync();

            var result = Mapper.Map<PatientDTO>(patient);
            return Ok(result);
        }

        [HttpPut("DeActiveAndActive/{id}")]
        public async Task<IActionResult> DeActiveAndActiveAsync(int id)
        {
            var patient = await UnitOfWork.GetRepository<Patient>().FindAsync(c => c.Id == id);

            if (patient is null)
                return NotFound();

            UnitOfWork.GetRepository<Patient>().DeActiveAndActive(patient);
            await UnitOfWork.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("DeletePatient")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var patient = UnitOfWork.GetRepository<Patient>().Delete(id);

            if (patient is null)
                return NotFound();

            await UnitOfWork.SaveChangesAsync();

            var result = Mapper.Map<PatientDTO>(patient);
            return Ok(result);
        }
    }
}
