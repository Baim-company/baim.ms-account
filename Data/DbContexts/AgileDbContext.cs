using Microsoft.EntityFrameworkCore;
using PersonalAccount.API.Models.Enums;
using PersonalAccount.API.Models.Entities.Clients;
using PersonalAccount.API.Models.Entities.Staffs;
using PersonalAccount.API.Models.Entities.Users;
using PersonalAccount.API.Models.Entities.Agiles.Comments;
using PersonalAccount.API.Models.Entities.Agiles.Projects;
using PersonalAccount.API.Models.Entities.Agiles.Tickets;
using PersonalAccount.API.Models.Entities.Agiles.ProjectTasks;
using PersonalAccount.API.Models.Entities.Agiles.SubTickets;
using PersonalAccount.API.Models.Entities.Agiles.CheckItems;
using PersonalAccount.API.Models.Entities.Agiles.CheckLists;
using System.Reflection.Emit;

namespace PersonalAccount.API.Data.DbContexts;

public class AgileDbContext : DbContext
{
    public AgileDbContext(DbContextOptions<AgileDbContext> options) : base(options) { }

    #region User
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    #endregion


    #region Client-User
    public DbSet<Client> Clients { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<CompanyTypeOfActivity> CompanyTypeOfActivities { get; set; }
    public DbSet<TypeOfActivity> TypeOfActivities { get; set; }
    #endregion


    #region Staff-User
    public DbSet<Staff> Staff { get; set; }
    public DbSet<StaffImage> StaffImages { get; set; }
    public DbSet<Certificate> Certificates { get; set; }
    #endregion


    #region Bitrix
    public DbSet<Project> Projects { get; set; }
    public DbSet<ProjectUser> ProjectUsers { get; set; }


    public DbSet<ProjectSubTiket> ProjectSubTikets { get; set; }
    public DbSet<ProjectSubTiketUser> ProjectSubTiketUsers { get; set; }


    public DbSet<ProjectTask> ProjectTasks { get; set; }
    public DbSet<ProjectTicketTaskUser> ProjectTicketTaskUsers { get; set; }
    public DbSet<ProjectSubTicketTaskUser> ProjectSubTicketTaskUsers { get; set; }


    public DbSet<ProjectTiket> ProjectTikets { get; set; }
    public DbSet<ProjectTiketUser> ProjectTiketUsers { get; set; }


    public DbSet<TaskRole> TaskRoles { get; set; }
    public DbSet<ProjectTaskCheckItem> ProjectTaskCheckItems { get; set; }
    public DbSet<ProjectTaskCheckList> ProjectTaskCheckLists { get; set; }
    public DbSet<ProjectTaskCheckItemFile> ProjectTaskCheckItemFiles { get; set; }

    public DbSet<Comment> Comments { get; set; }
    public DbSet<CommentFile> CommentFiles { get; set; }
    public DbSet<Reaction> Reactions { get; set; }
    public DbSet<Mention> Mentions { get; set; }

    #endregion


    protected override void OnModelCreating(ModelBuilder builder)
    {
        #region User

        builder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);

            entity.Property(u => u.Name).IsRequired();
            entity.Property(u => u.Surname).HasMaxLength(50);
            entity.Property(u => u.Patronymic).HasMaxLength(50);
            entity.Property(u => u.Position).HasMaxLength(50);
            entity.Property(u => u.BirthDate);
            entity.Property(u => u.Gender).HasConversion(
                    v => v.ToString(),
                    v => (Gender)Enum.Parse(typeof(Gender), v));

            entity.Property(u => u.Email).HasMaxLength(80).IsRequired();
            entity.Property(u => u.PersonalEmail).HasMaxLength(80);

            entity.Property(u => u.PhoneNumber).HasMaxLength(50);
            entity.Property(u => u.BusinessPhoneNumber).HasMaxLength(50);

            entity.Property(pi => pi.AvatarPath).HasMaxLength(100);


            entity.HasOne(u => u.Client)
                .WithOne(c => c.User)
                .HasForeignKey<Client>(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(u => u.Staff)
                .WithOne(e => e.User)
                .HasForeignKey<Staff>(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<UserRole>(entity =>
        {
            entity.HasKey(ur => new { ur.UserId, ur.RoleId });

            entity.HasOne(ur => ur.User)
                .WithMany(ur => ur.UserRoles)
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<Role>(entity =>
        {
            entity.HasKey(r => r.Id);
            entity.Property(r => r.Id).ValueGeneratedOnAdd();
            entity.Property(u => u.RoleName).HasMaxLength(50);
        });

        #endregion


        #region Client

        builder.Entity<Client>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.Property(u => u.IsActive).HasDefaultValue(true);

            entity.HasOne(c => c.Company)
              .WithMany(c => c.Clients)
              .HasForeignKey(c => c.CompanyId)
              .OnDelete(DeleteBehavior.Restrict);

        });

        builder.Entity<Product>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Name).IsRequired().HasMaxLength(200);
            entity.Property(u => u.Description).IsRequired();
            entity.Property(u => u.IsPublic).IsRequired();
            entity.Property(u => u.ProductType).IsRequired().HasMaxLength(200);
            entity.Property(u => u.ImagePath).IsRequired();
        });

        builder.Entity<Company>(entity =>
        {
            entity.HasKey(u => u.Id);

            entity.Property(u => u.IsNational);
            entity.Property(u => u.IsPublic).HasDefaultValue(false);
            entity.Property(u => u.CompanyName);

            entity.Property(u => u.Voen).HasMaxLength(100);
            entity.Property(u => u.LegalAddress);
            entity.Property(u => u.LegalForm);
            entity.Property(u => u.LegalRepresentative);

            entity.Property(u => u.LogoImagePath);

            entity.HasMany(c => c.Clients)
                  .WithOne(c => c.Company)
                  .HasForeignKey(c => c.CompanyId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(c => c.CompanyTypeOfActivities)
                  .WithOne(c => c.Company)
                  .HasForeignKey(c => c.CompanyId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(c => c.CompanyProducts)
                  .WithOne(c => c.Company)
                  .HasForeignKey(c => c.CompanyId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(c => c.Projects)
                  .WithOne(c => c.Company)
                  .HasForeignKey(c => c.CompanyId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(u => u.Voen).IsUnique();
        });

        builder.Entity<TypeOfActivity>(entity =>
        {
            entity.HasKey(op => op.Id);
            entity.Property(op => op.Id).ValueGeneratedOnAdd();
            entity.Property(c => c.Title).IsRequired().HasMaxLength(200);

            entity.HasIndex(u => u.Title).IsUnique();
        });

        builder.Entity<CompanyTypeOfActivity>(entity =>
        {
            entity.HasKey(cc => new { cc.TypeOfActivityId, cc.CompanyId });

            entity.HasOne(cc => cc.Company)
                .WithMany(cc => cc.CompanyTypeOfActivities)
                .HasForeignKey(cc => cc.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(cc => cc.TypeOfActivity)
                .WithMany(cc => cc.CompanyTypeOfActivities)
                .HasForeignKey(cc => cc.TypeOfActivityId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<CompanyProduct>(entity =>
        {
            entity.HasKey(cc => new { cc.ProductId, cc.CompanyId });

            entity.HasOne(cc => cc.Company)
                .WithMany(cc => cc.CompanyProducts)
                .HasForeignKey(cc => cc.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(cc => cc.Product)
                .WithMany(cc => cc.CompanyProducts)
                .HasForeignKey(cc => cc.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        #endregion


        #region Bitrix

        builder.Entity<Comment>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Text).IsRequired();
            entity.Property(c => c.CreatedAt).IsRequired();
            entity.Property(c => c.UserId).IsRequired();
            entity.Property(c => c.ProjectTaskId).IsRequired();

            entity.HasOne(c => c.User)
                  .WithMany(c => c.Comments)
                  .HasForeignKey(c => c.UserId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(c => c.ProjectTask)
                  .WithMany(c => c.Comments)
                  .HasForeignKey(c => c.ProjectTaskId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(c => c.Files)
                  .WithOne(c => c.Comment)
                  .HasForeignKey(cf => cf.CommentId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(c => c.Reactions)
                  .WithOne(c => c.Comment)
                  .HasForeignKey(r => r.CommentId)
                  .OnDelete(DeleteBehavior.Cascade);
        });


        builder.Entity<Reaction>(entity =>
        {
            entity.HasOne(r => r.Comment)
           .WithMany(c => c.Reactions)
           .HasForeignKey(r => r.CommentId);
        });

        builder.Entity<Mention>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Comment)
                .WithMany(c => c.Mentions)
                .HasForeignKey(e => e.CommentId)
                .OnDelete(DeleteBehavior.Cascade);
        });


        builder.Entity<Project>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Name).IsRequired().HasMaxLength(200);
            entity.Property(u => u.CreatedDate).IsRequired();
            entity.Property(u => u.FinishDate).IsRequired();
            entity.Property(u => u.Description).IsRequired();
            entity.Property(u => u.IsCompleted).HasDefaultValue(false);
            entity.Property(u => u.IsPublic).HasDefaultValue(false);
            entity.Property(u => u.ProjectType).HasConversion(
                    v => v.ToString(),
                    v => (ProjectType)Enum.Parse(typeof(ProjectType), v));

            entity.Property(u => u.DesignThemeImagePath).IsRequired();
            entity.Property(u => u.ProjectAvatarImagePath).IsRequired();



            entity.HasOne(u => u.ProjectManager)
                  .WithMany(pm => pm.MyManageProjects)
                  .HasForeignKey(u => u.ProjectManagerId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(u => u.Product)
                  .WithMany(p => p.Projects)
                  .HasForeignKey(u => u.ProductId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(u => u.Company)
                  .WithMany(p => p.Projects)
                  .HasForeignKey(u => u.CompanyId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(p => p.ProjectUsers)
                  .WithOne(p => p.Project)
                  .HasForeignKey(p => p.ProjectId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(u => u.Name).IsUnique();
        });

        builder.Entity<TaskRole>(entity =>
        {
            entity.HasKey(pr => pr.Id);

            entity.Property(u => u.ProjectUserRole).HasConversion(
                    v => v.ToString(),
                    v => (ProjectUserRole)Enum.Parse(typeof(ProjectUserRole), v));
            entity.Property(u => u.UserId).IsRequired();

            entity.HasOne(p => p.ProjectTask)
                .WithMany()
                .HasForeignKey(e => e.ProjectTaskId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<ProjectSubTiket>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Title).IsRequired().HasMaxLength(100);
            entity.Property(u => u.CreatedTime).IsRequired();
            entity.Property(u => u.Deadline).IsRequired();

            entity.HasOne(p => p.ProjectTiket)
                .WithMany(p => p.ProjectSubTikets)
                .HasForeignKey(e => e.ProjectTiketId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<ProjectSubTiketUser>(entity =>
        {
            entity.HasKey(pr => pr.Id);

            entity.HasOne(p => p.ProjectSubTiket)
                .WithMany(p => p.ProjectSubTiketUsers)
                .HasForeignKey(e => e.ProjectSubTiketId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(p => p.ProjectTiketUser)
                .WithMany()
                .HasForeignKey(e => e.ProjectTiketUserId)
                .OnDelete(DeleteBehavior.Restrict);
        });


        builder.Entity<ProjectTask>(entity =>
        {
            entity.HasKey(pt => pt.Id);

            entity.Property(pt => pt.Title)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(pt => pt.Description)
                .IsRequired();

            entity.Property(pt => pt.IsVeryImportant)
                .HasDefaultValue(false);

            entity.Property(pt => pt.IsExpired)
                .HasDefaultValue(false);

            entity.Property(pt => pt.StartDate)
                .IsRequired();

            entity.Property(pt => pt.DeadLine)
                .IsRequired();

            entity.Property(pt => pt.DoneState)
                .HasConversion(
                    v => v.ToString(),
                    v => (DoneState)Enum.Parse(typeof(DoneState), v));


            entity.HasOne(pt => pt.Project)
                .WithMany(p => p.ProjectTasks)
                .HasForeignKey(pt => pt.ProjectId)
                .OnDelete(DeleteBehavior.Restrict);


            entity.HasOne(pt => pt.ProjectTicket)
                .WithMany(pt => pt.ProjectTasks)
                .HasForeignKey(pt => pt.ProjectTicketId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            entity.HasOne(pt => pt.ProjectSubTicket)
                .WithMany(pst => pst.ProjectTasks)
                .HasForeignKey(pt => pt.ProjectSubTicketId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            entity.HasOne(pt => pt.ProjectTaskCheckList)
                .WithOne(cl => cl.ProjectTask)
                .HasForeignKey<ProjectTaskCheckList>(cl => cl.ProjectTaskId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(pt => pt.Comments)
                .WithOne(c => c.ProjectTask)
                .HasForeignKey(c => c.ProjectTaskId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(pt => pt.ProjectTicketTaskUsers)
                .WithOne()
                .HasForeignKey("ProjectTaskId")
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(pt => pt.ProjectSubTicketTaskUsers)
                .WithOne()
                .HasForeignKey("ProjectTaskId")
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<ProjectTicketTaskUser>(entity =>
        {
            entity.HasKey(pr => pr.Id);
            entity.Property(u => u.ProjectUserRole).HasConversion(
                    v => v.ToString(),
                    v => (ProjectUserRole)Enum.Parse(typeof(ProjectUserRole), v));


            entity.HasOne(p => p.ProjectTask)
                .WithMany(p => p.ProjectTicketTaskUsers)
                .HasForeignKey(e => e.ProjectTaskId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(p => p.ProjectTiketUser)
                .WithMany(p => p.ProjectTicketTaskUsers)
                .HasForeignKey(e => e.ProjectTiketUserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<ProjectSubTicketTaskUser>(entity =>
        {
            entity.HasKey(pr => pr.Id);
            entity.Property(u => u.ProjectUserRole).HasConversion(
                    v => v.ToString(),
                    v => (ProjectUserRole)Enum.Parse(typeof(ProjectUserRole), v));


            entity.HasOne(p => p.ProjectTask)
                .WithMany(p => p.ProjectSubTicketTaskUsers)
                .HasForeignKey(e => e.ProjectTaskId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(p => p.ProjectSubTiketUser)
                .WithMany(p => p.ProjectSubTicketTaskUsers)
                .HasForeignKey(e => e.ProjectSubTiketUserId)
                .OnDelete(DeleteBehavior.Restrict);
        });


        builder.Entity<ProjectTiket>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Title).IsRequired().HasMaxLength(200);
            entity.Property(u => u.CreatedTime).IsRequired();
            entity.Property(u => u.Deadline).IsRequired();
            entity.Property(u => u.HasSubTiket).HasDefaultValue(false);


            entity.HasOne(p => p.Project)
                .WithMany(p => p.ProjectTikets)
                .HasForeignKey(e => e.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(p => p.ProjectTiketUsers)
                .WithOne(p => p.ProjectTiket)
                .HasForeignKey(p => p.ProjectTiketId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<ProjectTiketUser>(entity =>
        {
            entity.HasKey(p => p.Id);

            entity.HasOne(p => p.ProjectTiket)
                .WithMany(p => p.ProjectTiketUsers)
                .HasForeignKey(e => e.ProjectTiketId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<ProjectUser>(entity =>
        {
            entity.HasKey(pr => pr.Id);

            entity.HasOne(p => p.Project)
                .WithMany(p => p.ProjectUsers)
                .HasForeignKey(e => e.ProjectId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(p => p.Staff)
                .WithMany(c => c.ProjectUsers)
                .HasForeignKey(e => e.StaffId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(p => p.Client)
                .WithMany(c => c.ProjectUsers)
                .HasForeignKey(e => e.ClientId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<ProjectTaskCheckList>(entity =>
        {
            entity.HasKey(c => c.Id);

            entity.Property(c => c.Title)
                .IsRequired()
                .HasMaxLength(255);

            entity.HasOne(c => c.ProjectTask)
                .WithOne(p => p.ProjectTaskCheckList)
                .HasForeignKey<ProjectTaskCheckList>(c => c.ProjectTaskId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(c => c.ProjectTaskCheckItems)
                .WithOne(i => i.CheckList)
                .HasForeignKey(i => i.CheckListId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<ProjectTaskCheckItem>(entity =>
        {
            entity.HasKey(i => i.Id);

            entity.Property(i => i.Name)
                .IsRequired()
                .HasMaxLength(255);

            entity.HasMany(i => i.ProjectTaskCheckItemFiles)
                .WithOne(f => f.ProjectTaskCheckItem)
                .HasForeignKey(f => f.ProjectTaskCheckItemId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<ProjectTaskCheckItemFile>(entity =>
        {
            entity.HasKey(f => f.Id);

            entity.Property(f => f.FileName)
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(f => f.FileExtension)
                .IsRequired()
                .HasMaxLength(10);

            entity.Property(f => f.FileData)
                .IsRequired();
        });

        #endregion


        #region Staff

        builder.Entity<Certificate>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Name).IsRequired().HasMaxLength(160);
            entity.Property(u => u.Authority).IsRequired().HasMaxLength(160);

            entity.Property(u => u.GivenTime).IsRequired();
            entity.Property(u => u.Deadline).IsRequired();

            entity.Property(u => u.CertificateFilePath).IsRequired(false);


            entity.HasOne(s => s.Staff)
                .WithMany(s => s.Certificates)
                .HasForeignKey(s => s.StaffId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<StaffImage>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.Property(u => u.ImagePath);

            entity.HasOne(u => u.Staff)
                .WithMany(s => s.StaffImages)
                .HasForeignKey(e => e.StaffId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<Staff>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.Property(u => u.IsDismissed).HasDefaultValue(false);
            entity.Property(u => u.Experience);


            entity.HasMany(s => s.StaffImages)
                .WithOne(s => s.Staff)
                .HasForeignKey(s => s.StaffId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(s => s.Certificates)
                .WithOne(s => s.Staff)
                .HasForeignKey(s => s.StaffId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(s => s.MyManageProjects)
                .WithOne(s => s.ProjectManager)
                .HasForeignKey(s => s.ProjectManagerId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        #endregion


        base.OnModelCreating(builder);
    }
}