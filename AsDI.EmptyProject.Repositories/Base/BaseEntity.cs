using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace AsDI.EmptyProject.Repositories.Base
{
    public class BaseEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key, Comment("主键"), MaxLength(50)]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// 创建人
        /// </summary>
        [MaxLength(50), Comment("创建人")]
        public string CreatedBy { get; set; } = "system";

        /// <summary>
        /// 创建时间
        /// </summary>
        [Comment("创建时间")]
        public DateTime? CreatedTime { get; set; }

        /// <summary>
        /// 最后更新人
        /// </summary>
        [MaxLength(50), Comment("更新人")]
        public string ModifiedBy { get; set; } = "system";

        /// <summary>
        /// 最后更新时间
        /// </summary>
        [Comment("更新时间")]
        public DateTime? ModifiedTime { get; set; }

        /// <summary>
        /// 乐观锁
        /// </summary>
        [Comment("乐观锁")]
        public long RowVersion { get; set; } = 1;

        /// <summary>
        /// 已删除
        /// </summary>
        [Comment("已删除")]
        public bool IsDeleted { get; set; } = false;

        /// <summary>
        /// 状态
        /// </summary>
        [Comment("状态")]
        public int? Status { get; set; }


    }
}
