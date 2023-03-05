namespace P01_HospitalDatabase.Data;

using Microsoft.EntityFrameworkCore;

using Models.Common;
using Data.Models;
using System.Security.Cryptography;

public class HospitalContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured!)
        {
            optionsBuilder.UseSqlServer(DbConfig.ConnectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PatientMedicament>(e => e.HasKey(pk => new { pk.PatientId, pk.MedicamentId }));
            
    }
}