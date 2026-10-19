using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Omu.ValueInjecter;

namespace NTTData.AMOPortal.WebUI.Mapper
{
    public class Mapper<TEntity, TInput> : IMapper<TEntity, TInput>
        where TEntity : class, new()
        where TInput : new()
    {
        public virtual TInput MapToInput(TEntity entity)
        {
            var input = new TInput();
            //Added by Anandhi for Client Profile Begin
            if (entity != null)
            {
                //Added by Anandhi for Client Profile End
                input.InjectFrom(entity)
                    .InjectFrom<NormalToNullables>(entity)
                    .InjectFrom<EntitiesToInts>(entity);
                //Added by Anandhi for Client Profile Begin
            }
            //Added by Anandhi for Client Profile End
            return input;
        }

        public virtual TEntity MapToEntity(TInput input, TEntity e)
        {
             //Added by Anandhi for Client Profile Begin
            if ((input != null) && (e != null))
            {
           //Added by Anandhi for Client Profile End
            e.InjectFrom(input)
               .InjectFrom<IntsToEntities>(input)
               .InjectFrom<NullablesToNormal>(input);
                //Added by Anandhi for Client Profile Begin
            }
            //Added by Anandhi for Client Profile Begin
            return e;
        }
    }
}