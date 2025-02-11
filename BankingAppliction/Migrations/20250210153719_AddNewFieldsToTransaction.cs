using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankingAppliction.Migrations
{
    /// <inheritdoc />
    public partial class AddNewFieldsToTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                table: "Transactions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "FullNameEnglish",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FullNameHebrew",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "FullNameEnglish",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "FullNameHebrew",
                table: "Transactions");
        }
    }
}
