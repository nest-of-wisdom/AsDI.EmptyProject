using AsDI.Attributes;
using AsDI.DbExtend.Attributes;
using AsDI.Interceptor;


namespace AsDI.DbExtend
{
    [Fit("true", 100)]
    public class TransInterceptor : IInterceptor
    {

        private readonly IDbExecutor nativeExecutor;

        public TransInterceptor(IDbExecutor executor)
        {
            this.nativeExecutor = executor;
        }

        public object Around(AspectEntity aspect)
        {
            var analyzer = aspect.Method.GetTargetAnalyzer(aspect.Target);
            var trans = analyzer.FindAttribute<AutoTransAttribute>();
            bool transBegin = false;
            if (trans != null)
            {
                transBegin = nativeExecutor.BeginTrans();
            }
            try
            {
                return aspect.Continue();
            }
            catch (Exception ex)
            {
                bool rollBack = trans != null && trans.AutoRollBack(ex);
                if (rollBack)
                {
                    nativeExecutor.RollBack();
                }
                throw;
            }
            finally
            {
                if (trans != null && transBegin)
                {
                    nativeExecutor.Commit();
                }
            }

        }
    }
}