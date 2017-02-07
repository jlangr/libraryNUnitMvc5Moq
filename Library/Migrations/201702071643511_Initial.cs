namespace Library.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Branches",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 20),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Holdings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Classification = c.String(),
                        CopyNumber = c.Int(nullable: false),
                        CheckOutTimestamp = c.DateTime(),
                        LastCheckedIn = c.DateTime(),
                        DueDate = c.DateTime(),
                        BranchId = c.Int(nullable: false),
                        HeldByPatronId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Patrons",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 25),
                        Balance = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Patrons");
            DropTable("dbo.Holdings");
            DropTable("dbo.Branches");
        }
    }
}
