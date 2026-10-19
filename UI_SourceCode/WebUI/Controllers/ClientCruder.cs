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
    public abstract class ClientCruder<TEntity, TInput, TAEntity, TAInput,TEEntity,TEInput> : ClientCrudere<TEntity, TInput, TInput, TAEntity, TAInput, TAInput,TEEntity, TEInput, TEInput>
        where TInput : Input, new()
        where TEntity : DelEntity, new()
        where TAInput : Input, new()
        where TAEntity : DelEntity, new()
         where TEInput : Input, new()
        where TEEntity : DelEntity, new()
    {
        public ClientCruder(ICrudService<TEntity> service, IMapper<TEntity, TInput> v,
            ICrudService<TAEntity> Addressservice, IMapper<TAEntity, TAInput> AddressMap,
            ICrudService<TEEntity> Engagementservice, IMapper<TEEntity, TEInput> EngagementMap)
            : base(service, v, v, Addressservice, AddressMap, AddressMap, Engagementservice,EngagementMap, EngagementMap)
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