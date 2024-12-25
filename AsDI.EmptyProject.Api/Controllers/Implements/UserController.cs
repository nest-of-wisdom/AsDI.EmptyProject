using AsDI.Attributes;
using AsDI.EmptyProject.IServices;
using AsDI.EmptyProject.IServices.Dtos;
using AsDI.EmptyProject.Utils.Models;
using Microsoft.AspNetCore.Mvc;

namespace AsDI.EmptyProject.Api.Controllers
{
    [Service]
    public class UserController : IUserController
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        public PagedList<UserDTO> Query(string? keyword, int page, int pageSize)
        {
            return userService.Query(keyword, page, pageSize);
        }

        public UserDTO Get(string id)
        {
            return userService.GetSingle(id);
        }

        public UserDTO Add(UserDTO user)
        {
            return userService.Insert(user);
        }

        public UserDTO Update(UserDTO update)
        {
            return userService.Update(update);
        }

        public int Delete(string id)
        {
            return userService.Delete(id);
        }

        public UserDTO GetByUserName(string userName)
        {
            return userService.GetByUserName(userName);
        }

    }
}
