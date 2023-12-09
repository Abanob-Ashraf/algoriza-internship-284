using AutoMapper;
using VezeetaApi.Domain.Dtos;
using VezeetaApi.Domain.Models;
using VezeetaApi.Infrastructure.Repositories;

namespace VezeetaApi.Infrastructure.AutoMapperConfig
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<DoctorSpecializionDTO, DoctorSpecializion>()
                .ForMember(dist => dist.Id, src => src.MapFrom(src => src.Id))
                .ForMember(dist => dist.SpecializationName, src => src.MapFrom(src => src.SpecializationName))
                .ForMember(dist => dist.Doctors, src => src.Ignore())
                .ReverseMap();

            CreateMap<DoctorDTO, Doctor>()
                .ForMember(dist => dist.Id, src => src.MapFrom(src => src.Id))
                .ForMember(dist => dist.DocImage, src => src.MapFrom(src => src.DocImage))
                .ForMember(dist => dist.DoctorFullName, src => src.Ignore())
                .ForMember(dist => dist.DocFirstName, src => src.MapFrom(src => src.DocFirstName))
                .ForMember(dist => dist.DocLastName, src => src.MapFrom(src => src.DocLastName))
                .ForMember(dist => dist.DocBirthDate, src => src.MapFrom(src => src.DocBirthDate))
                .ForMember(dist => dist.DocGender, src => src.MapFrom(src => src.DocGender))
                .ForMember(dist => dist.DocPhone, src => src.MapFrom(src => src.DocPhone))
                .ForMember(dist => dist.DocEmail, src => src.MapFrom(src => src.DocEmail))
                .ForMember(dist => dist.DocPassword, src => src.MapFrom(src => src.DocPassword))
                .ForMember(dist => dist.SpecializationId, src => src.MapFrom(src => src.SpecializationId))
                .ForMember(dist => dist.SpecializationIdNavigation, src => src.Ignore())
                .ForMember(dist => dist.Appointments, src => src.Ignore())
                .ForMember(dist => dist.DoctorSchedules, src => src.Ignore())
                .ReverseMap();

            CreateMap<DoctorScheduleDTO, DoctorSchedule>()
                .ForMember(dist => dist.Id, src => src.MapFrom(src => src.Id))
                .ForMember(dist => dist.Amount, src => src.MapFrom(src => src.Amount))
                .ForMember(dist => dist.ScheduleDay, src => src.MapFrom(src => src.ScheduleDay))
                .ForMember(dist => dist.ScheduleTime, src => src.MapFrom(src => src.ScheduleTime))
                .ForMember(dist => dist.DoctorId, src => src.MapFrom(src => src.DoctorId))
                .ForMember(dist => dist.DoctorIdNavigation, src => src.Ignore())
                .ReverseMap();
            
            
            CreateMap<DiscountDTO, Discount>()
                .ForMember(dist => dist.Id, src => src.MapFrom(src => src.Id))
                .ForMember(dist => dist.DiscountCode, src => src.MapFrom(src => src.DiscountCode))
                .ForMember(dist => dist.NumOfCompletedRequests, src => src.MapFrom(src => src.NumOfCompletedRequests))
                .ForMember(dist => dist.DiscountType, src => src.MapFrom(src => src.DiscountType))
                .ForMember(dist => dist.DiscountValue, src => src.MapFrom(src => src.DiscountValue))
                .ForMember(dist => dist.Appointments, src => src.Ignore())
                .ReverseMap();


            CreateMap<PatientDTO, Patient>()
                .ForMember(dist => dist.Id, src => src.MapFrom(src => src.Id))
                .ForMember(dist => dist.PatientImage, src => src.MapFrom(src => src.PatientImage))
                .ForMember(dist => dist.PatientFullName, src => src.Ignore())
                .ForMember(dist => dist.PatientFirstName, src => src.MapFrom(src => src.PatientFirstName))
                .ForMember(dist => dist.PatientLastName, src => src.MapFrom(src => src.PatientLastName))
                .ForMember(dist => dist.PatientBirthDate, src => src.MapFrom(src => src.PatientBirthDate))
                .ForMember(dist => dist.PatientGender, src => src.MapFrom(src => src.PatientGender))
                .ForMember(dist => dist.PatientPhone, src => src.MapFrom(src => src.PatientPhone))
                .ForMember(dist => dist.PatientEmail, src => src.MapFrom(src => src.PatientEmail))
                .ForMember(dist => dist.PatientPassword, src => src.MapFrom(src => src.PatientPassword))
                .ForMember(dist => dist.Appointments, src => src.Ignore())
                .ReverseMap();


            CreateMap<AppointmentDTO, Appointment>()
                .ForMember(dist => dist.Id, src => src.MapFrom(src => src.Id))
                .ForMember(dist => dist.ResevationDate, src => src.MapFrom(src => src.ResevationDate))
                .ForMember(dist => dist.Status, src => src.MapFrom(src => src.Status))
                .ForMember(dist => dist.PatientId, src => src.MapFrom(src => src.PatientId))
                .ForMember(dist => dist.DoctorId, src => src.MapFrom(src => src.DoctorId))
                .ForMember(dist => dist.DiscountId, src => src.MapFrom(src => src.DiscountId))
                .ForMember(dist => dist.PatientIdNavigation, src => src.Ignore())
                .ForMember(dist => dist.DoctorIdNavigation, src => src.Ignore())
                .ForMember(dist => dist.DiscountIdNavigation, src => src.Ignore())
                .ReverseMap();

            CreateMap<AppUser, RegisterDTO>()
               .ForMember(dist => dist.FirstName, src => src.MapFrom(src => src.FirstName))
               .ForMember(dist => dist.LastName, src => src.MapFrom(src => src.LastName))
               .ForMember(dist => dist.Email, src => src.MapFrom(src => src.Email))
               .ForMember(dist => dist.PhoneNumber, src => src.MapFrom(src => src.PhoneNumber))
               .ReverseMap();
        }
    }
}
