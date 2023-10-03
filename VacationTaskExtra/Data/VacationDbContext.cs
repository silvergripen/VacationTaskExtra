
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using VacationTaskExtra.Models;


namespace VacationTaskExtra.Data
{
    public class VacationDbContext : IdentityDbContext<PersonelModel>
    {
        public VacationDbContext(DbContextOptions<VacationDbContext> options) : base(options)
        {
        }

        public DbSet<WaitingRequestModel> WaitingRequests { get; set; }
        public DbSet<PersonelModel> Personels { get; set; }
        public DbSet<RequestVacationModel> RequestVacations { get; set; }
        public DbSet<VacationTypeModel> VacationTypes { get; set; }



    }
}
