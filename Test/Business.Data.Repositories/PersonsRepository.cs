using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business.Data;
using Data;
using DTO;

namespace Business.Repositories
{
    public class PersonsRepository : Business.Contacts.IPersonsRepository
    {
        DataBaseContext _dbcontext;
        public PersonsRepository()
        {
            this._dbcontext = new DataBaseContext();
        }

        public Business.Data.Person Add(DTO.PersonRequest person)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg
                .CreateMap<DTO.PersonRequest, Data.Person>()
                .ForMember(x => x.Avatar, e => e.MapFrom(o => Convert.FromBase64String(o.Avatar)));
            });

            IMapper iMapper = config.CreateMapper();

            var dataItem = iMapper.Map<DTO.PersonRequest, Data.Person>(person);

            try
            {
                this._dbcontext.Persons.Add(dataItem);
                this._dbcontext.SaveChanges();

                return dataItem;
            }
            catch (Exception ex)
            {
                throw new Exception("error while saving person in database");
            }
        }

        public Data.Person Get(string phone)
        {
            try
            {
                return _dbcontext.Persons.FirstOrDefault(x => x.Phone == phone);
            }
            catch (Exception ex)
            {
                throw new Exception("can't access database");
            }
        }

        public bool IsEmailExists(string email)
        {
            try
            {
                return _dbcontext.Persons.Any(x => x.Email == email);
            }
            catch (Exception ex)
            {
                throw new Exception("can't access database");
            }
        }

        public bool IsPhoneExists(string phone)
        {
            try
            {
                return _dbcontext.Persons.Any(x => x.Phone == phone);
            }
            catch (Exception ex)
            {
                throw new Exception("can't access database");
            }
        }

    }
}
