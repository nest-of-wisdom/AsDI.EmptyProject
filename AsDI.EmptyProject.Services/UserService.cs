using AsDI.Attributes;
using AsDI.DbExtend.Attributes;
using AsDI.EmptyProject.IServices;
using AsDI.EmptyProject.IServices.Dtos;
using AsDI.EmptyProject.Repositories;
using AsDI.EmptyProject.Repositories.Base;
using AsDI.EmptyProject.Repositories.Entities;
using AsDI.EmptyProject.Services.Base;
using AsDI.EmptyProject.Utils.Models;
using Mapster;
using System.ComponentModel.DataAnnotations;

namespace AsDI.EmptyProject.Services
{
    [Service]
    public class UserService : BaseService<UserDTO, UserEntity>, IUserService
    {
        private readonly IUserRepository repository;

        public UserService(IUserRepository repository)
        {
            this.repository = repository;
        }

        public override IBaseRepository<UserEntity> BaseRepository => repository;

        public UserDTO GetByUserName(string userName)
        {
            string condition = "userId='admin'";

            PagedList<UserEntity> data = repository.Query(condition, new Dictionary<string, object>(), 1, 10);

            return repository.FindUser(userName).Adapt<UserDTO>();
        }

        public PagedList<UserDTO> Query(string keyword, int page, int pageSize)
        {
            keyword ??= "";



            var data = repository.GetList(p => keyword == "" || p.UserName.Contains(keyword) || p.Email.Contains(keyword), p => p.CreatedTime, page, pageSize);
            PagedList<UserDTO> rtn = new()
            {
                Data = data.Data.Adapt<List<UserDTO>>(),
                Page = data.Page,
                PageSize = data.PageSize,
                Total = data.Total
            };

            return rtn;

        }
    }

}
