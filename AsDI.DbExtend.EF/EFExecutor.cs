using AsDI.Attributes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections;
using System.Reflection;
using System.Text;

namespace AsDI.DbExtend.EF
{
    [Service]
    public class EFExecutor : IDbExecutor
    {

        private static readonly AsyncLocal<DbContext> context = new();

        private static readonly AsyncLocal<IDbContextTransaction> transaction = new();

        private readonly MethodInfo method;

        public EFExecutor()
        {
            method = typeof(EFExecutor).GetMethod("QueryList");
        }

        public static DbContext Context
        {
            get
            {
                if (context.Value == null)
                {
                    lock (context)
                    {
                        context.Value ??= AsDIContext.New<DbContext>();
                    }
                }
                return context.Value;
            }
        }

        public bool BeginTrans()
        {
            if (transaction.Value != null)
            {
                return false;
            }
            else
            {
                transaction.Value = Context.Database.BeginTransaction();
                return true;
            }
        }

        public void Commit()
        {
            transaction.Value?.Commit();
            transaction.Value = null;
        }
        public void RollBack()
        {
            transaction.Value?.Rollback();
            transaction.Value = null;
        }

        public int Modify(string sql, IDictionary<string, object> ps)
        {
            var rawSql = RawSql.Parse(sql, ps);
            return Context.Database.ExecuteSqlRaw(rawSql.Sql, rawSql.Parameters);
        }

        public object Query(string sql, IDictionary<string, object> parameters, Type returnType)
        {
            RawType rawType = RawType.From(returnType);

            var rtn = method?.MakeGenericMethod(rawType.ElementType ?? typeof(object)).Invoke(this, [sql, parameters]);
            if (rtn == null)
            {
                return null;
            }

            if (rawType.IsList)
            {
                return rtn;
            }
            else if (rawType.IsArray)
            {
                var el = rtn as IList;
                var real = Array.CreateInstance(rawType.ElementType ?? typeof(object), el?.Count ?? 0);
                int i = 0;
                foreach (var v in el)
                {
                    real.SetValue(v, i);
                    i++;
                }
                return real;
            }
            else
            {
                var list = rtn as IList;
                if (list?.Count > 0)
                {
                    return list[0];
                }
                else
                {
                    return null;
                }
            }
        }

        public IEnumerable<T> QueryList<T>(string sql, IDictionary<string, object> parameters)
        {
            var rawSql = RawSql.Parse(sql, parameters);
            return Context.Database.SqlQueryRaw<T>(rawSql.Sql, rawSql.Parameters).AsQueryable().ToList();
        }

        public void Dispose()
        {
            if (transaction.Value == null && context.Value != null)
            {
                context.Value.Dispose();
                context.Value = null;
            }
        }

    }

    internal class RawSql
    {

        public string Sql { get; set; } = "";

        public object[] Parameters { get; set; } = [];

        public static RawSql Parse(string sql, IDictionary<string, object> ps)
        {
            RawSql rtn = new RawSql();

            StringBuilder builder = new(sql);
            object[] args = new object[ps.Count];
            int i = 0;

            foreach (var item in ps)
            {
                builder.Replace("@" + item.Key, "{" + i + "}");
                builder.Replace(":" + item.Key, "{" + i + "}");
                args[i] = item.Value;
                i++;
            }

            rtn.Sql = builder.ToString();
            rtn.Parameters = args;
            return rtn;

        }
    }

    internal class RawType
    {

        public bool IsList { get; set; }

        public bool IsPrimitiveType { get; set; }

        public Type ElementType { get; set; }

        public bool IsArray { get; set; }

        public static RawType From(Type type)
        {
            if (type == typeof(void))
            {
                throw new InvalidOperationException();
            }

            var rtn = new RawType();
            if (type.IsGenericType)
            {
                var typeDefind = type.GetGenericTypeDefinition();
                if (typeDefind == typeof(List<>)
                    || typeDefind == typeof(IList<>)
                    || typeDefind == typeof(IReadOnlyList<>)
                    || typeDefind == typeof(ICollection<>)
                    || typeDefind == typeof(IEnumerable<>)
                    || typeDefind == typeof(IReadOnlyCollection<>))
                {
                    rtn.IsList = true;
                    rtn.ElementType = type.GenericTypeArguments[0];
                }
                else if (typeDefind == typeof(Nullable<>))
                {
                    var real = type.GenericTypeArguments.First();
                    if (IsPrimitive(real))
                    {
                        rtn.IsPrimitiveType = true;
                        rtn.ElementType = type;
                    }
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
            else if (type.IsArray)
            {
                rtn.IsArray = type.IsArray;
                rtn.ElementType = type.GetElementType();
            }
            else if (IsPrimitive(type))
            {
                rtn.IsPrimitiveType = true;
                rtn.ElementType = type;
            }
            else
            {
                rtn.ElementType = type;
            }

            return rtn;

        }

        private static bool IsPrimitive(Type type)
        {
            return type.IsPrimitive || type.IsEnum || type.IsValueType;
        }

    }
}
