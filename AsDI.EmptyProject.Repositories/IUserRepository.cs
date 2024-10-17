using AsDI.DbExtend.Attributes;
using AsDI.EmptyProject.Repositories.Base;
using AsDI.EmptyProject.Repositories.Entities;

namespace AsDI.EmptyProject.Repositories
{

    /// <summary>
    /// 带有[Repository]标识的接口，不需要实现，只需要定义好方法的[NactiveExecute即可
    /// 如果需要想手动实现部分功能，又不想实现已经注释的方法，有两种方式：
    /// 1、直接实现IUserRepository，但对于带有[NactiveExecute]的方法，直接Throw New NotImplementedException()即可
    /// 2、写一个扩展接口，如IUserExtendRepository，然后实现它（记住在实现的类型上标注[Service]），然后当前接口继承扩展接口即可
    /// </summary>
    [Repository]
    public interface IUserRepository : IBaseRepository<UserEntity>
    {

        /// <summary>
        /// 通过NactiveExecute可以进行SQL执行，默认SQL为查询SQL
        /// </summary>
        /// <param name="userName">参数</param>
        /// <returns></returns>
        [NactiveExecute("select * from sys_user where UserName=@userName")]
        UserEntity FindUser(string userName);

        /// <summary>
        /// 通过NativeExecute执行更新SQL，需要设置SqlType为SqlType.DML，表示当前SQL数据管理语句
        /// </summary>
        /// <param name="userName">参数</param>
        /// <returns></returns>
        [NactiveExecute("update sys_user set Status=Status+1 where UserName=@userName", SqlType = SqlType.DML)]
        int UpdateStatus(string userName);

    }
}
