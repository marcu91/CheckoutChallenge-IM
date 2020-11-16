using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Gateway.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Banks",
                columns: table => new
                {
                    BankID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BankName = table.Column<string>(type: "NVARCHAR(200)", nullable: false),
                    BankURL = table.Column<string>(type: "NVARCHAR(200)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banks", x => x.BankID);
                });

            migrationBuilder.CreateTable(
                name: "CardDetails",
                columns: table => new
                {
                    CardDetailsID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CardNumber = table.Column<string>(type: "VARCHAR(25)", nullable: false),
                    Cvv = table.Column<string>(type: "VARCHAR(4)", maxLength: 4, nullable: false),
                    HolderName = table.Column<string>(type: "NVARCHAR(200)", nullable: false),
                    ExpiryMonth = table.Column<string>(type: "VARCHAR(2)", nullable: false),
                    ExpiryYear = table.Column<short>(type: "SMALLINT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardDetails", x => x.CardDetailsID);
                });

            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    CurrencyId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.CurrencyId);
                });

            migrationBuilder.CreateTable(
                name: "Merchants",
                columns: table => new
                {
                    MerchantID = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Active = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Merchants", x => x.MerchantID);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    TransactionID = table.Column<Guid>(nullable: false),
                    Amount = table.Column<decimal>(nullable: false),
                    Status = table.Column<string>(maxLength: 20, nullable: true),
                    SubStatus = table.Column<string>(maxLength: 100, nullable: true),
                    BankReferenceID = table.Column<Guid>(nullable: false),
                    BankID = table.Column<int>(nullable: false),
                    MerchantID = table.Column<Guid>(nullable: false),
                    CurrencyId = table.Column<int>(nullable: false),
                    CardDetailsID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.TransactionID);
                    table.ForeignKey(
                        name: "FK_Transactions_Banks_BankID",
                        column: x => x.BankID,
                        principalTable: "Banks",
                        principalColumn: "BankID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transactions_CardDetails_CardDetailsID",
                        column: x => x.CardDetailsID,
                        principalTable: "CardDetails",
                        principalColumn: "CardDetailsID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transactions_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "CurrencyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transactions_Merchants_MerchantID",
                        column: x => x.MerchantID,
                        principalTable: "Merchants",
                        principalColumn: "MerchantID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_BankID",
                table: "Transactions",
                column: "BankID");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_CardDetailsID",
                table: "Transactions",
                column: "CardDetailsID");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_CurrencyId",
                table: "Transactions",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_MerchantID",
                table: "Transactions",
                column: "MerchantID");

            #region Data Seed
            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Name" },
                values: new object[] { "RON" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Name" },
                values: new object[] { "USD" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Name" },
                values: new object[] { "GBP" });

            migrationBuilder.InsertData(
                    table: "Currencies",
                    columns: new[] { "Name" },
                    values: new object[] { "EUR" });

            migrationBuilder.InsertData(
                    table: "Currencies",
                    columns: new[] { "Name" },
                    values: new object[] { "CAD" });

            migrationBuilder.InsertData(
                table: "Merchants",
                columns: new[] { "MerchantID", "Active", "Description", "Name" },
                values: new object[,]
                {
                    { new Guid("a9b05e16-acd8-4acf-8e29-951e0d39da9a"), true, "This is a description of Amazon.", "Amazon" },
                    { new Guid("b3e7c684-99a2-4253-898b-01515b92f1b1"), true, "This is a description of Apple.", "Apple" },
                    { new Guid("e7039e52-410b-4a4a-8516-b15d64880734"), true, "This is a description of E-bay.", "E-bay" }
                });

            migrationBuilder.InsertData(
                table: "Banks",
                columns: new[] { "BankID", "BankName", "BankURL" },
                values: new object[,]
                {
                    { 1, "MockBank", "http://gatewayapimockbank.azurewebsites.net/" },
                    { 2, "LocalTestBank", "http://localhost:32771/" }
                }
            );
            #endregion

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "Banks");

            migrationBuilder.DropTable(
                name: "CardDetails");

            migrationBuilder.DropTable(
                name: "Currencies");

            migrationBuilder.DropTable(
                name: "Merchants");
        }
    }
}
