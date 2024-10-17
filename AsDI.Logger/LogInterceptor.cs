//using AsDI.Attributes;
//using AsDI.Interceptor;

//namespace AsDI.Logger
//{
//    [Fit("!attr('AsDI.+NoLogAttribute')",999)]
//    public class LogInterceptor : IInterceptor
//    {
//        public object Around(AspectEntity aspect)
//        {
//            var logInfo = new LogInfo(aspect);
//            Logger.LogStart(logInfo);
//            try
//            {
//                var value = aspect.Continue();
//                Logger.LogEnd(value);
//                return value;
//            }
//            catch (Exception ex)
//            {
//                Logger.LogEnd(ex);
//                throw;
//            }
//        }
//    }
//}
