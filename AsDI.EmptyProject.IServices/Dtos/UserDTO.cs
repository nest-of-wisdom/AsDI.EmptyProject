using AsDI.EmptyProject.IServices.Base;

namespace AsDI.EmptyProject.IServices.Dtos
{
    public class UserDTO : BaseDTO
    {

        public string? UserName { get; set; }

        public int Gender { get; set; }

        public string? Password { get; set; }

        public string? Email { get; set; }

        public string? Department { get; set; }

        public string? Position { get; set; }

    }
}
