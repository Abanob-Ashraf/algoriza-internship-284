# Algoriza-internship-284

Vezeeta Website (Endpoints)


## About Clinic System Website

Admin User Name = admin@vezeeta.org
Admin Password = Admin$123

Doctor User Name = testdoctor@vezeeta.org
Doctor Password = Admin$123

Patient User Name = testpatient@vezeeta.com
Patient Password = Patient$123

## How to use it :

The Vezeeta Website (Endpoints) is designed to be user-friendly and intuitive, making it easy for both healthcare providers and patients to navigate. Here's a step-by-step guide on how to use the Endpoints effectively:

- You will need the latest Visual Studio 2022 and the latest .NET Core 7.
- You will need  MS SQL Server
- Make sure from the configuration in the **AppSettings.json** file that meets the application features :
    (**JWT** for Authantication and Authorization, **Email** Configuration Service)

### Email Configuration Section

```json
"EmailConfiguration": {
    "DisplayName": "",
    "Email": "",
    "Password": "",
    "UseSmtpServerrname": "",
    "Port": ""
  }
```

### JWT Configuration Section

```json
"JWT": {
    "SecureApi": "",
    "SecureApiUser": "",
    "DurationInMinutes": 30,
    "Key": "", 
  } 
```
- And you can make Your key from here  'https://8gwifi.org/jwsgen.jsp'

- Install Packages from **NuGet Pakage Manager** Or **Package Manager Console**
```
    Install-Package AutoMapper.Extensions.Microsoft.DependencyInjection
    Install-Package MailKit
    Install-Package MimeKit
    Install-Package Microsoft.AspNetCore.Identity.EntityFrameworkCore
    Install-Package Microsoft.EntityFrameworkCore
    Install-Package Microsoft.EntityFrameworkCore.Proxies
    Install-Package Microsoft.EntityFrameworkCore.Tools
    Install-Package Microsoft.EntityFrameworkCore.SqlServer
    Install-Package Microsoft.Extensions.Hosting.Abstractions
    Install-Package Microsoft.AspNetCore.Http.Features
    Install-Package Microsoft.AspNetCore.Authentication.JwtBearer
    Install-Package Microsoft.AspNetCore.OpenApi
    Install-Package Microsoft.EntityFrameworkCore.Design
    Install-Package Swashbuckle.AspNetCore
```

- In **Package Manager Console** make Db migration using command

```cmd
add-migration init
```
- Write another command to update the database

```cmd
updat
```
- Change the connection string (SQL Server, username & password )

```json
 "ConnectionStrings": {
    "DefaultConnection": "Data Source=[Server Name];Initial Catalog=Clinical_DB;User ID=[Sql server Username]];Password=[Sql server Password];Integrated Security=True"
  }
```
the application will run on route 'https://localhost:7097'

## Features :

- User Registration and Login:
  Patients and healthcare providers can register and log in securely.


- Patient Management:
  Patients can create and manage their profiles, including contact information.

- Appointment Booking:
  Patients can schedule appointments with their preferred healthcare providers.
  Healthcare providers can manage appointments efficiently.

- Notifications:
  Automated notifications are sent to Doctors with there Email and Password.

- User Roles:
Different user roles, including patients, healthcare providers, and administrators.

- Administrator Controls Panal:
   User management for adding and managing healthcare professionals.
   Customization options to tailor the system to the clinic's specific needs.
   Add Discount Coupons.


## DataBase
- [Digram](https://dbdiagram.io/d/Vezeeta-Website-Endpoints-65660b063be1495787e76d59)

- [BackUp](https://github.com/Abanob-Ashraf/algoriza-internship-284/tree/main/DataBaseBackUp)


#### Refrences

- [ASP .NET CORE API](https://learn.microsoft.com/en-us/aspnet/core/?view=aspnetcore-7.0).