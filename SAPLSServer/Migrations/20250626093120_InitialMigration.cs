using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SAPLSServer.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AttachedFile",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UploadAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Attached__3214EC073061CFA9", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ProfileImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, defaultValueSql: "(NULL)"),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Inactive"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__User__3214EC07C4FE8F5D", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AdminProfile",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    AdminId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Admin")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__AdminPro__1788CC4C7DA9A2EB", x => x.UserId);
                    table.ForeignKey(
                        name: "AdminProfile_fk0",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClientProfile",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    CitizenId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: false),
                    Sex = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    Nationality = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PlaceOfOrigin = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PlaceOfResidence = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ShareCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CitizenIdCardImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ClientPr__1788CC4C8C0013CF", x => x.UserId);
                    table.ForeignKey(
                        name: "ClientProfile_fk0",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ParkingLotOwnerProfile",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    ParkingLotOwnerId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ParkingL__1788CC4CF028C9F5", x => x.UserId);
                    table.ForeignKey(
                        name: "ParkingLotOwnerProfile_fk0",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Request",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    Header = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Open"),
                    SubmittedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())"),
                    InternalNote = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true, defaultValueSql: "(NULL)"),
                    ResponseMessage = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true, defaultValueSql: "(NULL)"),
                    LastUpdatePersonId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Request__3214EC07B2ACCA82", x => x.Id);
                    table.ForeignKey(
                        name: "Request_fk8",
                        column: x => x.LastUpdatePersonId,
                        principalTable: "AdminProfile",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Vehicle",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    LicensePlate = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Brand = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Model = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EngineNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ChassisNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Color = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    OwnerVehicleFullName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Inactive"),
                    SharingStatus = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Unavailable"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())"),
                    OwnerId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    VehicleRegistrationCertificateUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Vehicle__3214EC07EBABF790", x => x.Id);
                    table.ForeignKey(
                        name: "Vehicle_fk12",
                        column: x => x.OwnerId,
                        principalTable: "ClientProfile",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ParkingLot",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true, defaultValueSql: "(NULL)"),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    TotalParkingSlot = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())"),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Active"),
                    ParkingLotOwnerId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ParkingL__3214EC07C891BE20", x => x.Id);
                    table.ForeignKey(
                        name: "ParkingLot_fk8",
                        column: x => x.ParkingLotOwnerId,
                        principalTable: "ParkingLotOwnerProfile",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "PaymentSource",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BankName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AccountName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    AccountNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())"),
                    ParkingLotOwnerId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PaymentS__3214EC071D18B705", x => x.Id);
                    table.ForeignKey(
                        name: "PaymentSource_fk5",
                        column: x => x.ParkingLotOwnerId,
                        principalTable: "ParkingLotOwnerProfile",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClientRequest",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    SenderId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ClientRe__3214EC075E2A0DDC", x => x.Id);
                    table.ForeignKey(
                        name: "ClientRequest_fk0",
                        column: x => x.Id,
                        principalTable: "Request",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "ClientRequest_fk1",
                        column: x => x.SenderId,
                        principalTable: "ClientProfile",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "ParkingLotOwnerRequest",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    SenderId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ParkingL__3214EC072A46A363", x => x.Id);
                    table.ForeignKey(
                        name: "ParkingLotOwnerRequest_fk0",
                        column: x => x.Id,
                        principalTable: "Request",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "ParkingLotOwnerRequest_fk1",
                        column: x => x.SenderId,
                        principalTable: "ParkingLotOwnerProfile",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "RequestAttachedFile",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    RequestId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__RequestA__3214EC0735467020", x => x.Id);
                    table.ForeignKey(
                        name: "RequestAttachedFile_fk0",
                        column: x => x.Id,
                        principalTable: "AttachedFile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "RequestAttachedFile_fk1",
                        column: x => x.RequestId,
                        principalTable: "Request",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SharedVehicle",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    VehicleId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Available"),
                    AccessDuration = table.Column<int>(type: "int", nullable: true, defaultValueSql: "(NULL)"),
                    InvitationDate = table.Column<DateOnly>(type: "date", nullable: false, defaultValueSql: "(CONVERT([date],getdate()))"),
                    ExpirationDate = table.Column<DateOnly>(type: "date", nullable: true, defaultValueSql: "(NULL)"),
                    Note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, defaultValueSql: "(NULL)"),
                    SharedPersonId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SharedVe__3214EC07EDA49C12", x => x.Id);
                    table.ForeignKey(
                        name: "SharedVehicle_fk1",
                        column: x => x.VehicleId,
                        principalTable: "Vehicle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "SharedVehicle_fk7",
                        column: x => x.SharedPersonId,
                        principalTable: "ClientProfile",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "ParkingFeeSchedule",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    StartTime = table.Column<int>(type: "int", nullable: false),
                    EndTime = table.Column<int>(type: "int", nullable: false),
                    InitialFee = table.Column<decimal>(type: "decimal(8,2)", nullable: false),
                    AdditionalFee = table.Column<decimal>(type: "decimal(8,2)", nullable: false),
                    AdditionalMinutes = table.Column<int>(type: "int", nullable: false, defaultValue: 60),
                    DayOfWeeks = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true, defaultValueSql: "(NULL)"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())"),
                    ForVehicleType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Car"),
                    ParkingLotId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ParkingF__3214EC07ECBF6ECE", x => x.Id);
                    table.ForeignKey(
                        name: "ParkingFeeSchedule_fk10",
                        column: x => x.ParkingLotId,
                        principalTable: "ParkingLot",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ParkingSession",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    VehicleId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    ParkingLotId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    EntryDateTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())"),
                    ExitDateTime = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(NULL)"),
                    CheckOutDateTime = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(NULL)"),
                    EntryFrontCaptureUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    EntryBackCaptureUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ExitFrontCaptureUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, defaultValueSql: "(NULL)"),
                    ExitBackCaptureUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, defaultValueSql: "(NULL)"),
                    TransactionId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: true, defaultValueSql: "(NULL)"),
                    PaymentMethod = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Cash"),
                    Cost = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ParkingS__3214EC07AE20288E", x => x.Id);
                    table.ForeignKey(
                        name: "ParkingSession_fk1",
                        column: x => x.VehicleId,
                        principalTable: "Vehicle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "ParkingSession_fk2",
                        column: x => x.ParkingLotId,
                        principalTable: "ParkingLot",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StaffProfile",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    StaffId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ParkingLotId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__StaffPro__1788CC4C08D15458", x => x.UserId);
                    table.ForeignKey(
                        name: "StaffProfile_fk0",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "StaffProfile_fk2",
                        column: x => x.ParkingLotId,
                        principalTable: "ParkingLot",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WhiteList",
                columns: table => new
                {
                    ParkingLotId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    ClientId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    AddedDate = table.Column<DateOnly>(type: "date", nullable: false, defaultValueSql: "(CONVERT([date],getdate()))"),
                    ExpiredDate = table.Column<DateOnly>(type: "date", nullable: true, defaultValueSql: "(NULL)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__WhiteLis__3140FF2BC4A11819", x => new { x.ParkingLotId, x.ClientId });
                    table.ForeignKey(
                        name: "WhiteList_fk0",
                        column: x => x.ParkingLotId,
                        principalTable: "ParkingLot",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "WhiteList_fk1",
                        column: x => x.ClientId,
                        principalTable: "ClientProfile",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "IncidenceReport",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    Header = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ReportedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())"),
                    Priority = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Medium"),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Open"),
                    ReporterId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Incidenc__3214EC073D4C10CA", x => x.Id);
                    table.ForeignKey(
                        name: "IncidenceReport_fk6",
                        column: x => x.ReporterId,
                        principalTable: "StaffProfile",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShiftDiary",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    Header = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Body = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())"),
                    ParkingLotId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    SenderId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ShiftDia__3214EC07A005ABA7", x => x.Id);
                    table.ForeignKey(
                        name: "ShiftDiary_fk4",
                        column: x => x.ParkingLotId,
                        principalTable: "ParkingLot",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "ShiftDiary_fk5",
                        column: x => x.SenderId,
                        principalTable: "StaffProfile",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "IncidenceEvidence",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    IncidenceReportId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Incidenc__3214EC07F31A5873", x => x.Id);
                    table.ForeignKey(
                        name: "IncidenceEvidence_fk0",
                        column: x => x.Id,
                        principalTable: "AttachedFile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "IncidenceEvidence_fk1",
                        column: x => x.IncidenceReportId,
                        principalTable: "IncidenceReport",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdminProfile_AdminId",
                table: "AdminProfile",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_AdminProfile_Role",
                table: "AdminProfile",
                column: "Role");

            migrationBuilder.CreateIndex(
                name: "UQ__AdminPro__1788CC4D9EF726F6",
                table: "AdminProfile",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__AdminPro__719FE489908A1F22",
                table: "AdminProfile",
                column: "AdminId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AttachedFile_Name",
                table: "AttachedFile",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_AttachedFile_UploadAt",
                table: "AttachedFile",
                column: "UploadAt");

            migrationBuilder.CreateIndex(
                name: "UQ__Attached__3214EC064F8384D8",
                table: "AttachedFile",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClientProfile_CitizenId",
                table: "ClientProfile",
                column: "CitizenId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientProfile_ShareCode",
                table: "ClientProfile",
                column: "ShareCode");

            migrationBuilder.CreateIndex(
                name: "UQ__ClientPr__1788CC4D5BBDA1C7",
                table: "ClientProfile",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__ClientPr__20877041E79C6089",
                table: "ClientProfile",
                column: "ShareCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__ClientPr__6E49FA0D1DEB3EB6",
                table: "ClientProfile",
                column: "CitizenId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClientRequest_SenderId",
                table: "ClientRequest",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "UQ__ClientRe__3214EC062F9856E5",
                table: "ClientRequest",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IncidenceEvidence_IncidenceReportId",
                table: "IncidenceEvidence",
                column: "IncidenceReportId");

            migrationBuilder.CreateIndex(
                name: "UQ__Incidenc__3214EC06E432993E",
                table: "IncidenceEvidence",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IncidenceReport_Priority",
                table: "IncidenceReport",
                column: "Priority");

            migrationBuilder.CreateIndex(
                name: "IX_IncidenceReport_ReportedDate",
                table: "IncidenceReport",
                column: "ReportedDate");

            migrationBuilder.CreateIndex(
                name: "IX_IncidenceReport_ReporterId",
                table: "IncidenceReport",
                column: "ReporterId");

            migrationBuilder.CreateIndex(
                name: "IX_IncidenceReport_Status",
                table: "IncidenceReport",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "UQ__Incidenc__3214EC06B7457B6E",
                table: "IncidenceReport",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ParkingFeeSchedule_ForVehicleType",
                table: "ParkingFeeSchedule",
                column: "ForVehicleType");

            migrationBuilder.CreateIndex(
                name: "IX_ParkingFeeSchedule_IsActive",
                table: "ParkingFeeSchedule",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_ParkingFeeSchedule_ParkingLotId",
                table: "ParkingFeeSchedule",
                column: "ParkingLotId");

            migrationBuilder.CreateIndex(
                name: "UQ__ParkingF__3214EC06D4A43EC7",
                table: "ParkingFeeSchedule",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ParkingLot_CreatedAt",
                table: "ParkingLot",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ParkingLot_ParkingLotOwnerId",
                table: "ParkingLot",
                column: "ParkingLotOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_ParkingLot_Status",
                table: "ParkingLot",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "UQ__ParkingL__3214EC0632D39888",
                table: "ParkingLot",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ParkingLotOwnerProfile_ParkingLotOwnerId",
                table: "ParkingLotOwnerProfile",
                column: "ParkingLotOwnerId");

            migrationBuilder.CreateIndex(
                name: "UQ__ParkingL__1788CC4D382048FE",
                table: "ParkingLotOwnerProfile",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__ParkingL__F2333F3FC8F7E68C",
                table: "ParkingLotOwnerProfile",
                column: "ParkingLotOwnerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ParkingLotOwnerRequest_SenderId",
                table: "ParkingLotOwnerRequest",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "UQ__ParkingL__3214EC06D26B24EA",
                table: "ParkingLotOwnerRequest",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ParkingSession_EntryDateTime",
                table: "ParkingSession",
                column: "EntryDateTime");

            migrationBuilder.CreateIndex(
                name: "IX_ParkingSession_ExitDateTime",
                table: "ParkingSession",
                column: "ExitDateTime");

            migrationBuilder.CreateIndex(
                name: "IX_ParkingSession_ParkingLotId",
                table: "ParkingSession",
                column: "ParkingLotId");

            migrationBuilder.CreateIndex(
                name: "IX_ParkingSession_PaymentMethod",
                table: "ParkingSession",
                column: "PaymentMethod");

            migrationBuilder.CreateIndex(
                name: "IX_ParkingSession_VehicleId",
                table: "ParkingSession",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "UQ__ParkingS__3214EC06FA15E15F",
                table: "ParkingSession",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__ParkingS__55433A6AADDA8936",
                table: "ParkingSession",
                column: "TransactionId",
                unique: true,
                filter: "[TransactionId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentSource_ParkingLotOwnerId",
                table: "PaymentSource",
                column: "ParkingLotOwnerId");

            migrationBuilder.CreateIndex(
                name: "UQ__PaymentS__3214EC06523096D8",
                table: "PaymentSource",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Request_LastUpdatePersonId",
                table: "Request",
                column: "LastUpdatePersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Request_Status",
                table: "Request",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Request_SubmittedAt",
                table: "Request",
                column: "SubmittedAt");

            migrationBuilder.CreateIndex(
                name: "UQ__Request__3214EC06DDE85BED",
                table: "Request",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RequestAttachedFile_RequestId",
                table: "RequestAttachedFile",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "UQ__RequestA__3214EC06F7DE62C9",
                table: "RequestAttachedFile",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SharedVehicle_ExpirationDate",
                table: "SharedVehicle",
                column: "ExpirationDate");

            migrationBuilder.CreateIndex(
                name: "IX_SharedVehicle_InvitationDate",
                table: "SharedVehicle",
                column: "InvitationDate");

            migrationBuilder.CreateIndex(
                name: "IX_SharedVehicle_SharedPersonId",
                table: "SharedVehicle",
                column: "SharedPersonId");

            migrationBuilder.CreateIndex(
                name: "IX_SharedVehicle_Status",
                table: "SharedVehicle",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_SharedVehicle_VehicleId",
                table: "SharedVehicle",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "UQ__SharedVe__3214EC065D8B45A9",
                table: "SharedVehicle",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShiftDiary_CreatedAt",
                table: "ShiftDiary",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ShiftDiary_ParkingLotId",
                table: "ShiftDiary",
                column: "ParkingLotId");

            migrationBuilder.CreateIndex(
                name: "IX_ShiftDiary_SenderId",
                table: "ShiftDiary",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "UQ__ShiftDia__3214EC0646F660F8",
                table: "ShiftDiary",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StaffProfile_ParkingLotId",
                table: "StaffProfile",
                column: "ParkingLotId");

            migrationBuilder.CreateIndex(
                name: "IX_StaffProfile_StaffId",
                table: "StaffProfile",
                column: "StaffId");

            migrationBuilder.CreateIndex(
                name: "UQ__StaffPro__1788CC4D9769DB66",
                table: "StaffProfile",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__StaffPro__96D4AB16462CCED3",
                table: "StaffProfile",
                column: "StaffId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_CreatedAt",
                table: "User",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_User_Email",
                table: "User",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_User_Status",
                table: "User",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "UQ__User__3214EC068BE2D2B3",
                table: "User",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__User__5C7E359E767DDD65",
                table: "User",
                column: "Phone",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__User__A9D1053406494FD4",
                table: "User",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vehicle_CreatedAt",
                table: "Vehicle",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicle_LicensePlate",
                table: "Vehicle",
                column: "LicensePlate");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicle_OwnerId",
                table: "Vehicle",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicle_SharingStatus",
                table: "Vehicle",
                column: "SharingStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicle_Status",
                table: "Vehicle",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "UQ__Vehicle__026BC15C3384BD24",
                table: "Vehicle",
                column: "LicensePlate",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__Vehicle__3214EC06A9DEC9F7",
                table: "Vehicle",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WhiteList_AddedDate",
                table: "WhiteList",
                column: "AddedDate");

            migrationBuilder.CreateIndex(
                name: "IX_WhiteList_ClientId",
                table: "WhiteList",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_WhiteList_ExpiredDate",
                table: "WhiteList",
                column: "ExpiredDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientRequest");

            migrationBuilder.DropTable(
                name: "IncidenceEvidence");

            migrationBuilder.DropTable(
                name: "ParkingFeeSchedule");

            migrationBuilder.DropTable(
                name: "ParkingLotOwnerRequest");

            migrationBuilder.DropTable(
                name: "ParkingSession");

            migrationBuilder.DropTable(
                name: "PaymentSource");

            migrationBuilder.DropTable(
                name: "RequestAttachedFile");

            migrationBuilder.DropTable(
                name: "SharedVehicle");

            migrationBuilder.DropTable(
                name: "ShiftDiary");

            migrationBuilder.DropTable(
                name: "WhiteList");

            migrationBuilder.DropTable(
                name: "IncidenceReport");

            migrationBuilder.DropTable(
                name: "AttachedFile");

            migrationBuilder.DropTable(
                name: "Request");

            migrationBuilder.DropTable(
                name: "Vehicle");

            migrationBuilder.DropTable(
                name: "StaffProfile");

            migrationBuilder.DropTable(
                name: "AdminProfile");

            migrationBuilder.DropTable(
                name: "ClientProfile");

            migrationBuilder.DropTable(
                name: "ParkingLot");

            migrationBuilder.DropTable(
                name: "ParkingLotOwnerProfile");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
