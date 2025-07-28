using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SAPLSServer.Models;

public partial class SaplsContext : DbContext
{
    public SaplsContext()
    {
    }

    public SaplsContext(DbContextOptions<SaplsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AdminProfile> AdminProfiles { get; set; }

    public virtual DbSet<AttachedFile> AttachedFiles { get; set; }

    public virtual DbSet<ClientProfile> ClientProfiles { get; set; }

    public virtual DbSet<IncidenceEvidence> IncidenceEvidences { get; set; }

    public virtual DbSet<IncidenceReport> IncidenceReports { get; set; }

    public virtual DbSet<ParkingFeeSchedule> ParkingFeeSchedules { get; set; }

    public virtual DbSet<ParkingLot> ParkingLots { get; set; }

    public virtual DbSet<ParkingLotOwnerProfile> ParkingLotOwnerProfiles { get; set; }

    public virtual DbSet<ParkingSession> ParkingSessions { get; set; }

    public virtual DbSet<PaymentSource> PaymentSources { get; set; }

    public virtual DbSet<Request> Requests { get; set; }

    public virtual DbSet<RequestAttachedFile> RequestAttachedFiles { get; set; }

    public virtual DbSet<SharedVehicle> SharedVehicles { get; set; }

    public virtual DbSet<ShiftDiary> ShiftDiaries { get; set; }

    public virtual DbSet<StaffProfile> StaffProfiles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Vehicle> Vehicles { get; set; }

    public virtual DbSet<WhiteList> WhiteLists { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("server=ANEMOS\\SQLEXPRESS;database=SAPLS;uid=sa;pwd=sa;TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AdminProfile>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__AdminPro__1788CC4C56347231");

            entity.ToTable("AdminProfile");

            entity.HasIndex(e => e.AdminId, "IX_AdminProfile_AdminId");

            entity.HasIndex(e => e.Role, "IX_AdminProfile_Role");

            entity.HasIndex(e => e.UserId, "UQ__AdminPro__1788CC4DC1E113D5").IsUnique();

            entity.HasIndex(e => e.AdminId, "UQ__AdminPro__719FE48988C1CBFA").IsUnique();

            entity.Property(e => e.UserId).HasMaxLength(36);
            entity.Property(e => e.AdminId).HasMaxLength(20);
            entity.Property(e => e.Role).HasMaxLength(20);

            entity.HasOne(d => d.User).WithOne(p => p.AdminProfile)
                .HasForeignKey<AdminProfile>(d => d.UserId)
                .HasConstraintName("AdminProfile_fk0");
        });

        modelBuilder.Entity<AttachedFile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Attached__3214EC0773420A23");

            entity.ToTable("AttachedFile");

            entity.HasIndex(e => e.Name, "IX_AttachedFile_Name");

            entity.HasIndex(e => e.UploadAt, "IX_AttachedFile_UploadAt");

            entity.HasIndex(e => e.Id, "UQ__Attached__3214EC06B1509C9B").IsUnique();

            entity.Property(e => e.Id).HasMaxLength(36);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.UploadAt).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<ClientProfile>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__ClientPr__1788CC4C07B7BA9F");

            entity.ToTable("ClientProfile");

            entity.HasIndex(e => e.CitizenId, "IX_ClientProfile_CitizenId");

            entity.HasIndex(e => e.ShareCode, "IX_ClientProfile_ShareCode");

            entity.HasIndex(e => e.UserId, "UQ__ClientPr__1788CC4D35AAEF69").IsUnique();

            entity.HasIndex(e => e.ShareCode, "UQ__ClientPr__208770413BAB0902").IsUnique();

            entity.HasIndex(e => e.CitizenId, "UQ__ClientPr__6E49FA0DA6B0F1C5").IsUnique();

            entity.Property(e => e.UserId).HasMaxLength(36);
            entity.Property(e => e.CitizenId).HasMaxLength(50);
            entity.Property(e => e.CitizenIdCardImageUrl).HasMaxLength(500);
            entity.Property(e => e.Nationality).HasMaxLength(100);
            entity.Property(e => e.PlaceOfOrigin).HasMaxLength(255);
            entity.Property(e => e.PlaceOfResidence).HasMaxLength(255);
            entity.Property(e => e.Sex).HasDefaultValue(true);
            entity.Property(e => e.ShareCode).HasMaxLength(20);

            entity.HasOne(d => d.User).WithOne(p => p.ClientProfile)
                .HasForeignKey<ClientProfile>(d => d.UserId)
                .HasConstraintName("ClientProfile_fk0");
        });

        modelBuilder.Entity<IncidenceEvidence>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Incidenc__3214EC0725A8A24C");

            entity.ToTable("IncidenceEvidence");

            entity.HasIndex(e => e.Id, "UQ__Incidenc__3214EC06A3702B07").IsUnique();

            entity.Property(e => e.Id).HasMaxLength(36);
            entity.Property(e => e.IncidenceReportId).HasMaxLength(36);

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.IncidenceEvidence)
                .HasForeignKey<IncidenceEvidence>(d => d.Id)
                .HasConstraintName("IncidenceEvidence_fk0");

            entity.HasOne(d => d.IncidenceReport).WithMany(p => p.IncidenceEvidences)
                .HasForeignKey(d => d.IncidenceReportId)
                .HasConstraintName("IncidenceEvidence_fk1");
        });

        modelBuilder.Entity<IncidenceReport>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Incidenc__3214EC07D412DE08");

            entity.ToTable("IncidenceReport");

            entity.HasIndex(e => e.Priority, "IX_IncidenceReport_Priority");

            entity.HasIndex(e => e.ReportedDate, "IX_IncidenceReport_ReportedDate");

            entity.HasIndex(e => e.ReporterId, "IX_IncidenceReport_ReporterId");

            entity.HasIndex(e => e.Status, "IX_IncidenceReport_Status");

            entity.HasIndex(e => e.Id, "UQ__Incidenc__3214EC06DE213DDD").IsUnique();

            entity.Property(e => e.Id).HasMaxLength(36);
            entity.Property(e => e.Description).HasMaxLength(2000);
            entity.Property(e => e.Header).HasMaxLength(255);
            entity.Property(e => e.ParkingLotId).HasMaxLength(36);
            entity.Property(e => e.Priority)
                .HasMaxLength(20)
                .HasDefaultValue("Medium");
            entity.Property(e => e.ReportedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.ReporterId).HasMaxLength(36);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Open");

            entity.HasOne(d => d.ParkingLot).WithMany(p => p.IncidenceReports)
                .HasForeignKey(d => d.ParkingLotId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_IncidenceReport_ParkingLot");

            entity.HasOne(d => d.Reporter).WithMany(p => p.IncidenceReports)
                .HasForeignKey(d => d.ReporterId)
                .HasConstraintName("IncidenceReport_fk6");
        });

        modelBuilder.Entity<ParkingFeeSchedule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ParkingF__3214EC072533CD44");

            entity.ToTable("ParkingFeeSchedule");

            entity.HasIndex(e => e.ForVehicleType, "IX_ParkingFeeSchedule_ForVehicleType");

            entity.HasIndex(e => e.IsActive, "IX_ParkingFeeSchedule_IsActive");

            entity.HasIndex(e => e.ParkingLotId, "IX_ParkingFeeSchedule_ParkingLotId");

            entity.HasIndex(e => e.Id, "UQ__ParkingF__3214EC067ED1ED17").IsUnique();

            entity.Property(e => e.Id).HasMaxLength(36);
            entity.Property(e => e.AdditionalFee).HasColumnType("decimal(8, 2)");
            entity.Property(e => e.AdditionalMinutes).HasDefaultValue(60);
            entity.Property(e => e.DayOfWeeks)
                .HasMaxLength(20)
                .HasDefaultValueSql("(NULL)");
            entity.Property(e => e.ForVehicleType)
                .HasMaxLength(20)
                .HasDefaultValue("Car");
            entity.Property(e => e.InitialFee).HasColumnType("decimal(8, 2)");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.ParkingLotId).HasMaxLength(36);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.ParkingLot).WithMany(p => p.ParkingFeeSchedules)
                .HasForeignKey(d => d.ParkingLotId)
                .HasConstraintName("ParkingFeeSchedule_fk10");
        });

        modelBuilder.Entity<ParkingLot>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ParkingL__3214EC07D98A3F04");

            entity.ToTable("ParkingLot");

            entity.HasIndex(e => e.CreatedAt, "IX_ParkingLot_CreatedAt");

            entity.HasIndex(e => e.ParkingLotOwnerId, "IX_ParkingLot_ParkingLotOwnerId");

            entity.HasIndex(e => e.Status, "IX_ParkingLot_Status");

            entity.HasIndex(e => e.Id, "UQ__ParkingL__3214EC066134085F").IsUnique();

            entity.Property(e => e.Id).HasMaxLength(36);
            entity.Property(e => e.Address).HasMaxLength(500);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Description)
                .HasMaxLength(1000)
                .HasDefaultValueSql("(NULL)");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.ParkingLotOwnerId).HasMaxLength(36);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Active");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.ParkingLotOwner).WithMany(p => p.ParkingLots)
                .HasForeignKey(d => d.ParkingLotOwnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ParkingLot_fk8");
        });

        modelBuilder.Entity<ParkingLotOwnerProfile>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__ParkingL__1788CC4C461C8DDB");

            entity.ToTable("ParkingLotOwnerProfile");

            entity.HasIndex(e => e.ParkingLotOwnerId, "IX_ParkingLotOwnerProfile_ParkingLotOwnerId");

            entity.HasIndex(e => e.UserId, "UQ__ParkingL__1788CC4D2C170C27").IsUnique();

            entity.HasIndex(e => e.ParkingLotOwnerId, "UQ__ParkingL__F2333F3FA5015388").IsUnique();

            entity.Property(e => e.UserId).HasMaxLength(36);
            entity.Property(e => e.ParkingLotOwnerId).HasMaxLength(20);

            entity.HasOne(d => d.User).WithOne(p => p.ParkingLotOwnerProfile)
                .HasForeignKey<ParkingLotOwnerProfile>(d => d.UserId)
                .HasConstraintName("ParkingLotOwnerProfile_fk0");
        });

        modelBuilder.Entity<ParkingSession>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ParkingS__3214EC076244C9DD");

            entity.ToTable("ParkingSession");

            entity.HasIndex(e => e.EntryDateTime, "IX_ParkingSession_EntryDateTime");

            entity.HasIndex(e => e.ExitDateTime, "IX_ParkingSession_ExitDateTime");

            entity.HasIndex(e => e.ParkingLotId, "IX_ParkingSession_ParkingLotId");

            entity.HasIndex(e => e.PaymentMethod, "IX_ParkingSession_PaymentMethod");

            entity.HasIndex(e => e.VehicleId, "IX_ParkingSession_VehicleId");

            entity.HasIndex(e => e.Id, "UQ__ParkingS__3214EC06ED8B4805").IsUnique();

            entity.HasIndex(e => e.TransactionId, "UQ__ParkingS__55433A6AF76245F4").IsUnique();

            entity.Property(e => e.Id).HasMaxLength(36);
            entity.Property(e => e.CheckOutDateTime).HasDefaultValueSql("(NULL)");
            entity.Property(e => e.Cost).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.EntryBackCaptureUrl).HasMaxLength(500);
            entity.Property(e => e.EntryDateTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.EntryFrontCaptureUrl).HasMaxLength(500);
            entity.Property(e => e.ExitBackCaptureUrl)
                .HasMaxLength(500)
                .HasDefaultValueSql("(NULL)");
            entity.Property(e => e.ExitDateTime).HasDefaultValueSql("(NULL)");
            entity.Property(e => e.ExitFrontCaptureUrl)
                .HasMaxLength(500)
                .HasDefaultValueSql("(NULL)");
            entity.Property(e => e.ParkingLotId).HasMaxLength(36);
            entity.Property(e => e.PaymentMethod)
                .HasMaxLength(20)
                .HasDefaultValue("Cash");
            entity.Property(e => e.PaymentStatus)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TransactionId)
                .HasMaxLength(36)
                .HasDefaultValueSql("(NULL)");
            entity.Property(e => e.VehicleId).HasMaxLength(36);

            entity.HasOne(d => d.ParkingLot).WithMany(p => p.ParkingSessions)
                .HasForeignKey(d => d.ParkingLotId)
                .HasConstraintName("ParkingSession_fk2");

            entity.HasOne(d => d.Vehicle).WithMany(p => p.ParkingSessions)
                .HasForeignKey(d => d.VehicleId)
                .HasConstraintName("ParkingSession_fk1");
        });

        modelBuilder.Entity<PaymentSource>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PaymentS__3214EC074DD2D6E5");

            entity.ToTable("PaymentSource");

            entity.HasIndex(e => e.ParkingLotOwnerId, "IX_PaymentSource_ParkingLotOwnerId");

            entity.HasIndex(e => e.Id, "UQ__PaymentS__3214EC064155607D").IsUnique();

            entity.Property(e => e.AccountName).HasMaxLength(255);
            entity.Property(e => e.AccountNumber).HasMaxLength(50);
            entity.Property(e => e.BankName).HasMaxLength(100);
            entity.Property(e => e.ParkingLotOwnerId).HasMaxLength(36);
            entity.Property(e => e.Status).HasMaxLength(25);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.ParkingLotOwner).WithMany(p => p.PaymentSources)
                .HasForeignKey(d => d.ParkingLotOwnerId)
                .HasConstraintName("PaymentSource_fk5");
        });

        modelBuilder.Entity<Request>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Request__3214EC07B2ACCA82");

            entity.ToTable("Request");

            entity.HasIndex(e => e.LastUpdatePersonId, "IX_Request_LastUpdatePersonId");

            entity.HasIndex(e => e.SenderId, "IX_Request_SenderId");

            entity.HasIndex(e => e.Status, "IX_Request_Status");

            entity.HasIndex(e => e.SubmittedAt, "IX_Request_SubmittedAt");

            entity.HasIndex(e => e.Id, "UQ__Request__3214EC06DDE85BED").IsUnique();

            entity.Property(e => e.Id).HasMaxLength(36);
            entity.Property(e => e.Description).HasMaxLength(2000);
            entity.Property(e => e.Header).HasMaxLength(255);
            entity.Property(e => e.InternalNote)
                .HasMaxLength(1000)
                .HasDefaultValueSql("(NULL)");
            entity.Property(e => e.LastUpdatePersonId).HasMaxLength(36);
            entity.Property(e => e.ResponseMessage)
                .HasMaxLength(2000)
                .HasDefaultValueSql("(NULL)");
            entity.Property(e => e.SenderId).HasMaxLength(36);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Open");
            entity.Property(e => e.SubmittedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Type).HasMaxLength(50);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.LastUpdatePerson).WithMany(p => p.Requests)
                .HasForeignKey(d => d.LastUpdatePersonId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("Request_fk8");

            entity.HasOne(d => d.Sender).WithMany(p => p.Requests)
                .HasForeignKey(d => d.SenderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Request_fk_Sender");
        });

        modelBuilder.Entity<RequestAttachedFile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RequestA__3214EC074C99AEB9");

            entity.ToTable("RequestAttachedFile");

            entity.HasIndex(e => e.Id, "UQ__RequestA__3214EC06C2C43B6C").IsUnique();

            entity.Property(e => e.Id).HasMaxLength(36);
            entity.Property(e => e.RequestId).HasMaxLength(36);

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.RequestAttachedFile)
                .HasForeignKey<RequestAttachedFile>(d => d.Id)
                .HasConstraintName("RequestAttachedFile_fk0");

            entity.HasOne(d => d.Request).WithMany(p => p.RequestAttachedFiles)
                .HasForeignKey(d => d.RequestId)
                .HasConstraintName("RequestAttachedFile_fk1");
        });

        modelBuilder.Entity<SharedVehicle>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SharedVe__3214EC07EDA49C12");

            entity.ToTable("SharedVehicle");

            entity.HasIndex(e => e.AcceptAt, "IX_SharedVehicle_AcceptAt");

            entity.HasIndex(e => e.ExpireAt, "IX_SharedVehicle_ExpireAt");

            entity.HasIndex(e => e.InviteAt, "IX_SharedVehicle_InviteAt");

            entity.HasIndex(e => e.SharedPersonId, "IX_SharedVehicle_SharedPersonId");

            entity.HasIndex(e => e.VehicleId, "IX_SharedVehicle_VehicleId");

            entity.HasIndex(e => e.Id, "UQ__SharedVe__3214EC065D8B45A9").IsUnique();

            entity.Property(e => e.Id).HasMaxLength(36);
            entity.Property(e => e.Note).HasMaxLength(500);
            entity.Property(e => e.SharedPersonId).HasMaxLength(36);
            entity.Property(e => e.VehicleId).HasMaxLength(36);

            entity.HasOne(d => d.SharedPerson).WithMany(p => p.SharedVehicles)
                .HasForeignKey(d => d.SharedPersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SharedVehicle_fk7");

            entity.HasOne(d => d.Vehicle).WithMany(p => p.SharedVehicles)
                .HasForeignKey(d => d.VehicleId)
                .HasConstraintName("SharedVehicle_fk1");
        });

        modelBuilder.Entity<ShiftDiary>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ShiftDia__3214EC0741C06B6B");

            entity.ToTable("ShiftDiary");

            entity.HasIndex(e => e.CreatedAt, "IX_ShiftDiary_CreatedAt");

            entity.HasIndex(e => e.ParkingLotId, "IX_ShiftDiary_ParkingLotId");

            entity.HasIndex(e => e.SenderId, "IX_ShiftDiary_SenderId");

            entity.HasIndex(e => e.Id, "UQ__ShiftDia__3214EC06394E68B1").IsUnique();

            entity.Property(e => e.Id).HasMaxLength(36);
            entity.Property(e => e.Body).HasMaxLength(2000);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Header).HasMaxLength(255);
            entity.Property(e => e.ParkingLotId).HasMaxLength(36);
            entity.Property(e => e.SenderId).HasMaxLength(36);

            entity.HasOne(d => d.ParkingLot).WithMany(p => p.ShiftDiaries)
                .HasForeignKey(d => d.ParkingLotId)
                .HasConstraintName("ShiftDiary_fk4");

            entity.HasOne(d => d.Sender).WithMany(p => p.ShiftDiaries)
                .HasForeignKey(d => d.SenderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ShiftDiary_fk5");
        });

        modelBuilder.Entity<StaffProfile>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__StaffPro__1788CC4CAFCD2EA0");

            entity.ToTable("StaffProfile");

            entity.HasIndex(e => e.ParkingLotId, "IX_StaffProfile_ParkingLotId");

            entity.HasIndex(e => e.StaffId, "IX_StaffProfile_StaffId");

            entity.HasIndex(e => e.UserId, "UQ__StaffPro__1788CC4DEBE8406F").IsUnique();

            entity.HasIndex(e => e.StaffId, "UQ__StaffPro__96D4AB1667A0D540").IsUnique();

            entity.Property(e => e.UserId).HasMaxLength(36);
            entity.Property(e => e.ParkingLotId).HasMaxLength(36);
            entity.Property(e => e.StaffId).HasMaxLength(20);

            entity.HasOne(d => d.ParkingLot).WithMany(p => p.StaffProfiles)
                .HasForeignKey(d => d.ParkingLotId)
                .HasConstraintName("StaffProfile_fk2");

            entity.HasOne(d => d.User).WithOne(p => p.StaffProfile)
                .HasForeignKey<StaffProfile>(d => d.UserId)
                .HasConstraintName("StaffProfile_fk0");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User__3214EC0786524D8F");

            entity.ToTable("User");

            entity.HasIndex(e => e.CreatedAt, "IX_User_CreatedAt");

            entity.HasIndex(e => e.Email, "IX_User_Email");

            entity.HasIndex(e => e.Status, "IX_User_Status");

            entity.HasIndex(e => e.Id, "UQ__User__3214EC06366C8D89").IsUnique();

            entity.HasIndex(e => e.Phone, "UQ__User__5C7E359E4EC59181").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__User__A9D1053456547DAC").IsUnique();

            entity.Property(e => e.Id).HasMaxLength(36);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.FullName).HasMaxLength(255);
            entity.Property(e => e.OneTimePassword).HasMaxLength(24);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.ProfileImageUrl)
                .HasMaxLength(500)
                .HasDefaultValueSql("(NULL)");
            entity.Property(e => e.Role).HasMaxLength(50);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Inactive");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<Vehicle>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Vehicle__3214EC070791C79B");

            entity.ToTable("Vehicle");

            entity.HasIndex(e => e.CreatedAt, "IX_Vehicle_CreatedAt");

            entity.HasIndex(e => e.LicensePlate, "IX_Vehicle_LicensePlate");

            entity.HasIndex(e => e.OwnerId, "IX_Vehicle_OwnerId");

            entity.HasIndex(e => e.SharingStatus, "IX_Vehicle_SharingStatus");

            entity.HasIndex(e => e.Status, "IX_Vehicle_Status");

            entity.HasIndex(e => e.LicensePlate, "UQ__Vehicle__026BC15C32994F63").IsUnique();

            entity.HasIndex(e => e.Id, "UQ__Vehicle__3214EC06B23BA738").IsUnique();

            entity.Property(e => e.Id).HasMaxLength(36);
            entity.Property(e => e.Brand).HasMaxLength(50);
            entity.Property(e => e.ChassisNumber).HasMaxLength(50);
            entity.Property(e => e.Color).HasMaxLength(50);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.EngineNumber).HasMaxLength(50);
            entity.Property(e => e.LicensePlate).HasMaxLength(20);
            entity.Property(e => e.Model).HasMaxLength(50);
            entity.Property(e => e.OwnerId).HasMaxLength(36);
            entity.Property(e => e.OwnerVehicleFullName).HasMaxLength(255);
            entity.Property(e => e.SharingStatus)
                .HasMaxLength(20)
                .HasDefaultValue("Unavailable");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Inactive");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.VehicleRegistrationCertificateUrl).HasMaxLength(500);

            entity.HasOne(d => d.Owner).WithMany(p => p.Vehicles)
                .HasForeignKey(d => d.OwnerId)
                .HasConstraintName("Vehicle_fk12");
        });

        modelBuilder.Entity<WhiteList>(entity =>
        {
            entity.HasKey(e => new { e.ParkingLotId, e.ClientId }).HasName("PK__WhiteLis__3140FF2BC4A11819");

            entity.ToTable("WhiteList");

            entity.HasIndex(e => e.AddedAt, "IX_WhiteList_AddedDate");

            entity.HasIndex(e => e.ClientId, "IX_WhiteList_ClientId");

            entity.HasIndex(e => e.ExpireAt, "IX_WhiteList_ExpiredDate");

            entity.Property(e => e.ParkingLotId).HasMaxLength(36);
            entity.Property(e => e.ClientId).HasMaxLength(36);
            entity.Property(e => e.AddedAt).HasDefaultValueSql("(CONVERT([date],getdate()))");
            entity.Property(e => e.ExpireAt).HasDefaultValueSql("(NULL)");

            entity.HasOne(d => d.Client).WithMany(p => p.WhiteLists)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("WhiteList_fk1");

            entity.HasOne(d => d.ParkingLot).WithMany(p => p.WhiteLists)
                .HasForeignKey(d => d.ParkingLotId)
                .HasConstraintName("WhiteList_fk0");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
