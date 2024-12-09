using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitDb1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicalRecords_Users_DoctorId",
                table: "MedicalRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Users_DoctorId",
                table: "Users");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalRecords_Users_DoctorId",
                table: "MedicalRecords",
                column: "DoctorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Users_DoctorId",
                table: "Users",
                column: "DoctorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicalRecords_Users_DoctorId",
                table: "MedicalRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Users_DoctorId",
                table: "Users");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalRecords_Users_DoctorId",
                table: "MedicalRecords",
                column: "DoctorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Users_DoctorId",
                table: "Users",
                column: "DoctorId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
