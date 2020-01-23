namespace Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Data.DataBaseContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Data.DataBaseContext context)
        {
            context.Countries.AddOrUpdate(
                  p => p.Name,
                  new Business.Data.Country { Name = "Egypt", Code = "EG" },
                  new Business.Data.Country { Name = "United states", Code = "US" },
                  new Business.Data.Country { Name = "Saudia arabia", Code = "KSA" }

                );
        }
    }
}
