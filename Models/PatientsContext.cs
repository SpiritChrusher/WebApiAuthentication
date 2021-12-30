using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace WebApiAuthentication.Models;
public class PatientsContext : DbContext
{
    public DbSet<PatientDto> Patients { get; set; }

    public PatientsContext([NotNullAttribute] DbContextOptions options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<PatientDto>();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer();
    }
}