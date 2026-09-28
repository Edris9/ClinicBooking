using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClinicBooking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddResponsiblePhysician : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ResponsiblePhysician",
                table: "Patients",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Patients_ResponsiblePhysician",
                table: "Patients",
                column: "ResponsiblePhysician");

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_Doctors_ResponsiblePhysician",
                table: "Patients",
                column: "ResponsiblePhysician",
                principalTable: "Doctors",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Patients_Doctors_ResponsiblePhysician",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Patients_ResponsiblePhysician",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "ResponsiblePhysician",
                table: "Patients");
        }
    }
}
