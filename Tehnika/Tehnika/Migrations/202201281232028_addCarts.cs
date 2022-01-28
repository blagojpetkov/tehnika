namespace Tehnika.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addCarts : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ShoppingCarts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        username = c.String(),
                        address = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProductShoppingCarts",
                c => new
                    {
                        Product_Id = c.Int(nullable: false),
                        ShoppingCart_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Product_Id, t.ShoppingCart_Id })
                .ForeignKey("dbo.Products", t => t.Product_Id, cascadeDelete: true)
                .ForeignKey("dbo.ShoppingCarts", t => t.ShoppingCart_Id, cascadeDelete: true)
                .Index(t => t.Product_Id)
                .Index(t => t.ShoppingCart_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductShoppingCarts", "ShoppingCart_Id", "dbo.ShoppingCarts");
            DropForeignKey("dbo.ProductShoppingCarts", "Product_Id", "dbo.Products");
            DropIndex("dbo.ProductShoppingCarts", new[] { "ShoppingCart_Id" });
            DropIndex("dbo.ProductShoppingCarts", new[] { "Product_Id" });
            DropTable("dbo.ProductShoppingCarts");
            DropTable("dbo.ShoppingCarts");
        }
    }
}
