namespace AsDI.DbExtend
{
    public interface IDbExecutor : IDisposable
    {
        /// <summary>
        /// 开启事务
        /// </summary>
        /// <returns>是否开启成功（true 开启成功，false 已被开启）</returns>
        bool BeginTrans();

        void Commit();

        void RollBack();

        int Modify(string sql, IDictionary<string, object> ps);


        object Query(string sql, IDictionary<string, object> parameters, Type returnType);

        T Query<T>(string sql, IDictionary<string, object> parameters);

        IEnumerable<T> QueryList<T>(string sql, IDictionary<string, object> parameters);

    }
}
