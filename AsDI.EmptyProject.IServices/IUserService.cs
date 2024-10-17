using AsDI.EmptyProject.IServices.Base;
using AsDI.EmptyProject.IServices.Dtos;
using AsDI.EmptyProject.Utils.Models;

namespace AsDI.EmptyProject.IServices
{
    public interface IUserService : IBaseService<UserDTO>
    {
        UserDTO GetByUserName(string userName);


        PagedList<UserDTO> Query(string? keyword, int page, int pageSize);


    }
}
