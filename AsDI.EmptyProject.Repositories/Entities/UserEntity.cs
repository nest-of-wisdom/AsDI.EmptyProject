using AsDI.EmptyProject.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsDI.EmptyProject.Repositories.Entities
{
    [Table("sys_user")]
    public class UserEntity : BaseEntity
    {
        [Comment("用户名"), MaxLength(50), Required]
        public string UserName { get; set; } = "";

        [Comment("性别")]
        public int Gender { get; set; }

        [Comment("密码"), MaxLength(100)]
        public string Password { get; set; }

        [Comment("Email"), MaxLength(200), EmailAddress]
        public string Email { get; set; } = "";

        [Comment("部门"), MaxLength(100)]
        public string Department { get; set; } = "";

        [Comment("岗位"), MaxLength(100)]
        public string Position { get; set; } = "";

    }
}
