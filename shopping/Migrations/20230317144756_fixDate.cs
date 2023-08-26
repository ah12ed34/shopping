using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace shopping.Migrations
{
    public partial class fixDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_orders_id_member",
                table: "orders",
                column: "id_member");

            migrationBuilder.AddForeignKey(
                name: "FK_orders_members_id_member",
                table: "orders",
                column: "id_member",
                principalTable: "members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_orders_members_id_member",
                table: "orders");

            migrationBuilder.DropIndex(
                name: "IX_orders_id_member",
                table: "orders");
        }
    }
}
