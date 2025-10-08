using Microsoft.EntityFrameworkCore;
using FinanzAPP.Models;
using System.IO;
using System;

namespace FinanzAPP.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Transaccion> Transacciones { get; set; }
        public DbSet<Ingreso> Ingresos { get; set; }
        public DbSet<Objetivo> Objetivos { get; set; }
        public DbSet<Aporte> Aportes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var carpetaDatos = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "FinanzAPP"
            );

            if (!Directory.Exists(carpetaDatos))
            {
                Directory.CreateDirectory(carpetaDatos);
            }

            var rutaDB = Path.Combine(carpetaDatos, "finanzas.db");

            optionsBuilder.UseSqlite($"Data Source={rutaDB}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configurar relación Aporte -> Objetivo
            modelBuilder.Entity<Aporte>()
                .HasOne(a => a.Objetivo)
                .WithMany()
                .HasForeignKey(a => a.ObjetivoId);
        }
    }
}