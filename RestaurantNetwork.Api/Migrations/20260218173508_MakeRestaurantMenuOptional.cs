using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantNetwork.Api.Migrations
{
    /// <inheritdoc />
    public partial class MakeRestaurantMenuOptional : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Restaurants_Menu_MenuId",
                table: "Restaurants");

            migrationBuilder.AlterColumn<int>(
                name: "MenuId",
                table: "Restaurants",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurants_Menu_MenuId",
                table: "Restaurants",
                column: "MenuId",
                principalTable: "Menu",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Restaurants_Menu_MenuId",
                table: "Restaurants");

            migrationBuilder.AlterColumn<int>(
                name: "MenuId",
                table: "Restaurants",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurants_Menu_MenuId",
                table: "Restaurants",
                column: "MenuId",
                principalTable: "Menu",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
