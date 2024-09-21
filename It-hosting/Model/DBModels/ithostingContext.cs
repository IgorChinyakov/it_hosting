using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace It_hosting_2._0.Models.DBModels
{
    public partial class ithostingContext : DbContext
    {
        public ithostingContext()
        {
        }

        public ithostingContext(DbContextOptions<ithostingContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Branch> Branches { get; set; } = null!;
        public virtual DbSet<Collaborator> Collaborators { get; set; } = null!;
        public virtual DbSet<Commit> Commits { get; set; } = null!;
        public virtual DbSet<File> Files { get; set; } = null!;
        public virtual DbSet<PullRequest> PullRequests { get; set; } = null!;
        public virtual DbSet<Repository> Repositories { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySql("server=localhost;user=root;password=1234;database=it-hosting;charset=utf8", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.22-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");

            modelBuilder.Entity<Branch>(entity =>
            {
                entity.ToTable("branches");

                entity.HasIndex(e => e.RepositoryId, "FK_branches_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IsMain).HasColumnName("isMain");

                entity.Property(e => e.RepositoryId).HasColumnName("repository_id");

                entity.Property(e => e.Title)
                    .HasMaxLength(255)
                    .HasColumnName("title");

                entity.HasOne(d => d.Repository)
                    .WithMany(p => p.Branches)
                    .HasForeignKey(d => d.RepositoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_branches_id");
            });

            modelBuilder.Entity<Collaborator>(entity =>
            {
                entity.ToTable("collaborators");

                entity.HasIndex(e => e.RepositoryId, "FK_collaborators_repository_id");

                entity.HasIndex(e => e.UserId, "FK_collaborators_user_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.RepositoryId).HasColumnName("repository_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Repository)
                    .WithMany(p => p.Collaborators)
                    .HasForeignKey(d => d.RepositoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_collaborators_repository_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Collaborators)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_collaborators_user_id");
            });

            modelBuilder.Entity<Commit>(entity =>
            {
                entity.ToTable("commits");

                entity.HasIndex(e => e.FileId, "FK_commits_file_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatingDate)
                    .HasColumnType("datetime")
                    .HasColumnName("creating_date")
                    .HasDefaultValueSql("'0000-00-00 00:00:00'");

                entity.Property(e => e.FileId).HasColumnName("file_id");

                entity.Property(e => e.Text)
                    .HasColumnType("text")
                    .HasColumnName("text");

                entity.HasOne(d => d.File)
                    .WithMany(p => p.Commits)
                    .HasForeignKey(d => d.FileId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_commits_file_id");
            });

            modelBuilder.Entity<File>(entity =>
            {
                entity.ToTable("files");

                entity.HasIndex(e => e.BranchId, "FK_files_branch_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.BranchId).HasColumnName("branch_id");

                entity.Property(e => e.Text)
                    .HasColumnType("text")
                    .HasColumnName("text");

                entity.Property(e => e.Title)
                    .HasMaxLength(255)
                    .HasColumnName("title");

                entity.HasOne(d => d.Branch)
                    .WithMany(p => p.Files)
                    .HasForeignKey(d => d.BranchId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_files_branch_id");
            });

            modelBuilder.Entity<PullRequest>(entity =>
            {
                entity.ToTable("pull requests");

                entity.HasIndex(e => e.FromBranchId, "FK_pull requests_FirstBranchID");

                entity.HasIndex(e => e.ToBranchId, "FK_pull requests_SecondBranchID");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.FromBranchId).HasColumnName("from_branch_id");

                entity.Property(e => e.ToBranchId).HasColumnName("to_branch_id");

                entity.HasOne(d => d.FromBranch)
                    .WithMany(p => p.PullRequestFromBranches)
                    .HasForeignKey(d => d.FromBranchId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_pull requests_FirstBranchID");

                entity.HasOne(d => d.ToBranch)
                    .WithMany(p => p.PullRequestToBranches)
                    .HasForeignKey(d => d.ToBranchId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_pull requests_SecondBranchID");
            });

            modelBuilder.Entity<Repository>(entity =>
            {
                entity.ToTable("repositories");

                entity.HasIndex(e => e.UserId, "FK_repositories_user_id");

                entity.HasIndex(e => new { e.Id, e.UserId }, "UK_repositories")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .HasColumnName("description")
                    .HasDefaultValueSql("' '");

                entity.Property(e => e.IsPrivate).HasColumnName("isPrivate");

                entity.Property(e => e.Title)
                    .HasMaxLength(255)
                    .HasColumnName("title");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Repositories)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_repositories_user_id");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Image)
                    .HasMaxLength(255)
                    .HasColumnName("image")
                    .HasDefaultValueSql("'avatarka-pustaya-vk_0.jpg'");

                entity.Property(e => e.Login)
                    .HasMaxLength(255)
                    .HasColumnName("login");

                entity.Property(e => e.Nickname)
                    .HasMaxLength(255)
                    .HasColumnName("nickname");

                entity.Property(e => e.Password)
                    .HasMaxLength(255)
                    .HasColumnName("password");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
