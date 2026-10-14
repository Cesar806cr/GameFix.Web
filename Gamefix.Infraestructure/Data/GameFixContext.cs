using System;
using System.Collections.Generic;
using Gamefix.Infraestructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Gamefix.Infraestructure.Data;

public partial class GameFixContext : DbContext
{
    public GameFixContext(DbContextOptions<GameFixContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BloqueoHorario> BloqueoHorario { get; set; }

    public virtual DbSet<CategoriaProducto> CategoriaProducto { get; set; }

    public virtual DbSet<DetalleFactura> DetalleFactura { get; set; }

    public virtual DbSet<EstadoReserva> EstadoReserva { get; set; }

    public virtual DbSet<Factura> Factura { get; set; }

    public virtual DbSet<HorarioSucursal> HorarioSucursal { get; set; }

    public virtual DbSet<Notificacion> Notificacion { get; set; }

    public virtual DbSet<Producto> Producto { get; set; }

    public virtual DbSet<Reserva> Reserva { get; set; }

    public virtual DbSet<Rol> Rol { get; set; }

    public virtual DbSet<Servicio> Servicio { get; set; }

    public virtual DbSet<Sucursal> Sucursal { get; set; }

    public virtual DbSet<Usuario> Usuario { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BloqueoHorario>(entity =>
        {
            entity.HasIndex(e => new { e.SucursalId, e.FechaInicio, e.FechaFin }, "IX_BloqueoHorario_SucursalFechas");

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.FechaRegistro)
                .HasPrecision(0)
                .HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.HoraFin).HasPrecision(0);
            entity.Property(e => e.HoraInicio).HasPrecision(0);
            entity.Property(e => e.Motivo).HasMaxLength(300);

            entity.HasOne(d => d.Sucursal).WithMany(p => p.BloqueoHorario)
                .HasForeignKey(d => d.SucursalId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BloqueoHorario_Sucursal");

            entity.HasOne(d => d.UsuarioRegistro).WithMany(p => p.BloqueoHorario)
                .HasForeignKey(d => d.UsuarioRegistroId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BloqueoHorario_Usuario");
        });

        modelBuilder.Entity<CategoriaProducto>(entity =>
        {
            entity.HasIndex(e => e.Nombre, "UQ_CategoriaProducto_Nombre").IsUnique();

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Descripcion).HasMaxLength(250);
            entity.Property(e => e.Nombre).HasMaxLength(80);
        });

        modelBuilder.Entity<DetalleFactura>(entity =>
        {
            entity.ToTable(tb => tb.HasTrigger("TR_DetalleFactura_ActualizarTotales"));

            entity.HasIndex(e => e.FacturaId, "IX_DetalleFactura_Factura");

            entity.HasIndex(e => e.ProductoId, "IX_DetalleFactura_Producto");

            entity.HasIndex(e => e.ServicioId, "IX_DetalleFactura_Servicio");

            entity.Property(e => e.Cantidad).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Descripcion).HasMaxLength(250);
            entity.Property(e => e.PorcentajeImpuesto)
                .HasDefaultValue(13.00m)
                .HasColumnType("decimal(5, 2)");
            entity.Property(e => e.PrecioUnitario).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.Subtotal)
                .HasComputedColumnSql("(CONVERT([decimal](12,2),[Cantidad]*[PrecioUnitario]))", true)
                .HasColumnType("decimal(12, 2)");

            entity.HasOne(d => d.Factura).WithMany(p => p.DetalleFactura)
                .HasForeignKey(d => d.FacturaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DetalleFactura_Factura");

            entity.HasOne(d => d.Producto).WithMany(p => p.DetalleFactura)
                .HasForeignKey(d => d.ProductoId)
                .HasConstraintName("FK_DetalleFactura_Producto");

            entity.HasOne(d => d.Servicio).WithMany(p => p.DetalleFactura)
                .HasForeignKey(d => d.ServicioId)
                .HasConstraintName("FK_DetalleFactura_Servicio");
        });

        modelBuilder.Entity<EstadoReserva>(entity =>
        {
            entity.HasIndex(e => e.Nombre, "UQ_EstadoReserva_Nombre").IsUnique();

            entity.Property(e => e.EstadoReservaId).ValueGeneratedNever();
            entity.Property(e => e.ColorHexadecimal)
                .HasMaxLength(7)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Descripcion).HasMaxLength(200);
            entity.Property(e => e.Nombre).HasMaxLength(30);
        });

        modelBuilder.Entity<Factura>(entity =>
        {
            entity.HasIndex(e => new { e.ClienteId, e.FechaEmision }, "IX_Factura_ClienteFecha");

            entity.HasIndex(e => new { e.EstadoFactura, e.FechaEmision }, "IX_Factura_EstadoFecha");

            entity.HasIndex(e => e.ReservaId, "IX_Factura_Reserva");

            entity.HasIndex(e => new { e.SucursalId, e.FechaEmision }, "IX_Factura_SucursalFecha");

            entity.HasIndex(e => e.NumeroFactura, "UQ_Factura_Numero").IsUnique();

            entity.Property(e => e.EsProforma).HasDefaultValue(true);
            entity.Property(e => e.EstadoFactura)
                .HasMaxLength(20)
                .HasDefaultValue("Proforma");
            entity.Property(e => e.FechaCorreoEnviado).HasPrecision(0);
            entity.Property(e => e.FechaEmision)
                .HasPrecision(0)
                .HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.FechaPago).HasPrecision(0);
            entity.Property(e => e.Impuesto).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.NumeroFactura).HasMaxLength(30);
            entity.Property(e => e.Observaciones).HasMaxLength(500);
            entity.Property(e => e.PorcentajeImpuesto)
                .HasDefaultValue(13.00m)
                .HasColumnType("decimal(5, 2)");
            entity.Property(e => e.RutaComprobantePdf).HasMaxLength(500);
            entity.Property(e => e.Subtotal).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.Total).HasColumnType("decimal(12, 2)");

            entity.HasOne(d => d.Cliente).WithMany(p => p.FacturaCliente)
                .HasForeignKey(d => d.ClienteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Factura_Cliente");

            entity.HasOne(d => d.Reserva).WithMany(p => p.Factura)
                .HasForeignKey(d => d.ReservaId)
                .HasConstraintName("FK_Factura_Reserva");

            entity.HasOne(d => d.Sucursal).WithMany(p => p.Factura)
                .HasForeignKey(d => d.SucursalId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Factura_Sucursal");

            entity.HasOne(d => d.UsuarioRegistro).WithMany(p => p.FacturaUsuarioRegistro)
                .HasForeignKey(d => d.UsuarioRegistroId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Factura_UsuarioRegistro");
        });

        modelBuilder.Entity<HorarioSucursal>(entity =>
        {
            entity.HasIndex(e => new { e.SucursalId, e.DiaSemana }, "IX_HorarioSucursal_SucursalDia");

            entity.HasIndex(e => new { e.SucursalId, e.DiaSemana }, "UQ_HorarioSucursal_Dia").IsUnique();

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.HoraApertura).HasPrecision(0);
            entity.Property(e => e.HoraCierre).HasPrecision(0);

            entity.HasOne(d => d.Sucursal).WithMany(p => p.HorarioSucursal)
                .HasForeignKey(d => d.SucursalId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HorarioSucursal_Sucursal");
        });

        modelBuilder.Entity<Notificacion>(entity =>
        {
            entity.HasIndex(e => e.ClienteId, "IX_Notificacion_Cliente");

            entity.HasIndex(e => new { e.FechaProgramada, e.Enviada }, "IX_Notificacion_Programada");

            entity.HasIndex(e => e.ReservaId, "IX_Notificacion_Reserva");

            entity.Property(e => e.DetalleError).HasMaxLength(1000);
            entity.Property(e => e.FechaEnviada).HasPrecision(0);
            entity.Property(e => e.FechaProgramada).HasPrecision(0);
            entity.Property(e => e.Medio).HasMaxLength(20);
            entity.Property(e => e.Mensaje).HasMaxLength(1000);
            entity.Property(e => e.TipoNotificacion).HasMaxLength(30);

            entity.HasOne(d => d.Cliente).WithMany(p => p.Notificacion)
                .HasForeignKey(d => d.ClienteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Notificacion_Cliente");

            entity.HasOne(d => d.Reserva).WithMany(p => p.Notificacion)
                .HasForeignKey(d => d.ReservaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Notificacion_Reserva");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasIndex(e => e.Activo, "IX_Producto_Activo");

            entity.HasIndex(e => e.CategoriaProductoId, "IX_Producto_Categoria");

            entity.HasIndex(e => e.Existencia, "IX_Producto_Existencia");

            entity.HasIndex(e => e.Nombre, "IX_Producto_Nombre");

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Compatibilidad).HasMaxLength(200);
            entity.Property(e => e.Descripcion).HasMaxLength(500);
            entity.Property(e => e.FechaRegistro)
                .HasPrecision(0)
                .HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Marca).HasMaxLength(100);
            entity.Property(e => e.Nombre).HasMaxLength(120);
            entity.Property(e => e.NombreImagen).HasMaxLength(150);
            entity.Property(e => e.Precio).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.TipoImagen).HasMaxLength(50);

            entity.HasOne(d => d.CategoriaProducto).WithMany(p => p.Producto)
                .HasForeignKey(d => d.CategoriaProductoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Producto_Categoria");
        });

        modelBuilder.Entity<Reserva>(entity =>
        {
            entity.HasIndex(e => new { e.ClienteId, e.FechaHoraInicio }, "IX_Reserva_ClienteFecha");

            entity.HasIndex(e => new { e.FechaHoraInicio, e.EstadoReservaId }, "IX_Reserva_FechaEstado");

            entity.HasIndex(e => e.ServicioId, "IX_Reserva_Servicio");

            entity.HasIndex(e => new { e.SucursalId, e.EstadoReservaId, e.FechaHoraInicio }, "IX_Reserva_SucursalEstado");

            entity.HasIndex(e => new { e.SucursalId, e.FechaHoraInicio, e.FechaHoraFin }, "IX_Reserva_SucursalFecha");

            entity.HasIndex(e => e.CodigoReserva, "UQ_Reserva_Codigo").IsUnique();

            entity.Property(e => e.CodigoReserva).HasMaxLength(20);
            entity.Property(e => e.DiagnosticoEncargado).HasMaxLength(1000);
            entity.Property(e => e.EstadoReservaId).HasDefaultValue(1);
            entity.Property(e => e.FechaActualizacion).HasPrecision(0);
            entity.Property(e => e.FechaHoraAnterior).HasPrecision(0);
            entity.Property(e => e.FechaHoraFin).HasPrecision(0);
            entity.Property(e => e.FechaHoraInicio).HasPrecision(0);
            entity.Property(e => e.FechaNotificacion).HasPrecision(0);
            entity.Property(e => e.FechaRegistro)
                .HasPrecision(0)
                .HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.HaceRuidoLectora).HasDefaultValue(false);
            entity.Property(e => e.MotivoReprogramacion).HasMaxLength(500);
            entity.Property(e => e.ObservacionesCliente).HasMaxLength(1000);
            entity.Property(e => e.ProblemaReportado).HasMaxLength(1000);
            entity.Property(e => e.TipoConsola).HasMaxLength(100);

            entity.HasOne(d => d.Cliente).WithMany(p => p.ReservaCliente)
                .HasForeignKey(d => d.ClienteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Reserva_Cliente");

            entity.HasOne(d => d.EstadoReserva).WithMany(p => p.Reserva)
                .HasForeignKey(d => d.EstadoReservaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Reserva_Estado");

            entity.HasOne(d => d.Servicio).WithMany(p => p.Reserva)
                .HasForeignKey(d => d.ServicioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Reserva_Servicio");

            entity.HasOne(d => d.Sucursal).WithMany(p => p.Reserva)
                .HasForeignKey(d => d.SucursalId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Reserva_Sucursal");

            entity.HasOne(d => d.UsuarioRegistro).WithMany(p => p.ReservaUsuarioRegistro)
                .HasForeignKey(d => d.UsuarioRegistroId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Reserva_UsuarioRegistro");

            entity.HasOne(d => d.UsuarioReprogramo).WithMany(p => p.ReservaUsuarioReprogramo)
                .HasForeignKey(d => d.UsuarioReprogramoId)
                .HasConstraintName("FK_Reserva_UsuarioReprogramo");
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasIndex(e => e.Nombre, "UQ_Rol_Nombre").IsUnique();

            entity.Property(e => e.RolId).ValueGeneratedNever();
            entity.Property(e => e.Descripcion).HasMaxLength(200);
            entity.Property(e => e.Nombre).HasMaxLength(30);
        });

        modelBuilder.Entity<Servicio>(entity =>
        {
            entity.HasIndex(e => e.Activo, "IX_Servicio_Activo");

            entity.HasIndex(e => e.Nombre, "IX_Servicio_Nombre");

            entity.HasIndex(e => e.Nombre, "UQ_Servicio_Nombre").IsUnique();

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.ConsolaCompatible).HasMaxLength(150);
            entity.Property(e => e.Descripcion).HasMaxLength(500);
            entity.Property(e => e.FechaRegistro)
                .HasPrecision(0)
                .HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.NivelDificultad).HasMaxLength(20);
            entity.Property(e => e.Nombre).HasMaxLength(120);
            entity.Property(e => e.Precio).HasColumnType("decimal(12, 2)");
        });

        modelBuilder.Entity<Sucursal>(entity =>
        {
            entity.HasIndex(e => e.CorreoElectronico, "UQ_Sucursal_Correo").IsUnique();

            entity.HasIndex(e => e.Nombre, "UQ_Sucursal_Nombre").IsUnique();

            entity.Property(e => e.Activa).HasDefaultValue(true);
            entity.Property(e => e.CorreoElectronico).HasMaxLength(150);
            entity.Property(e => e.Descripcion).HasMaxLength(500);
            entity.Property(e => e.DireccionExacta).HasMaxLength(300);
            entity.Property(e => e.FechaRegistro)
                .HasPrecision(0)
                .HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Telefono).HasMaxLength(25);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasIndex(e => e.Nombre, "IX_Usuario_Nombre");

            entity.HasIndex(e => e.RolId, "IX_Usuario_Rol");

            entity.HasIndex(e => e.SucursalId, "IX_Usuario_Sucursal");

            entity.HasIndex(e => e.CorreoElectronico, "UQ_Usuario_Correo").IsUnique();

            entity.HasIndex(e => e.RolId, "UX_Usuario_UnicoAdministrador")
                .IsUnique()
                .HasFilter("([RolId]=(1))");

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.ContrasenaHash).HasMaxLength(500);
            entity.Property(e => e.CorreoElectronico).HasMaxLength(150);
            entity.Property(e => e.DireccionExacta).HasMaxLength(300);
            entity.Property(e => e.FechaRegistro)
                .HasPrecision(0)
                .HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.FechaUltimoAcceso).HasPrecision(0);
            entity.Property(e => e.Identificacion).HasMaxLength(30);
            entity.Property(e => e.Nombre).HasMaxLength(120);
            entity.Property(e => e.Telefono).HasMaxLength(25);

            entity.HasOne(d => d.Rol).WithOne(p => p.Usuario)
                .HasForeignKey<Usuario>(d => d.RolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Usuario_Rol");

            entity.HasOne(d => d.Sucursal).WithMany(p => p.Usuario)
                .HasForeignKey(d => d.SucursalId)
                .HasConstraintName("FK_Usuario_Sucursal");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
