using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fontys.BlockExplorer.API.Migrations
{
    public partial class initialDb : Migration
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
                    PreviousBlockHash = table.Column<string>(type: "text", nullable: true),
                    CoinType = table.Column<int>(type: "integer", nullable: false),
                    NetworkType = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blocks", x => x.Hash);
                });

            migrationBuilder.CreateTable(
                name: "Tx",
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
                name: "TxInputs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IsNewlyGenerated = table.Column<bool>(type: "boolean", nullable: false),
                    TransactionHash = table.Column<string>(type: "text", nullable: true),
                    Value = table.Column<double>(type: "double precision", nullable: false),
                    AddressHash = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TxInputs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TxInputs_Addresses_AddressHash",
                        column: x => x.AddressHash,
                        principalTable: "Addresses",
                        principalColumn: "Hash");
                    table.ForeignKey(
                        name: "FK_TxInputs_Transactions_TransactionHash",
                        column: x => x.TransactionHash,
                        principalTable: "Tx",
                        principalColumn: "Hash");
                });

            migrationBuilder.CreateTable(
                name: "TxOutputs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TransactionHash = table.Column<string>(type: "text", nullable: true),
                    Value = table.Column<double>(type: "double precision", nullable: false),
                    AddressHash = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TxOutputs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TxOutputs_Addresses_AddressHash",
                        column: x => x.AddressHash,
                        principalTable: "Addresses",
                        principalColumn: "Hash");
                    table.ForeignKey(
                        name: "FK_TxOutputs_Transactions_TransactionHash",
                        column: x => x.TransactionHash,
                        principalTable: "Tx",
                        principalColumn: "Hash");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_BlockHash",
                table: "Tx",
                column: "BlockHash");

            migrationBuilder.CreateIndex(
                name: "IX_TxInputs_AddressHash",
                table: "TxInputs",
                column: "AddressHash");

            migrationBuilder.CreateIndex(
                name: "IX_TxInputs_TransactionHash",
                table: "TxInputs",
                column: "TransactionHash");

            migrationBuilder.CreateIndex(
                name: "IX_TxOutputs_AddressHash",
                table: "TxOutputs",
                column: "AddressHash");

            migrationBuilder.CreateIndex(
                name: "IX_TxOutputs_TransactionHash",
                table: "TxOutputs",
                column: "TransactionHash");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TxInputs");

            migrationBuilder.DropTable(
                name: "TxOutputs");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "Tx");

            migrationBuilder.DropTable(
                name: "Blocks");
        }
    }
}
