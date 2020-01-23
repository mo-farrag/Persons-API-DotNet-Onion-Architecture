using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System;
using System.Linq;
using System.Web;
using Business.Data;

namespace Data
{

    public class DataBaseContext : DbContext
    {
        public DataBaseContext() : base("dbConnection")
        {

        }

        public DbSet<Person> Persons { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<AuthToken> AuthTokens { get; set; }

    }

}
