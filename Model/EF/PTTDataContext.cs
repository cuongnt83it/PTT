namespace Model.EF
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class PTTDataContext : DbContext
    {
        public PTTDataContext()
            : base("name=PTTDataContext")
        {
        }

        public virtual DbSet<Builder> Builders { get; set; }
        public virtual DbSet<Business> Businesses { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<CompetiorProduct> CompetiorProducts { get; set; }
        public virtual DbSet<Competitor> Competitors { get; set; }
        public virtual DbSet<Content> Contents { get; set; }
        public virtual DbSet<Contrator> Contrators { get; set; }
        public virtual DbSet<District> Districts { get; set; }
        public virtual DbSet<Feedback> Feedbacks { get; set; }
        public virtual DbSet<FeedbackInfor> FeedbackInfors { get; set; }
        public virtual DbSet<GrantPermission> GrantPermissions { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<GroupUser> GroupUsers { get; set; }
        public virtual DbSet<Information> Information { get; set; }
        public virtual DbSet<InforUser> InforUsers { get; set; }
        public virtual DbSet<MenuType> MenuTypes { get; set; }
        public virtual DbSet<Messege> Messeges { get; set; }
        public virtual DbSet<Permission> Permissions { get; set; }
        public virtual DbSet<Price> Prices { get; set; }
        public virtual DbSet<Process> Processes { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<ProjectCompetitor> ProjectCompetitors { get; set; }
        public virtual DbSet<ProjectProduct> ProjectProducts { get; set; }
        public virtual DbSet<ProjectUser> ProjectUsers { get; set; }
        public virtual DbSet<Resource> Resources { get; set; }
        public virtual DbSet<Supplier> Suppliers { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<V_Project_Builer_Contrator> V_Project_Builer_Contrator { get; set; }
        public virtual DbSet<V_Project_Products> V_Project_Products { get; set; }
        public virtual DbSet<V_Project_Supplier> V_Project_Supplier { get; set; }
        public virtual DbSet<V_Project_Users> V_Project_Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Builder>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Builder>()
                .Property(e => e.CreateBy)
                .IsUnicode(false);

            modelBuilder.Entity<Builder>()
                .Property(e => e.ModifiedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Category>()
                .Property(e => e.CreateBy)
                .IsUnicode(false);

            modelBuilder.Entity<Category>()
                .Property(e => e.ModifiedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Competitor>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Competitor>()
                .Property(e => e.CreateBy)
                .IsUnicode(false);

            modelBuilder.Entity<Competitor>()
                .Property(e => e.ModifiedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Content>()
                .Property(e => e.Image)
                .IsUnicode(false);

            modelBuilder.Entity<Content>()
                .Property(e => e.CreateBy)
                .IsUnicode(false);

            modelBuilder.Entity<Content>()
                .Property(e => e.ModifiedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Contrator>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Contrator>()
                .Property(e => e.CreateBy)
                .IsUnicode(false);

            modelBuilder.Entity<Contrator>()
                .Property(e => e.ModifiedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Feedback>()
                .Property(e => e.CreateBy)
                .IsUnicode(false);

            modelBuilder.Entity<Feedback>()
                .Property(e => e.ModifiedBy)
                .IsUnicode(false);

            modelBuilder.Entity<FeedbackInfor>()
                .Property(e => e.CreateBy)
                .IsUnicode(false);

            modelBuilder.Entity<FeedbackInfor>()
                .Property(e => e.ModifiedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Information>()
                .Property(e => e.CreateBy)
                .IsUnicode(false);

            modelBuilder.Entity<Information>()
                .Property(e => e.ModifiedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Messege>()
                .Property(e => e.CreateBy)
                .IsUnicode(false);

            modelBuilder.Entity<Messege>()
                .Property(e => e.ModifiedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Permission>()
                .Property(e => e.BusinessID)
                .IsUnicode(false);

            modelBuilder.Entity<Price>()
                .Property(e => e.PriceStart)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Price>()
                .Property(e => e.PriceEnd)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Price>()
                .Property(e => e.CreateBy)
                .IsUnicode(false);

            modelBuilder.Entity<Price>()
                .Property(e => e.ModifiedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Process>()
                .Property(e => e.CreateBy)
                .IsUnicode(false);

            modelBuilder.Entity<Process>()
                .Property(e => e.ModifiedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.Price)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Product>()
                .Property(e => e.CreateBy)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.ModifiedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Project>()
                .Property(e => e.Value)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Project>()
                .Property(e => e.CreateBy)
                .IsUnicode(false);

            modelBuilder.Entity<Project>()
                .Property(e => e.ModifiedBy)
                .IsUnicode(false);

            modelBuilder.Entity<ProjectProduct>()
                .Property(e => e.Price)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Resource>()
                .Property(e => e.CreateBy)
                .IsUnicode(false);

            modelBuilder.Entity<Resource>()
                .Property(e => e.ModifiedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Supplier>()
                .Property(e => e.SupplierID)
                .IsUnicode(false);

            modelBuilder.Entity<Supplier>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Supplier>()
                .Property(e => e.CreateBy)
                .IsUnicode(false);

            modelBuilder.Entity<Supplier>()
                .Property(e => e.ModifiedBy)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.UserName)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<V_Project_Builer_Contrator>()
                .Property(e => e.Value)
                .HasPrecision(18, 0);

            modelBuilder.Entity<V_Project_Builer_Contrator>()
                .Property(e => e.CreateBy)
                .IsUnicode(false);

            modelBuilder.Entity<V_Project_Builer_Contrator>()
                .Property(e => e.ModifiedBy)
                .IsUnicode(false);

            modelBuilder.Entity<V_Project_Products>()
                .Property(e => e.Value)
                .HasPrecision(18, 0);

            modelBuilder.Entity<V_Project_Products>()
                .Property(e => e.CreateBy)
                .IsUnicode(false);

            modelBuilder.Entity<V_Project_Products>()
                .Property(e => e.ModifiedBy)
                .IsUnicode(false);

            modelBuilder.Entity<V_Project_Products>()
                .Property(e => e.Price)
                .HasPrecision(18, 0);

            modelBuilder.Entity<V_Project_Supplier>()
                .Property(e => e.Value)
                .HasPrecision(18, 0);

            modelBuilder.Entity<V_Project_Supplier>()
                .Property(e => e.CreateBy)
                .IsUnicode(false);

            modelBuilder.Entity<V_Project_Supplier>()
                .Property(e => e.ModifiedBy)
                .IsUnicode(false);

            modelBuilder.Entity<V_Project_Supplier>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<V_Project_Users>()
                .Property(e => e.Value)
                .HasPrecision(18, 0);

            modelBuilder.Entity<V_Project_Users>()
                .Property(e => e.CreateBy)
                .IsUnicode(false);

            modelBuilder.Entity<V_Project_Users>()
                .Property(e => e.ModifiedBy)
                .IsUnicode(false);

            modelBuilder.Entity<V_Project_Users>()
                .Property(e => e.UserName)
                .IsUnicode(false);

            modelBuilder.Entity<V_Project_Users>()
                .Property(e => e.Email)
                .IsUnicode(false);
        }
    }
}
