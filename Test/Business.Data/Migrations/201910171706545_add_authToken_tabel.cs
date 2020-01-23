namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_authToken_tabel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AuthTokens",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Phone = c.String(),
                        Password = c.String(),
                        Token = c.String(),
                        ExpireAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.AuthTokens");
        }
    }
}
