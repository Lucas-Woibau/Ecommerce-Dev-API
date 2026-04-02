using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcommerceDev.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddPaymentFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IdExternalOrder",
                table: "Orders",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentUrl",
                table: "Orders",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IdExternalPayment",
                table: "Customers",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdExternalOrder",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "PaymentUrl",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "IdExternalPayment",
                table: "Customers");
        }
    }
}
