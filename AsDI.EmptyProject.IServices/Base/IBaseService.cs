using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsDI.EmptyProject.IServices.Base
{
    public interface IBaseService<Dto> where Dto : BaseDTO
    {
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>结果</returns>
        IEnumerable<Dto> GetAll();

        /// <summary>
        /// 获取单条信息
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns>结果</returns>
        Dto GetSingle(string id);

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="dto">原始数据</param>
        /// <returns>新数据</returns>
        Dto Insert(Dto dto);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="dto">原始数据</param>
        /// <returns>新数据</returns>
        Dto Update(Dto dto);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns>影响行数</returns>
        int Delete(string id);


    }
}
