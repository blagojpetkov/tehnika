namespace Tehnika.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddGrading : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "Grade", c => c.Double(nullable: false));
            AddColumn("dbo.Products", "Graders", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "Graders");
            DropColumn("dbo.Products", "Grade");
        }
    }
}
