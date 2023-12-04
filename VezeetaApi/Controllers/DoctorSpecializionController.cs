﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
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
            var docSpecs = await UnitOfWork.GetRepository<DoctorSpecializion>().GetAllAsync();
            if (docSpecs is null)
                return NotFound();
            return Ok(docSpecs);
        }

        [HttpGet("Specializion/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var oneDocSpec = await UnitOfWork.GetRepository<DoctorSpecializion>().GetByIdAsync(id);
            if (oneDocSpec is null) 
                return NotFound();

            return Ok(oneDocSpec);
        }

        [HttpPost("AddNewSpecializion")]
        public async Task<IActionResult> AddAsync(DoctorSpecializionDTO doctorSpecializionDTO)
        {
            var newDocSpec = Mapper.Map<DoctorSpecializion>(doctorSpecializionDTO);
            await UnitOfWork.GetRepository<DoctorSpecializion>().AddAsync(newDocSpec);
            await UnitOfWork.SaveChanges();
            return Ok();
        }

        [HttpPut("UpdateSpecializion")]
        public async Task<IActionResult> UpdateAsync(DoctorSpecializionDTO doctorSpecializionDTO)
        {
            var docSpec = await UnitOfWork.GetRepository<DoctorSpecializion>().FindAsync(c => c.Id == doctorSpecializionDTO.Id);

            if(docSpec is null)
                return NotFound(doctorSpecializionDTO);

            //docSpec = Mapper.Map<DoctorSpecializion>(doctorSpecializionDTO);

            docSpec.SpecializationName = doctorSpecializionDTO.SpecializationName;

            UnitOfWork.GetRepository<DoctorSpecializion>().Update(docSpec);
            await UnitOfWork.SaveChanges();

            return Ok(docSpec);
        }

        [HttpDelete("DeleteSpecializion")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var docSpec = UnitOfWork.GetRepository<DoctorSpecializion>().Delete(id);

            if (docSpec is null)
                return NotFound();

            await UnitOfWork.SaveChanges();
            return Ok(docSpec);
        }

    }
}