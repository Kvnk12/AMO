using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NTTData.AMOPortal.Domain.Entities;
using NTTData.AMOPortal.Domain.Service;
using NTTData.AMOPortal.WebUI.Dto;
using NTTData.AMOPortal.WebUI.Mapper;

namespace NTTData.AMOPortal.WebUI.Controllers
{
    /// <summary>
    /// generic crud controller for entities where there is no difference between the edit and create view
    /// </summary>
    /// <typeparam name="TEntity">the entity</typeparam>
    /// <typeparam name="TInput"> viewmodel </typeparam>
    public abstract class Cruder<TEntity, TInput> : Crudere<TEntity, TInput, TInput>
        where TInput : Input, new()
        where TEntity : DelEntity, new()
    {
        public Cruder(ICrudService<TEntity> service, IMapper<TEntity, TInput> v)
            : base(service, v, v)
        {
        }

        protected override string EditView
        {
            get { 
                return "create"; 
            }
        }
    }
}