using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VezeetaApi.Domain.Services;

namespace VezeetaApi.Infrastructure.Repositories
{
    public class InitializeDefaultDataRepository : IInitializeDefaultData
    {
        public RoleManager<IdentityRole> RoleManager { get; }
        public UserManager<AppUser> UserManager { get; }


        public InitializeDefaultDataRepository(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            RoleManager = roleManager;
            UserManager = userManager;
        }

        public async Task InitializeData()
        {
            IdentityRole admin = new IdentityRole();
            admin.Name = "Admin";

            IdentityRole doctor = new IdentityRole();
            doctor.Name = "Doctor";

            IdentityRole patient = new IdentityRole();
            patient.Name = "Patient";

            await RoleManager.CreateAsync(admin);
            await RoleManager.CreateAsync(doctor);
            await RoleManager.CreateAsync(patient);

            string GeneralPWD = "Admin$123";

            var Admin = new AppUser
            {
                FirstName = "Admin",
                LastName = "Admin",
                UserName = "admin@vezeeta.org",
                Email = "admin@vezeeta.org",
                EmailConfirmed = true,
                PasswordHash = GeneralPWD
            };
            await UserManager.CreateAsync(Admin, GeneralPWD);
            await UserManager.AddToRoleAsync(Admin, "Doctor");

            var Doctor = new AppUser
            {
                FirstName = "Test",
                LastName = "Doctor",
                UserName = "testdoctor@vezeeta.org",
                Email = "testdoctor@vezeeta.org",
                EmailConfirmed = true,
                PasswordHash = GeneralPWD
            };
            await UserManager.CreateAsync(Doctor, GeneralPWD);
            await UserManager.AddToRoleAsync(Doctor, "Patient");

            string PatientPWD = "Patient$123";

            var Patient = new AppUser
            {
                FirstName = "Test",
                LastName = "Patient",
                UserName = "testpatient@vezeeta.com",
                Email = "testpatient@roshvezeetaetaclinic.com",
                EmailConfirmed = true,
                PasswordHash = PatientPWD
            };
            await UserManager.CreateAsync(Patient, PatientPWD);
            await UserManager.AddToRoleAsync(Patient, "Patient");
        }
    }
}
