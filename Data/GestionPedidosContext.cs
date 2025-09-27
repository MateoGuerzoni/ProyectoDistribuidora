using Microsoft.EntityFrameworkCore;
using ProyectoDistribuidora.Models.Legacy;
using ProyectoDistribuidora.Models;
using ProyectoDistribuidora.Models.GestionPedidos;

namespace ProyectoDistribuidora.Data
{
    public class GestionPedidosContext : DbContext
    {
        public GestionPedidosContext(DbContextOptions<GestionPedidosContext> options) : base(options)
        {
        }

        public DbSet<UsuarioGP> Usuarios { get; set; } = default!;
        public DbSet<ClienteGP> Clientes { get; set; } = default!;
        public DbSet<StockGP> Stock { get; set; } = default!;
        public DbSet<PedidoGP> Pedidos { get; set; } = default!; 
        public DbSet<LineaPedidosGP> LineaPedidos { get; set; } = default!;
        public DbSet<SupervisorDeCargaGP> SupervisorDeCarga { get; set; }
        public DbSet<AdministracionGP> Administracion { get; set; }
        public DbSet<VendedorGP> Vendedores { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PedidoGP>()
                .Property(p => p.Total)
                .HasPrecision(18, 2); // Precision 18, escala 2

            // Configuración de la clave primaria compuesta para LineaPedidosGP
            modelBuilder.Entity<LineaPedidosGP>()
                .HasKey(lp => new { lp.IdPedido, lp.IdLineaPedido });

            modelBuilder.Entity<LineaPedidosGP>()
                .ToTable("LineaPedidos");

            // Configuración de precisión decimal para Subtotal
            modelBuilder.Entity<LineaPedidosGP>()
                .Property(lp => lp.Subtotal)
                .HasPrecision(18, 2);

            // Configuración de precisión decimal para Subtotal
            modelBuilder.Entity<LineaPedidosGP>()
                .Property(lp => lp.PrecioUnitario)
                .HasPrecision(18, 2);

            // Configuración de la clave primaria compuesta para UsuarioGP (IdUsuario, Rol)
            modelBuilder.Entity<UsuarioGP>()
                .HasKey(u => new { u.IdUsuario, u.Rol });

            modelBuilder.Entity<UsuarioGP>()
                .ToTable("Usuarios");

            // Configuración de la clave primaria para ClienteGP
            modelBuilder.Entity<ClienteGP>()
                .HasKey(c => c.NroCliente);

            modelBuilder.Entity<ClienteGP>()
                .ToTable("Clientes");

            modelBuilder.Entity<AdministracionGP>()
                .HasKey(a => a.ID);

            modelBuilder.Entity<AdministracionGP>()
                .ToTable("Administracion");

            modelBuilder.Entity<SupervisorDeCargaGP>()
                .HasKey(s => s.ID);

            modelBuilder.Entity<SupervisorDeCargaGP>()
                .ToTable("SupervisorDeCarga");

            modelBuilder.Entity<VendedorGP>()
                .HasKey(v => v.Nro_Vendedor);

            modelBuilder.Entity<VendedorGP>()
                .ToTable("Vendedores");

            // Configuración de la clave primaria para StockGP
            modelBuilder.Entity<StockGP>()
                .HasKey(s => s.Codigo);

            modelBuilder.Entity<StockGP>()
                .ToTable("Stock");

            modelBuilder.Entity<PedidoGP>()
                .HasKey(p => p.IdPedido);

            modelBuilder.Entity<PedidoGP>()
                .ToTable("Pedidos");
        }

    }
}


