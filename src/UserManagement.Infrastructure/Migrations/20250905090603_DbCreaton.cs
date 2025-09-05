using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserManagement.Infrastructure.Migrations
{
    public partial class DbCreaton : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    UserName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MobileNumber = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Language = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Culture = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApiKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Clients_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Clients_UserId",
                table: "Clients",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserName",
                table: "Users",
                column: "UserName",
                unique: true);

            var userId = new Guid("B9ABE5C4-99E1-41B7-BD13-5580D26F1D1F");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[]
                {
                    "Id", "UserName", "PasswordHash", "FirstName", "LastName",
                    "Email", "MobileNumber", "Language", "Culture",
                    "DateCreated", "DateModified"
                },
                values: new object[]
                {
                    userId,
                    "Nebojsa123",
                    "$2a$11$S56JhB.yHp8TIE8UVZSDOebpR9UgssdFKud4apWsF2l7/wS5gjyMG",
                    "Nebojsa",
                    "Kovacic",
                    "kovacic.nebojsa96@gmail.com",
                    "+381 123 1234 ",
                    "SR",
                    "SR",
                    new DateTime(2025, 9, 5, 8, 47, 11, 930, DateTimeKind.Utc),
                    null
                }
            );

            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] {"UserId", "ApiKey", "DateCreated" },
                values: new object[]
                {
                    userId,
                    "8956322a-3f73-4f05-91af-5ceeeffaa5ec",
                    new DateTime(2025, 9, 5, 8, 47, 11, 930, DateTimeKind.Utc)
                }
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "Clients");
            migrationBuilder.DropTable(name: "Users");
        }
    }
}
