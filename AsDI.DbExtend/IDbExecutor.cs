namespace AsDI.DbExtend
{
    /// <summary>
    /// 数据库执行器
    /// </summary>
    public interface IDbExecutor : IDisposable
    {
        /// <summary>
        /// 开启事务
        /// </summary>
        /// <returns>是否开启成功（true 开启成功，false 已被开启）</returns>
        bool BeginTrans();

        /// <summary>
        /// 提交事务
        /// </summary>
        void Commit();

        /// <summary>
        /// 回滚事务
        /// </summary>
        void RollBack();

        /// <summary>
        /// 更新、删除、插入数据
        /// </summary>
        /// <param name="sql">执行的SQL</param>
        /// <param name="ps">执行的参数</param>
        /// <returns></returns>
        int Modify(string sql, IDictionary<string, object> ps);

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <param name="returnType"></param>
        /// <returns></returns>
        object Query(string sql, IDictionary<string, object> parameters, Type returnType);

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        T Query<T>(string sql, IDictionary<string, object> parameters);

    }
}
