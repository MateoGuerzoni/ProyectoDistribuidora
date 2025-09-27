using Microsoft.EntityFrameworkCore;
using System;
using ProyectoDistribuidora.Models.Legacy;
using ProyectoDistribuidora.Models;
using ProyectoDistribuidora.Models.GestionPedidos;

namespace ProyectoDistribuidora.Data
{
    public class LegacyContext : DbContext
    {

        public LegacyContext(DbContextOptions<LegacyContext> options) : base(options)
        {
        }

        public DbSet<Cliente> Clientes { get; set; } = default!;
        public DbSet<Stock> Stocks { get; set; } = default!;
        public DbSet<Usuario> Usuarios { get; set; } = default!;
        public DbSet<Log> Logs { get; set; } = default!;
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<LineaPedido> Linea_Pedidos { get; set; }
        public DbSet<SupervisorDeCarga> SupervisorDeCarga { get; set; }
        public DbSet<Auditoria> Auditoria { get; set; }
        public DbSet<Administracion> Administracion { get; set; }
        public DbSet<Vendedor> Vendedores { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cliente>()
        .ToTable("Clientes", t => t.HasTrigger("trg_UpdateClientes"));

            modelBuilder.Entity<Stock>()
       .ToTable("Stock", t => t.HasTrigger("trg_UpdateStock"));

            modelBuilder.Entity<Pedido>()
               .HasKey(p => p.IdPedido);

            modelBuilder.Entity<LineaPedido>()
            .Property(p => p.PrecioUnitario)
            .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<LineaPedido>()
           .Property(p => p.Subtotal)
           .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Administracion>()
                .HasKey(a => a.ID);

            modelBuilder.Entity<Administracion>()
                .ToTable("Administracion");

            modelBuilder.Entity<Stock>()
          .ToTable("Stock"); // Especifica el nombre correcto de la tabla
            
            // Configuración de la clave primaria compuesta para 'Usuarios'
            modelBuilder.Entity<Usuario>()
                .HasKey(u => new { u.IdUsuario, u.Rol });

            modelBuilder.Entity<Stock>()
          .ToTable("Stock");

            modelBuilder.Entity<Cliente>()
           .HasKey(c => c.Nro_Cliente); // Define la clave primaria

            modelBuilder.Entity<Auditoria>()
            .HasKey(a => a.AuditoriaId); // Define la clave primaria

            modelBuilder.Entity<Cliente>()
          .ToTable("Clientes");

            modelBuilder.Entity<Vendedor>()
            .HasKey(v => v.Nro_Vendedor); // Define la clave primaria

            modelBuilder.Entity<Vendedor>()
          .ToTable("Vendedores");

            modelBuilder.Entity<SupervisorDeCarga>()
            .HasKey(s => s.ID); // Define la clave primaria

            modelBuilder.Entity<SupervisorDeCarga>()
          .ToTable("SupervisorDeCarga");

            // Si también necesitas configurar claves compuestas en otras entidades, sigue el mismo patrón
            modelBuilder.Entity<LineaPedido>()
          .ToTable("Linea_Pedidos"); // Especifica el nombre correcto de la tabla

                    modelBuilder.Entity<LineaPedido>()
              .HasKey(lp => new { lp.IdPedido, lp.IdLineaPedido }); // Define la clave primaria compuesta

                    modelBuilder.Entity<LineaPedido>()
                        .HasOne<Pedido>()
                        .WithMany()
                        .HasForeignKey(lp => lp.IdPedido);

                    modelBuilder.Entity<LineaPedido>()
                        .HasOne<Stock>()
                        .WithMany()
                        .HasForeignKey(lp => lp.Codigo);

                    base.OnModelCreating(modelBuilder);

                    modelBuilder.Entity<Pedido>()
              .ToTable("Pedidos", t => t.HasTrigger("trg_Pedidos")); // Define explícitamente que tiene triggers
        }

    }

}
