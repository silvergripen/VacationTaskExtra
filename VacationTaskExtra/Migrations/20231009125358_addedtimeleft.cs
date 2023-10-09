using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VacationTaskExtra.Migrations
{
    /// <inheritdoc />
    public partial class addedtimeleft : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "timeLefts",
                columns: table => new
                {
                    TimeLeftId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TimeLeft = table.Column<int>(type: "int", nullable: false),
                    FK_Personel = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FK_VacationType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_timeLefts", x => x.TimeLeftId);
                    table.ForeignKey(
                        name: "FK_timeLefts_AspNetUsers_FK_Personel",
                        column: x => x.FK_Personel,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_timeLefts_VacationTypes_FK_VacationType",
                        column: x => x.FK_VacationType,
                        principalTable: "VacationTypes",
                        principalColumn: "TypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_timeLefts_FK_Personel",
                table: "timeLefts",
                column: "FK_Personel");

            migrationBuilder.CreateIndex(
                name: "IX_timeLefts_FK_VacationType",
                table: "timeLefts",
                column: "FK_VacationType");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "timeLefts");
        }
    }
}
