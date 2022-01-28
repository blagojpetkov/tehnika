namespace Tehnika.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class comments : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProductComments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        comment = c.String(),
                        user_Id = c.String(maxLength: 128),
                        Product_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.user_Id)
                .ForeignKey("dbo.Products", t => t.Product_Id)
                .Index(t => t.user_Id)
                .Index(t => t.Product_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductComments", "Product_Id", "dbo.Products");
            DropForeignKey("dbo.ProductComments", "user_Id", "dbo.AspNetUsers");
            DropIndex("dbo.ProductComments", new[] { "Product_Id" });
            DropIndex("dbo.ProductComments", new[] { "user_Id" });
            DropTable("dbo.ProductComments");
        }
    }
}
