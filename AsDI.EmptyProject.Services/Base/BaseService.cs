using AsDI.Attributes;
using AsDI.EmptyProject.IServices.Base;
using AsDI.EmptyProject.Repositories.Base;
using AsDI.EmptyProject.Utils;
using Mapster;

namespace AsDI.EmptyProject.Services.Base
{
    public abstract class BaseService<Dto, Model> : IBaseService<Dto> where Dto : BaseDTO, new()
        where Model : BaseEntity, new()
    {
        [AutoAssemble]
        protected SnowflakeIdGenerator snowflake;

        public abstract IBaseRepository<Model> BaseRepository { get; }

        public virtual int Delete(string id)
        {
            return BaseRepository.Delete(p => p.Id == id);
        }

        public virtual IEnumerable<Dto> GetAll()
        {
            var data = BaseRepository.GetList(p => p.IsDeleted == false);
            return data.Adapt<List<Dto>>();
        }

        public virtual Dto GetSingle(string id)
        {
            var item = BaseRepository.GetSingle(p => p.Id == id);
            return item.Adapt<Dto>();
        }

        public virtual Dto Insert(Dto dto)
        {
            dto.IsDeleted = false;
            dto.RowVersion = 1;
            dto.CreatedBy = "";
            dto.CreatedTime = DateTime.Now;
            dto.ModifiedBy = "";
            dto.ModifiedTime = DateTime.Now;
            dto.Id = snowflake.Next();

            var item = dto.Adapt<Model>();

            int rtn = BaseRepository.Insert(item);

            if (rtn == 1)
            {
                return dto;
            }
            else
            {
                throw new Exception("Insert Failure");
            }

        }

        public virtual Dto Update(Dto dto)
        {
            dto.ModifiedBy = "";
            dto.ModifiedTime = DateTime.Now;

            var item = dto.Adapt<Model>();
            int rtn = BaseRepository.Update(item);
            if (rtn == 1)
            {
                return dto;
            }
            else
            {
                throw new Exception("Update Failure");
            }

        }
    }
}
