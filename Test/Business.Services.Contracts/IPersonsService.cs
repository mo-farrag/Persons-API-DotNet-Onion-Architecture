

namespace Business.Services.Contracts
{
    public interface IPersonsService
    {
        DTO.PersonResult Add(DTO.PersonRequest person);
    }
}
