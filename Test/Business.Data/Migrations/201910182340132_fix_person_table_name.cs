namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fix_person_table_name : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.People", newName: "Persons");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.Persons", newName: "People");
        }
    }
}
