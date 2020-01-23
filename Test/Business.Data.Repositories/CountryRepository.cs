using Business.Contacts.Repositories;
using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Data.Repositories
{
    public class CountryRepository : ICountryRepository
    {
        DataBaseContext _dbcontext;

        public CountryRepository()
        {
            this._dbcontext = new DataBaseContext();
        }

        public bool IsCountryCodeExists(string countrCode)
        {
            try
            {
                return _dbcontext.Countries.Any(x => x.Code == countrCode);
            }
            catch(Exception ex)
            {
                throw new Exception("can't access database");
            }
        }
    }
}
