using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Devices.Infrastructure.Migrations;

/// <inheritdoc />
public partial class InitialMigration : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.EnsureSchema(
            name: "Entities");

        migrationBuilder.CreateTable(
            name: "Device",
            schema: "Entities",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                Brand = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                State = table.Column<int>(type: "integer", nullable: false),
                CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                Removed = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Device", x => x.Id);
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Device",
            schema: "Entities");
    }
}
