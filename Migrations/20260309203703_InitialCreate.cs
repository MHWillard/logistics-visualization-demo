using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace logistics_visualization_demo.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Company",
                columns: table => new
                {
                    CompanyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company", x => x.CompanyId);
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.OrderId);
                });

            migrationBuilder.CreateTable(
                name: "OrderDetail",
                columns: table => new
                {
                    OrderDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetail", x => x.OrderDetailId);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.ProductId);
                });

            migrationBuilder.Sql(@"
                CREATE VIEW MonthlyOrderStats AS
                SELECT
                    CAST(YEAR(o.OrderDate) AS VARCHAR(4)) + '-' + RIGHT('0' + CAST(MONTH(o.OrderDate) AS VARCHAR(2)), 2) + '-' + CAST(c.CompanyId AS VARCHAR(10)) AS Id,
                    YEAR(o.OrderDate) AS Year,
                    MONTH(o.OrderDate) AS Month,
                    c.CompanyId,
                    c.Name AS CompanyName,
                    COUNT(DISTINCT o.OrderId) AS TotalOrders,
                    SUM(od.Quantity * p.Price) AS TotalIncome
                FROM [Order] o
                JOIN Company c ON o.CompanyId = c.CompanyId
                JOIN OrderDetail od ON o.OrderId = od.OrderId
                JOIN Product p ON od.ProductId = p.ProductId
                GROUP BY YEAR(o.OrderDate), MONTH(o.OrderDate), c.CompanyId, c.Name
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW IF EXISTS MonthlyOrderStats");

            migrationBuilder.DropTable(
                name: "Company");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "OrderDetail");

            migrationBuilder.DropTable(
                name: "Product");
        }
    }
}
