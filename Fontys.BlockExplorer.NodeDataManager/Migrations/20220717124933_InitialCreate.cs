using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fontys.BlockExplorer.API.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Hash = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Hash);
                });

            migrationBuilder.CreateTable(
                name: "Blocks",
                columns: table => new
                {
                    Hash = table.Column<string>(type: "text", nullable: false),
                    Height = table.Column<int>(type: "integer", nullable: false),
                    PreviousHash = table.Column<string>(type: "text", nullable: true),
                    CoinType = table.Column<int>(type: "integer", nullable: false),
                    NetworkType = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blocks", x => x.Hash);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Hash = table.Column<string>(type: "text", nullable: false),
                    BlockHash = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Hash);
                    table.ForeignKey(
                        name: "FK_Transactions_Blocks_BlockHash",
                        column: x => x.BlockHash,
                        principalTable: "Blocks",
                        principalColumn: "Hash");
                });

            migrationBuilder.CreateTable(
                name: "Transfers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Value = table.Column<long>(type: "bigint", nullable: false),
                    AddressHash = table.Column<string>(type: "text", nullable: false),
                    TransactionHash = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transfers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transfers_Addresses_AddressHash",
                        column: x => x.AddressHash,
                        principalTable: "Addresses",
                        principalColumn: "Hash",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transfers_Transactions_TransactionHash",
                        column: x => x.TransactionHash,
                        principalTable: "Transactions",
                        principalColumn: "Hash");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_BlockHash",
                table: "Transactions",
                column: "BlockHash");

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_AddressHash",
                table: "Transfers",
                column: "AddressHash");

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_TransactionHash",
                table: "Transfers",
                column: "TransactionHash");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transfers");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "Blocks");
        }
    }
}
