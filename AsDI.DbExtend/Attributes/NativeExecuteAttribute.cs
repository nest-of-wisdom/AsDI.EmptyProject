using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsDI.DbExtend.Attributes
{

    /// <summary>
    /// 原始数据查询定义
    /// </summary>
    [System.AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed class NactiveExecuteAttribute : Attribute
    {
        public NactiveExecuteAttribute(string sql)
        {
            this.Sql = sql;
        }

        public string Sql { get; private set; }

        public SqlType SqlType { get; set; } = SqlType.DQL;

    }

    public enum SqlType
    {
        /// <summary>
        /// 数据查询
        /// 包括Select
        /// </summary>
        DQL,

        /// <summary>
        /// 数据修改
        /// 包括Insert、Update、Delete等
        /// </summary>
        DML,

        /// <summary>
        /// 表结构变更
        /// 包括：Create、Alter、Drop等
        /// </summary>
        DDL,

        /// <summary>
        /// 数据库控制
        /// 包括授权、创建用户、备份等（与使用的数据库有关
        /// </summary>
        DCL
    }
}
