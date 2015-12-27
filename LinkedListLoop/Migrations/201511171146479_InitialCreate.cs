namespace LinkedListLoop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cheques",
                c => new
                    {
                        id = c.String(nullable: false, maxLength: 128),
                        Sender = c.String(),
                        Receiver = c.String(),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Date = c.String(),
                        FileId = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Cheques");
        }
    }
}
