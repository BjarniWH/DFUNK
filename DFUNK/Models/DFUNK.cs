namespace DFUNK.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class DFUNK : DbContext
    {
        public DFUNK()
            : base("name=DFUNK")
        {
        }

        public virtual DbSet<CompanyInfo> CompanyInfo { get; set; }
        public virtual DbSet<Contact> Contact { get; set; }
        public virtual DbSet<Events> Events { get; set; }
        public virtual DbSet<Group> Group { get; set; }
        public virtual DbSet<PaymentMethod> PaymentMethod { get; set; }
        public virtual DbSet<Payments> Payments { get; set; }
        public virtual DbSet<Projects> Projects { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<VolunteerInfo> VolunteerInfo { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CompanyInfo>()
                .Property(e => e.contactPerson)
                .IsUnicode(false);

            modelBuilder.Entity<CompanyInfo>()
                .Property(e => e.role)
                .IsUnicode(false);

            modelBuilder.Entity<CompanyInfo>()
                .Property(e => e.email)
                .IsUnicode(false);

            modelBuilder.Entity<CompanyInfo>()
                .Property(e => e.phone)
                .IsFixedLength();

            modelBuilder.Entity<Contact>()
                .Property(e => e.name)
                .IsFixedLength();

            modelBuilder.Entity<Contact>()
                .Property(e => e.surname)
                .IsFixedLength();

            modelBuilder.Entity<Contact>()
                .Property(e => e.street)
                .IsUnicode(false);

            modelBuilder.Entity<Contact>()
                .Property(e => e.city)
                .IsUnicode(false);

            modelBuilder.Entity<Contact>()
                .Property(e => e.phoneNr)
                .IsFixedLength();

            modelBuilder.Entity<Contact>()
                .Property(e => e.email)
                .IsUnicode(false);

            modelBuilder.Entity<Contact>()
                .HasOptional(e => e.CompanyInfo)
                .WithRequired(e => e.Contact);

            modelBuilder.Entity<Contact>()
                .HasMany(e => e.Events)
                .WithOptional(e => e.Contact)
                .HasForeignKey(e => e.leader);

            modelBuilder.Entity<Contact>()
                .HasMany(e => e.Group)
                .WithRequired(e => e.Contact)
                .HasForeignKey(e => e.leader)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Contact>()
                .HasMany(e => e.Payments)
                .WithRequired(e => e.Contact)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Contact>()
                .HasMany(e => e.Projects)
                .WithOptional(e => e.Contact)
                .HasForeignKey(e => e.leader);

            modelBuilder.Entity<Contact>()
                .HasOptional(e => e.VolunteerInfo)
                .WithRequired(e => e.Contact);

            modelBuilder.Entity<Contact>()
                .HasMany(e => e.Group1)
                .WithMany(e => e.Contact1)
                .Map(m => m.ToTable("GroupMember").MapLeftKey("contact_id").MapRightKey("group_id"));

            modelBuilder.Entity<Events>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<Events>()
                .Property(e => e.address)
                .IsUnicode(false);

            //modelBuilder.Entity<Events>()
            //    .Property(e => e.endDate)
            //    .IsFixedLength();

            modelBuilder.Entity<Events>()
                .HasMany(e => e.Contact1)
                .WithMany(e => e.Events1)
                .Map(m => m.ToTable("EventParticipators").MapLeftKey("events_id").MapRightKey("contact_id"));

            modelBuilder.Entity<Events>()
                .HasMany(e => e.Projects)
                .WithMany(e => e.Events)
                .Map(m => m.ToTable("ProjectEvent").MapLeftKey("event_id").MapRightKey("project_id"));

            modelBuilder.Entity<Group>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<Group>()
                .Property(e => e.city)
                .IsUnicode(false);

            modelBuilder.Entity<Group>()
                .Property(e => e.region)
                .IsUnicode(false);

            modelBuilder.Entity<Group>()
                .HasMany(e => e.Projects)
                .WithMany(e => e.Group)
                .Map(m => m.ToTable("ProjectGroup").MapLeftKey("group_id").MapRightKey("project_id"));

            modelBuilder.Entity<PaymentMethod>()
                .Property(e => e.paymentMethod_id)
                .IsFixedLength();

            modelBuilder.Entity<PaymentMethod>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<PaymentMethod>()
                .Property(e => e._ref)
                .IsUnicode(false);

            modelBuilder.Entity<PaymentMethod>()
                .HasMany(e => e.Payments)
                .WithOptional(e => e.PaymentMethod)
                .HasForeignKey(e => e.payment_method);

            modelBuilder.Entity<Payments>()
                .Property(e => e.amount)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Payments>()
                .Property(e => e.payment_method)
                .IsFixedLength();

            modelBuilder.Entity<Projects>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<Projects>()
                .Property(e => e.budget)
                .IsUnicode(false);

            modelBuilder.Entity<Projects>()
                .Property(e => e.contactNr)
                .IsUnicode(false);

            modelBuilder.Entity<Projects>()
                .Property(e => e.contactEmail)
                .IsUnicode(false);

            modelBuilder.Entity<VolunteerInfo>()
                .Property(e => e.tshirtSize)
                .IsFixedLength();

            modelBuilder.Entity<VolunteerInfo>()
                .Property(e => e.drivingLicense)
                .IsFixedLength();
        }
    }
}
