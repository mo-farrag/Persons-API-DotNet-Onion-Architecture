using AutoMapper;
using Business.Contacts.Repositories;
using Data;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Data.Repositories
{
    public class AuthTokenRepository : IAuthTokenRepository
    {
        DataBaseContext _dbcontext;
        public AuthTokenRepository()
        {
            this._dbcontext = new DataBaseContext();
        }
        public int Add(DTO.AuthToken token)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DTO.AuthToken, Data.AuthToken>();
            });

            IMapper iMapper = config.CreateMapper();

            var dataItem = iMapper.Map<DTO.AuthToken, Data.AuthToken>(token);
            this._dbcontext.AuthTokens.Add(dataItem);
            this._dbcontext.SaveChanges();

            return dataItem.Id;
        }

        public DTO.AuthToken Get(string phone, string token)
        {
            var item = _dbcontext.AuthTokens.FirstOrDefault(x => x.Phone == phone && x.Token == token);
            if (item == null)
                return null;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Data.AuthToken, DTO.AuthToken>();
            });
            IMapper iMapper = config.CreateMapper();

            return iMapper.Map<Data.AuthToken, DTO.AuthToken>(item);
        }
    }
}
