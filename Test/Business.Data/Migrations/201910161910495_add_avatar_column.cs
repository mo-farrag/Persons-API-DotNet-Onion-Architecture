namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_avatar_column : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.People", "Avatar", c => c.Binary(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.People", "Avatar");
        }
    }
}
