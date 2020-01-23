namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class create_person_table : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.People",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        CountryCode = c.String(nullable: false),
                        Phone = c.String(nullable: false),
                        Gender = c.String(nullable: false),
                        BirthDate = c.DateTime(nullable: false),
                        Avatar = c.String(nullable: false),
                        Email = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.People");
        }
    }
}
