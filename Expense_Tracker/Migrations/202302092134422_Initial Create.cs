namespace Expense_Tracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        CategoryID = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(),
                    })
                .PrimaryKey(t => t.CategoryID);
            
            CreateTable(
                "dbo.Expenses",
                c => new
                    {
                        ExpenseID = c.Int(nullable: false, identity: true),
                        ExpenseDate = c.String(),
                        ExpenseDescription = c.String(),
                        ExpenseAmount = c.Single(nullable: false),
                        MemberID = c.Int(nullable: false),
                        CategoryID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ExpenseID)
                .ForeignKey("dbo.Categories", t => t.CategoryID, cascadeDelete: true)
                .ForeignKey("dbo.Members", t => t.MemberID, cascadeDelete: true)
                .Index(t => t.MemberID)
                .Index(t => t.CategoryID);
            
            CreateTable(
                "dbo.Members",
                c => new
                    {
                        MemberID = c.Int(nullable: false, identity: true),
                        MemberName = c.String(),
                    })
                .PrimaryKey(t => t.MemberID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Expenses", "MemberID", "dbo.Members");
            DropForeignKey("dbo.Expenses", "CategoryID", "dbo.Categories");
            DropIndex("dbo.Expenses", new[] { "CategoryID" });
            DropIndex("dbo.Expenses", new[] { "MemberID" });
            DropTable("dbo.Members");
            DropTable("dbo.Expenses");
            DropTable("dbo.Categories");
        }
    }
}
