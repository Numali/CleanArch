using Domain.StudentCRUD;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.StudentCRUD
{
    public class ApplicationDBContext:IdentityDbContext<AppUser>//identity is written as identity is applied 
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-L12VA5U;Database=Clean Architectur;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True");
        }

        public DbSet<Student> Students { get; set; }
    }
    
}
