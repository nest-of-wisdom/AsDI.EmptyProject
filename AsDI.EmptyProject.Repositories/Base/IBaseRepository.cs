using AsDI.EmptyProject.Utils.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AsDI.EmptyProject.Repositories.Base
{
    public interface IBaseRepository<TModel> where TModel : class, new()
    {

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="model">数据</param>
        /// <returns></returns>
        int Insert(TModel model);

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="model">数据</param>
        /// <returns></returns>
        int Update(TModel model);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="model">数据</param>
        /// <returns></returns>
        int Delete(TModel model);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="filterExpression">删除条件</param>
        /// <returns></returns>
        int Delete(Expression<Func<TModel, bool>> filterExpression);

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="filterExpression">获取条件</param>
        /// <returns></returns>
        IEnumerable<TModel> GetList(Expression<Func<TModel, bool>> filterExpression);

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="filterExpression">获取条件</param>
        /// <param name="orderBy">排序</param>
        /// <returns></returns>
        IEnumerable<TModel> GetList<TKey>(Expression<Func<TModel, bool>> filterExpression, Expression<Func<TModel, TKey>> orderBy, bool isDesc = false);

        /// <summary>
        /// 获取数据（分页）
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="filterExpression">获取条件</param>
        /// <param name="orderBy">分页排序</param>
        /// <param name="page">页码</param>
        /// <param name="recordsPrePage">每页数量</param>
        /// <returns></returns>
        PagedList<TModel> GetList<TKey>(Expression<Func<TModel, bool>> filterExpression, Expression<Func<TModel, TKey>> orderBy, int page, int recordsPrePage, bool isDesc = false);

        /// <summary>
        /// 查找数据
        /// </summary>
        /// <param name="condition">查找条件</param>
        /// <returns></returns>
        IEnumerable<TModel> Search(SqlCondition<TModel> condition);

        /// <summary>
        /// 查找数据
        /// </summary>
        /// <typeparam name="TKey">排序字段类型</typeparam>
        /// <param name="condition">查找条件</param>
        /// <param name="orderBy">排序字段</param>
        /// <param name="isDesc">是否按倒序排列</param>
        /// <returns></returns>
        IEnumerable<TModel> Search<TKey>(SqlCondition<TModel> condition, Expression<Func<TModel, TKey>> orderBy, bool isDesc = false);

        /// <summary>
        /// 查找数据
        /// </summary>
        /// <typeparam name="TKey">排序字段类型</typeparam>
        /// <param name="condition">查找条件</param>
        /// <param name="orderBy">排序字段</param>
        /// <param name="page">页码</param>
        /// <param name="recordsPrePage">每页行数</param>
        /// <param name="isDesc">是否按倒序排列</param>
        /// <returns></returns>
        PagedList<TModel> Search<TKey>(SqlCondition<TModel> condition, Expression<Func<TModel, TKey>> orderBy, int page, int pageSize, bool isDesc = false);


        /// <summary>
        /// 获取单条数据
        /// </summary>
        /// <param name="filterExpression">获取条件</param>
        /// <returns></returns>
        TModel GetSingle(Expression<Func<TModel, bool>> filterExpression);

        /// <summary>
        /// 获取单条数据
        /// </summary>
        /// <param name="filterExpression">获取条件</param>
        /// <param name="orderBy">排序方式</param>
        /// <returns></returns>
        TModel GetSingle<TKey>(Expression<Func<TModel, bool>> filterExpression, Expression<Func<TModel, TKey>> orderBy);


        /// <summary>
        /// 用SQL查询
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="parameters">查询参数</param>
        /// <param name="page">页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <returns></returns>
        PagedList<TModel> Query(string condition, Dictionary<string, object> parameters, int page, int pageSize);

    }
}
