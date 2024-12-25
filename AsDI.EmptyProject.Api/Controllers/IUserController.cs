using AsDI.Core.Web.Rest;
using AsDI.EmptyProject.IServices.Dtos;
using AsDI.EmptyProject.Utils.Models;
using Microsoft.AspNetCore.Mvc;

namespace AsDI.EmptyProject.Api.Controllers
{
    [RestApi("/user")]
    public interface IUserController
    {
        [HttpGet("/query")]
        PagedList<UserDTO> Query(string? keyword, int page, int pageSize);

        [HttpGet("/get")]
        UserDTO Get(string id);

        [HttpPost("/add")]
        UserDTO Add(UserDTO user);

        [HttpPost("/update")]
        UserDTO Update(UserDTO update);

        [HttpDelete("/delete")]
        int Delete(string id);

        [HttpGet("/getByUserName")]
        UserDTO GetByUserName(string userName);

    }
}
