using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplicationAPI.Migrations {
  public partial class Added_Tag : Migration {
    protected override void Up(MigrationBuilder migrationBuilder) {
      migrationBuilder.DropForeignKey(
        name: "FK_Posts_AspNetUsers_UserId",
        table: "Posts");

      migrationBuilder.AlterColumn<string>(
        name: "UserId",
        table: "Posts",
        nullable: true,
        oldClrType: typeof(string),
        oldType: "nvarchar(450)");

      migrationBuilder.CreateTable(
        name: "Tags",
        columns: table => new {
          Name = table.Column<string>(type: "nvarchar(255)", nullable: false),
          CreatorId = table.Column<string>(nullable: true),
          CreatedOn = table.Column<DateTime>(nullable: true)
        },
        constraints: table => {
          table.PrimaryKey("PK_Tags", x => x.Name);
          table.ForeignKey(
            name: "FK_Tags_AspNetUsers_CreatorId",
            column: x => x.CreatorId,
            principalTable: "AspNetUsers",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);
        });

      migrationBuilder.CreateIndex(
        name: "IX_Tags_CreatorId",
        table: "Tags",
        column: "CreatorId");

      migrationBuilder.AddForeignKey(
        name: "FK_Posts_AspNetUsers_UserId",
        table: "Posts",
        column: "UserId",
        principalTable: "AspNetUsers",
        principalColumn: "Id",
        onDelete: ReferentialAction.Restrict);
    }

    protected override void Down(MigrationBuilder migrationBuilder) {
      migrationBuilder.DropForeignKey(
        name: "FK_Posts_AspNetUsers_UserId",
        table: "Posts");

      migrationBuilder.DropTable(
        name: "Tags");

      migrationBuilder.AlterColumn<string>(
        name: "UserId",
        table: "Posts",
        type: "nvarchar(450)",
        nullable: false,
        oldClrType: typeof(string),
        oldNullable: true);

      migrationBuilder.AddForeignKey(
        name: "FK_Posts_AspNetUsers_UserId",
        table: "Posts",
        column: "UserId",
        principalTable: "AspNetUsers",
        principalColumn: "Id",
        onDelete: ReferentialAction.Cascade);
    }
  }
}
