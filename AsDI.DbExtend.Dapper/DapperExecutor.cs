using AsDI.Attributes;
using AsDI.Core.Config;
using Dapper;
using System.Data;

namespace AsDI.DbExtend.Dapper
{
    [Service]
    public class DapperExecutor : IDbExecutor
    {
        private static readonly AsyncLocal<IDbConnection> conn = new();
        private static readonly AsyncLocal<IDbTransaction> transaction = new();

        private readonly string connStr;

        private readonly Type provider;

        public DapperExecutor(
            [Value("AsDI.DataBase.ConnectionString")] string connectionString,
            [Value("AsDI.DataBase.Provider")] string providerName
            )
        {
            connStr = connectionString;
            var type = Type.GetType(providerName);
            if (type == null)
            {
                throw new Exception("Provider not found");
            }
            provider = type;
        }

        public IDbConnection Connection
        {
            get
            {
                if (conn.Value == null)
                {
                    conn.Value = Activator.CreateInstance(provider, connStr) as IDbConnection;
                    if (conn.Value == null)
                    {
                        throw new Exception("Failed to create a connection instance.");
                    }
                }
                return conn.Value;
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
                Connection.Open();
                transaction.Value = Connection.BeginTransaction();
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

        public void Dispose()
        {
            if (transaction.Value == null && conn.Value != null)
            {
                if (conn.Value?.State != ConnectionState.Closed)
                {
                    conn.Value?.Close();
                }
                conn.Value.Dispose();
                transaction.Value = null;
            }
        }

        public int Modify(string sql, IDictionary<string, object> ps)
        {
            return Connection.Execute(sql, ps, transaction.Value);
        }

        public object Query(string sql, IDictionary<string, object> parameters, Type returnType)
        {
            RawType rawType = RawType.From(returnType);

            if (rawType.IsList)
            {
                return Connection.Query(returnType, sql, parameters, transaction.Value);
            }
            else if (rawType.IsArray)
            {
                var rtn = Connection.Query(returnType, sql, parameters, transaction.Value);
                var real = Array.CreateInstance(rawType.ElementType ?? typeof(object), rtn.Count());
                int i = 0;
                foreach (var v in rtn)
                {
                    real.SetValue(v, i);
                    i++;
                }
                return real;
            }
            else
            {
                var value = Connection.QueryFirstOrDefault(returnType, sql, parameters, transaction.Value);
                return value;
            }

        }

        public T Query<T>(string sql, IDictionary<string, object> parameters)
        {
            return (T)Query(sql, parameters, typeof(T));
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
                var eleType = type.GetElementType();
                if (IsPrimitive(eleType))
                {
                    rtn.IsPrimitiveType = true;
                    rtn.ElementType = type;
                }
                else
                {
                    rtn.IsArray = type.IsArray;
                    rtn.ElementType = type.GetElementType();
                }
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
