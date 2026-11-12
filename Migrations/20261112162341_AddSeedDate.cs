using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Assignment_04_EFC_.Migrations
{
    /// <inheritdoc />
    public partial class AddSeedDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Managers",
                columns: new[] { "Id", "Address", "DateOfBirth", "Email", "FullName", "HireDate", "PhoneNumber" },
                values: new object[] { 1, "Cairo, Egypt", new DateTime(1980, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "maged@bank.com", "Maged Mohamed", new DateTime(2018, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "01012345678" });

            migrationBuilder.InsertData(
                table: "Branches",
                columns: new[] { "Code", "Address", "ManagerId", "Name", "PhoneNumber" },
                values: new object[] { "CAI-01", "Tahrir Square, Cairo", 1, "Cairo Main Branch", "022345678" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Branches",
                keyColumn: "Code",
                keyValue: "CAI-01");

            migrationBuilder.DeleteData(
                table: "Managers",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
