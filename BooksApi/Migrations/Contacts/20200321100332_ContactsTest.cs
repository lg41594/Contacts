using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ContactsApi.Migrations.Contacts
{
    public partial class ContactsTest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Contacts",
                columns: table => new
                {
                    ContactId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Address = table.Column<string>(maxLength: 200, nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => x.ContactId);
                    table.UniqueConstraint("unique2", i => new { i.Name, i.Address });
                });

            migrationBuilder.CreateTable(
                name: "PhoneNumber",
                columns: table => new
                {
                    PhoneNumberId = table.Column<Guid>(nullable: false),
                    Number = table.Column<string>(nullable: false),
                    ContactId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhoneNumber", x => x.PhoneNumberId);
                    table.ForeignKey(
                        name: "FK_PhoneNumber_Contacts_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contacts",
                        principalColumn: "ContactId",
                        onDelete: ReferentialAction.Cascade);
                    
                });

            migrationBuilder.CreateIndex(
                name: "IX_PhoneNumber_ContactId",
                table: "PhoneNumber",
                column: "ContactId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PhoneNumber");

            migrationBuilder.DropTable(
                name: "Contacts");
        }
    }
}
