namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class drop_avatar_column : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.People", "Avatar");
        }
        
        public override void Down()
        {
            AddColumn("dbo.People", "Avatar", c => c.String(nullable: false));
        }
    }
}
