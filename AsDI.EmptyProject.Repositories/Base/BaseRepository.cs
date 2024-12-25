using AsDI.Attributes;
using AsDI.DbExtend;
using AsDI.DbExtend.EF;
using AsDI.EmptyProject.Utils.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Mysqlx.Prepare;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using System.Reflection;

namespace AsDI.EmptyProject.Repositories.Base
{
    [Service(2)]
    public class BaseRepository<TModel> : IBaseRepository<TModel>
        where TModel : BaseEntity, new()
    {


        private readonly string tableName;


        public BaseRepository()
        {
            var type = typeof(TModel);

            var attr = type.GetCustomAttribute<TableAttribute>();

            if (attr != null)
            {
                tableName = attr.Name;
            }
            else
            {
                tableName = type.Name;
            }

        }


        [AutoAssemble]
        private IDbExecutor executor;



        private DbContext Db
        {
            get
            {
                return EFExecutor.Context;
            }
        }

        public virtual int Insert(TModel model)
        {
            Db.Set<TModel>().Add(model);
            var rtn = Db.SaveChanges();
            return rtn;
        }

        public virtual int Update(TModel model)
        {
            if (Db.Entry<TModel>(model).State == EntityState.Detached)
            {
                Db.Set<TModel>().Attach(model);
                Db.Entry<TModel>(model).State = EntityState.Modified;
            }
            var rtn = Db.SaveChanges();
            return rtn;
        }

        public virtual int Delete(TModel model)
        {
            Db.Set<TModel>().Remove(model);
            return Db.SaveChanges();
        }

        public virtual int Delete(Expression<Func<TModel, bool>> filterExpression)
        {
            var items = Db.Set<TModel>().Where(filterExpression);
            foreach (var item in items)
            {
                item.IsDeleted = true;
            }
            return Db.SaveChanges();
        }

        public virtual IEnumerable<TModel> GetList(Expression<Func<TModel, bool>> filterExpression)
        {
            return Db.Set<TModel>().Where(filterExpression).Where(p => p.IsDeleted == false).ToList();
        }

        public virtual PagedList<TModel> GetList<TKey>(Expression<Func<TModel, bool>> filterExpression, Expression<Func<TModel, TKey>> orderBy, int page, int pageSize, bool isDesc = false)
        {
            var paged = new PagedList<TModel>
            {
                Page = page,
                PageSize = pageSize
            };

            var skip = (page - 1) * pageSize;
            var data = Db.Set<TModel>().Where(filterExpression).Where(p => p.IsDeleted == false);

            paged.Total = data.Count();

            if (!isDesc)
            {
                data = data.OrderBy(orderBy);
            }
            else
            {
                data = data.OrderByDescending(orderBy);
            }
            var rtn = data.Skip(skip).Take(pageSize).ToList();
            if (rtn.Count == 0)
            {
                rtn = data.Take(pageSize).ToList();
                paged.Page = 1;
            }

            paged.Data = rtn;

            return paged;

        }

        public virtual TModel GetSingle(Expression<Func<TModel, bool>> filterExpression)
        {
            return Db.Set<TModel>().Where(filterExpression).Where(p => p.IsDeleted == false).OrderByDescending(p => p.Id).FirstOrDefault();
        }

        public virtual TModel GetSingle<TKey>(Expression<Func<TModel, bool>> filterExpression, Expression<Func<TModel, TKey>> orderBy)
        {
            return Db.Set<TModel>().Where(filterExpression).Where(p => p.IsDeleted == false).OrderBy(orderBy).FirstOrDefault();
        }

        public virtual IEnumerable<TModel> GetList<TKey>(Expression<Func<TModel, bool>> filterExpression, Expression<Func<TModel, TKey>> orderBy, bool isDesc = false)
        {
            var data = Db.Set<TModel>().Where(filterExpression).Where(p => p.IsDeleted == false);
            if (!isDesc)
            {
                data = data.OrderBy(orderBy);
            }
            else
            {
                data = data.OrderByDescending(orderBy);
            }
            return data.ToList();
        }

        public virtual IEnumerable<TModel> Search(SqlCondition<TModel> condition)
        {

            var data = Db.Set<TModel>().Where(p => p.IsDeleted == false);
            return condition.Filter(data).ToList();

        }

        public virtual PagedList<TModel> Search<TKey>(SqlCondition<TModel> condition, Expression<Func<TModel, TKey>> orderBy, int page, int pageSize, bool isDesc = false)
        {

            var paged = new PagedList<TModel>
            {
                Page = page,
                PageSize = pageSize
            };

            var skip = (page - 1) * pageSize;
            var data = condition.Filter(Db.Set<TModel>().Where(p => p.IsDeleted == false));
            paged.Total = data.Count();
            if (!isDesc)
            {
                data = data.OrderBy(orderBy);
            }
            else
            {
                data = data.OrderByDescending(orderBy);
            }
            var rtn = data.Skip(skip).Take(pageSize).ToList();
            if (rtn.Count == 0)
            {
                rtn = data.Take(pageSize).ToList();
                paged.Page = 1;
            }
            paged.Data = rtn;
            return paged;

        }

        public virtual IEnumerable<TModel> Search<TKey>(SqlCondition<TModel> condition, Expression<Func<TModel, TKey>> orderBy, bool isDesc = false)
        {

            var data = condition.Filter(Db.Set<TModel>().Where(p => p.IsDeleted == false));
            if (!isDesc)
            {
                data = data.OrderBy(orderBy);
            }
            else
            {
                data = data.OrderByDescending(orderBy);
            }
            var rtn = data.ToList();
            return rtn;

        }


        public PagedList<TModel> Query(string condition, Dictionary<string, object> parameters, int page, int pageSize)
        {
            var where = "";
            if (!string.IsNullOrWhiteSpace(condition))
            {
                where = " where " + condition;
            }

            var sql = "select count(*) from " + tableName + where;
            var total = executor.Query<int>(sql, parameters);

            sql = "select * from " + tableName + where + " " + ToPageSql(page, pageSize);

            var result = executor.Query<List<TModel>>(sql, parameters);

            return new PagedList<TModel>()
            {
                Data = result,
                Total = total,
                Page = page,
                PageSize = pageSize
            };

        }

        private string ToPageSql(int page, int pageSize)
        {

            var type = this.Db.Database.ProviderName;

            if (type.ToLower().Contains("mysql"))
            {
                return " LIMIT " + pageSize + " OFFSET " + ((page - 1) * pageSize);
            }
            else
            {
                return "OFFSET " + ((page - 1) * pageSize) + " ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
            }

        }
    }
}
