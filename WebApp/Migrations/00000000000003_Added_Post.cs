using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplicationAPI.Migrations {
  public partial class Added_Post : Migration {
    protected override void Up(MigrationBuilder migrationBuilder) {
      migrationBuilder.CreateTable(
        name: "Posts",
        columns: table => new {
          Id = table.Column<Guid>(nullable: false),
          Name = table.Column<string>(nullable: false)
        },
        constraints: table => {
          table.PrimaryKey("PK_Posts", x => x.Id);
        });
    }

    protected override void Down(MigrationBuilder migrationBuilder) {
      migrationBuilder.DropTable(
        name: "Posts");
    }
  }
}
