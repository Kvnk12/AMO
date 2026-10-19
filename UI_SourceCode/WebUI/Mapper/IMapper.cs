using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTTData.AMOPortal.WebUI.Mapper
{
    public interface IMapper<TEntity, TInput>
        where TEntity : class, new()
        where TInput : new()
    {
        TInput MapToInput(TEntity entity);
        TEntity MapToEntity(TInput input, TEntity entity);
    }
}
