using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsDI.EmptyProject.IServices.Base
{
    public class BaseDTO
    {

        /// <summary>
        /// 主键
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; } = "system";

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreatedTime { get; set; }

        /// <summary>
        /// 最后更新人
        /// </summary>
        public string ModifiedBy { get; set; } = "system";

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime? ModifiedTime { get; set; }

        /// <summary>
        /// 乐观锁
        /// </summary>
        public long RowVersion { get; set; } = 1;

        /// <summary>
        /// 已删除
        /// </summary>
        public bool IsDeleted { get; set; } = false;

        /// <summary>
        /// 状态
        /// </summary>
        public int? Status { get; set; }
    }
}
