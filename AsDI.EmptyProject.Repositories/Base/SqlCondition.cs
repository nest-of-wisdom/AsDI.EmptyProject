using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AsDI.EmptyProject.Repositories.Base
{
    public class SqlCondition<TModel>
    {
        private readonly List<Expression<Func<TModel, bool>>> conditions;

        private SqlCondition()
        {
            conditions = new List<Expression<Func<TModel, bool>>>();
        }

        public static SqlCondition<TModel> Create()
        {
            return new SqlCondition<TModel>();
        }

        public SqlCondition<TModel> Where(Expression<Func<TModel, bool>> condition)
        {
            this.conditions.Add(condition);
            return this;
        }

        public IQueryable<TModel> Filter(IQueryable<TModel> data)
        {
            foreach (var condition in conditions)
            {
                data = data.Where(condition);
            }
            return data;
        }

    }
}
