using System;
using System.Collections.Generic;
using GestionnaireInventaire.Models;
using Microsoft.EntityFrameworkCore;

namespace GestionnaireInventaire.Data;

public partial class InventaireDbContext : DbContext
{
    public InventaireDbContext()
    {
    }

    public InventaireDbContext(DbContextOptions<InventaireDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Produit> Produits { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=LAP-LYES;Database=inventaire;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__categori__3213E83FE9DC4631");

            entity.ToTable("categories");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nom)
                .HasMaxLength(50)
                .HasColumnName("nom");
        });

        modelBuilder.Entity<Produit>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__produits__3213E83F8CD6152A");

            entity.ToTable("produits");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.IdCategorie).HasColumnName("id_categorie");
            entity.Property(e => e.Nom)
                .HasMaxLength(100)
                .HasColumnName("nom");
            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.IdCategorieNavigation).WithMany(p => p.Produits)
                .HasForeignKey(d => d.IdCategorie)
                .HasConstraintName("FK_produits_categories");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
