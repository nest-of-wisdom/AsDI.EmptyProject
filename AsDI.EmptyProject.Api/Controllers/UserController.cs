using AsDI.EmptyProject.IServices;
using AsDI.EmptyProject.IServices.Dtos;
using AsDI.EmptyProject.Utils.Models;
using Microsoft.AspNetCore.Mvc;

namespace AsDI.EmptyProject.Api.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        public const string PREFIX = "/User";

        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet(PREFIX + "/query")]
        public PagedList<UserDTO> Query(string? keyword, int page, int pageSize)
        {
            return userService.Query(keyword, page, pageSize);
        }

        [HttpGet(PREFIX + "/get")]
        public UserDTO Get(string id)
        {
            return userService.GetSingle(id);
        }

        [HttpPost(PREFIX + "/add")]
        public UserDTO Add(UserDTO user)
        {
            return userService.Insert(user);
        }

        [HttpPost(PREFIX + "/update")]
        public UserDTO Update(UserDTO update)
        {
            return userService.Update(update);
        }

        [HttpDelete(PREFIX + "/delete")]
        public int Delete(string id)
        {
            return userService.Delete(id);
        }

        [HttpGet(PREFIX + "/getByUserName")]
        public UserDTO GetByUserName(string userName)
        {
            return userService.GetByUserName(userName);
        }

    }
}
