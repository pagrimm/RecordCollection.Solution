using Microsoft.EntityFrameworkCore.Migrations;

namespace RecordCollection.Migrations
{
    public partial class ImageUrlForArtist : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Artists",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Artists");
        }
    }
}
