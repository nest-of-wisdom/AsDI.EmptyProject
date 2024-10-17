using AsDI.EmptyProject.Repositories.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsDI.EmptyProject.Repositories
{
    public partial class DbEntities
    {
        public DbSet<UserEntity> Users { get; set; }
    }
}
