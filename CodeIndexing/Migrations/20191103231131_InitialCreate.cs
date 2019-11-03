using Microsoft.EntityFrameworkCore.Migrations;

namespace CodeIndexing.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Classes",
                columns: table => new
                {
                    ClassId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UnderlyingType = table.Column<int>(nullable: false),
                    ClassName = table.Column<string>(nullable: true),
                    Namespace = table.Column<string>(nullable: true),
                    FileName = table.Column<string>(nullable: true),
                    FilePath = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classes", x => x.ClassId);
                });

            migrationBuilder.CreateTable(
                name: "Methods",
                columns: table => new
                {
                    MethodId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MethodName = table.Column<string>(nullable: true),
                    Namespace = table.Column<string>(nullable: true),
                    FileName = table.Column<string>(nullable: true),
                    FilePath = table.Column<string>(nullable: true),
                    BelongsToClassId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Methods", x => x.MethodId);
                    table.ForeignKey(
                        name: "FK_Methods_Classes_BelongsToClassId",
                        column: x => x.BelongsToClassId,
                        principalTable: "Classes",
                        principalColumn: "ClassId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MethodRelationship",
                columns: table => new
                {
                    CallingMethodId = table.Column<int>(nullable: false),
                    MethodBeingCalledId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MethodRelationship", x => new { x.CallingMethodId, x.MethodBeingCalledId });
                    table.ForeignKey(
                        name: "FK_MethodRelationship_Methods_CallingMethodId",
                        column: x => x.CallingMethodId,
                        principalTable: "Methods",
                        principalColumn: "MethodId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MethodRelationship_Methods_MethodBeingCalledId",
                        column: x => x.MethodBeingCalledId,
                        principalTable: "Methods",
                        principalColumn: "MethodId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Parameters",
                columns: table => new
                {
                    ParameterId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ParamterTypeClassId = table.Column<int>(nullable: true),
                    ParameterName = table.Column<string>(nullable: true),
                    BelongsToMethodMethodId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parameters", x => x.ParameterId);
                    table.ForeignKey(
                        name: "FK_Parameters_Methods_BelongsToMethodMethodId",
                        column: x => x.BelongsToMethodMethodId,
                        principalTable: "Methods",
                        principalColumn: "MethodId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Parameters_Classes_ParamterTypeClassId",
                        column: x => x.ParamterTypeClassId,
                        principalTable: "Classes",
                        principalColumn: "ClassId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MethodRelationship_MethodBeingCalledId",
                table: "MethodRelationship",
                column: "MethodBeingCalledId");

            migrationBuilder.CreateIndex(
                name: "IX_Methods_BelongsToClassId",
                table: "Methods",
                column: "BelongsToClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Parameters_BelongsToMethodMethodId",
                table: "Parameters",
                column: "BelongsToMethodMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_Parameters_ParamterTypeClassId",
                table: "Parameters",
                column: "ParamterTypeClassId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MethodRelationship");

            migrationBuilder.DropTable(
                name: "Parameters");

            migrationBuilder.DropTable(
                name: "Methods");

            migrationBuilder.DropTable(
                name: "Classes");
        }
    }
}
