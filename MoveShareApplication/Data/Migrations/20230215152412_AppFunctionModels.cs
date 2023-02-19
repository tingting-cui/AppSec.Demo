using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoveShareApplication.Data.Migrations
{
    /// <inheritdoc />
    public partial class AppFunctionModels : Migration
    {
        /// <inheritdoc />
        /// Comment out cascade delete attribute of FK_Order_AspNetUsers_Customer_id.Otherwise it will trigger error.
        
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Item",
                columns: table => new
                {
                    Itemid = table.Column<string>(name: "Item_id", type: "nvarchar(450)", nullable: false),
                    Ownerid = table.Column<string>(name: "Owner_id", type: "nvarchar(450)", nullable: false),
                    Createdat = table.Column<DateTime>(name: "Created_at", type: "datetime2", nullable: false),
                    LastUpdateat = table.Column<DateTime>(name: "LastUpdate_at", type: "datetime2", nullable: false),
                    Available = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    PickUpNote = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Item", x => x.Itemid);
                    table.ForeignKey(
                        name: "FK_Item_AspNetUsers_Owner_id",
                        column: x => x.Ownerid,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    Orderid = table.Column<string>(name: "Order_id", type: "nvarchar(450)", nullable: false),
                    Itemid = table.Column<string>(name: "Item_id", type: "nvarchar(450)", nullable: false),
                    Orderquantity = table.Column<int>(name: "Order_quantity", type: "int", nullable: false),
                    Createdat = table.Column<DateTime>(name: "Created_at", type: "datetime2", nullable: false),
                    LastUpdateat = table.Column<DateTime>(name: "LastUpdate_at", type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Customerid = table.Column<string>(name: "Customer_id", type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.Orderid);
                    table.ForeignKey(
                        name: "FK_Order_AspNetUsers_Customer_id",
                        column: x => x.Customerid,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id"//,
                        //onDelete: ReferentialAction.Cascade
                        );
                    table.ForeignKey(
                        name: "FK_Order_Item_Item_id",
                        column: x => x.Itemid,
                        principalTable: "Item",
                        principalColumn: "Item_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Picture",
                columns: table => new
                {
                    Pictureid = table.Column<string>(name: "Picture_id", type: "nvarchar(450)", nullable: false),
                    Pictures = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Createdat = table.Column<DateTime>(name: "Created_at", type: "datetime2", nullable: false),
                    AssociatedItemid = table.Column<string>(name: "AssociatedItem_id", type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Picture", x => x.Pictureid);
                    table.ForeignKey(
                        name: "FK_Picture_Item_AssociatedItem_id",
                        column: x => x.AssociatedItemid,
                        principalTable: "Item",
                        principalColumn: "Item_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Item_Owner_id",
                table: "Item",
                column: "Owner_id");

            migrationBuilder.CreateIndex(
                name: "IX_Order_Customer_id",
                table: "Order",
                column: "Customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_Order_Item_id",
                table: "Order",
                column: "Item_id");

            migrationBuilder.CreateIndex(
                name: "IX_Picture_AssociatedItem_id",
                table: "Picture",
                column: "AssociatedItem_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "Picture");

            migrationBuilder.DropTable(
                name: "Item");
        }
    }
}
