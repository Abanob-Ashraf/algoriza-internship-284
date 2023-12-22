using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using VezeetaApi.Domain;
using VezeetaApi.Domain.Dtos.CustomDtos;
using VezeetaApi.Domain.Models;

namespace VezeetaApi.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticController : ControllerBase
    {
        readonly IUnitOfWork UnitOfWork;
        readonly FilterTimeDTO FilterTimeDTO;

        public StatisticController(IUnitOfWork unitOfWork, FilterTimeDTO filterTimeDTO)
        {
            UnitOfWork = unitOfWork;
            FilterTimeDTO = filterTimeDTO;
        }

        [HttpGet("NumOfDoctors")]
        public async Task<IActionResult> NumOfDoctors()
        {
            var allDoctors = await UnitOfWork.GetRepository<Doctor>().GetAllAsync();
            return Ok(allDoctors.Count());
        }

        [HttpGet("NumOfPatients")]
        public async Task<IActionResult> NumOfPatients()
        {
            var allPatients = await UnitOfWork.GetRepository<Patient>().GetAllAsync();
            return Ok(allPatients.Count());
        }

        [HttpGet("NumOfAppointments")]
        public async Task<IActionResult> NumOfAppointments(Time id)
        {
            var allAppointments = await UnitOfWork.GetRepository<Appointment>().GetAllAsync();

            var filter = FilterTimeDTO.FilterByTime(id);

            var filteredAppointment = allAppointments.Where(c => c.ResevationDate >= filter && c.ResevationDate <= FilterTimeDTO.CurrentDate);

            var data = new
            {
                allAppointments = filteredAppointment.Count(),
                Pending = filteredAppointment.Count(c => c.Status == Status.pending),
                Completed = filteredAppointment.Count(c => c.Status == Status.completed),
                Cancelled = filteredAppointment.Count(c => c.Status == Status.cancelled),
            };

            return Ok(data);
        }

        [HttpGet("TopFiveSpecializations")]
        public async Task<IActionResult> TopFiveSpecializations(Time id)
        {
            var allAppointments = await UnitOfWork.GetRepository<Appointment>().GetAllAsync();

            var filter = FilterTimeDTO.FilterByTime(id);

            var topFiveSpecializations = allAppointments
                .Where(c => c.ResevationDate >= filter && c.ResevationDate <= FilterTimeDTO.CurrentDate)
                .GroupBy(c => c.DoctorIdNavigation.SpecializationIdNavigation.SpecializationName)
                .Select(v => new
                {
                    SpecializationName = v.Key,
                    NumOfReq = v.Count()
                })
                .OrderByDescending(v => v.NumOfReq)
                .Take(5);
            return Ok(topFiveSpecializations);
        }

        [HttpGet("TopTenDoctors")]
        public async Task<IActionResult> TopTenDoctors(Time id)
        {
            var allAppointments = await UnitOfWork.GetRepository<Appointment>().GetAllAsync();

            var filter = FilterTimeDTO.FilterByTime(id);

            var TopTenDoctors = allAppointments
                .Where(c => c.ResevationDate >= filter && c.ResevationDate <= FilterTimeDTO.CurrentDate)
                .GroupBy(c => c.DoctorIdNavigation)
                .Select(v => new
                {
                    Image = v.Key.DocImage,
                    FullName = v.Key.DoctorFullName,
                    specialize = v.Key.SpecializationIdNavigation.SpecializationName,
                    NumOfReq = v.Count()
                })
                .OrderByDescending(v => v.NumOfReq)
                .Take(10);

            return Ok(TopTenDoctors);
        }
    }   
}
