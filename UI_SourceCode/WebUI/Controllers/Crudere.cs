using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using NTTData.AMOPortal.Domain.Entities;
using NTTData.AMOPortal.Domain.Service;
using NTTData.AMOPortal.WebUI.Dto;
using NTTData.AMOPortal.WebUI.Mapper;

namespace NTTData.AMOPortal.WebUI.Controllers
{
    /// <summary>
    /// generic crud controller for entities where there is difference between the edit and create view
    /// </summary>
    /// <typeparam name="TEntity"> the entity</typeparam>
    /// <typeparam name="TCreateInput">create viewmodel</typeparam>
    /// <typeparam name="TEditInput">edit viewmodel</typeparam>
    public abstract class Crudere<TEntity, TCreateInput, TEditInput> : BaseController
        where TCreateInput : new()
        where TEditInput : Input, new()
        where TEntity : DelEntity, new()
    {
        protected readonly ICrudService<TEntity> service;
        protected readonly IMapper<TEntity, TCreateInput> createMapper;
        protected readonly IMapper<TEntity, TEditInput> editMapper;

        protected virtual string EditView
        {
            get { return "edit"; }
        }

        public Crudere(ICrudService<TEntity> service, IMapper<TEntity, TCreateInput> createMapper, IMapper<TEntity, TEditInput> editMapper)
        {
            this.service = service;
            this.createMapper = createMapper;
            this.editMapper = editMapper;
        }

        public virtual ActionResult Index()
        {
            IEnumerable<TEntity> entities = service.GetAll();
            var onlyNonDeleted = from p in entities where (!p.IsDeleted) select p;
            return View(onlyNonDeleted);
        }

        public ActionResult Create()
        {
            return View(createMapper.MapToInput(new TEntity()));
        }

        [HttpPost]
        public virtual ActionResult Create(TCreateInput input)
        {
            if (!ModelState.IsValid)
                return View(input);
            var id = service.Create(createMapper.MapToEntity(input, new TEntity()));
            var e = service.Get(id);
            return RedirectToAction("Index");
            
        }

        [OutputCache(Location = OutputCacheLocation.None)]//for ie
        public virtual ActionResult Edit(int id)
        {
            var entity = service.Get(id);
                        
           return View(EditView, editMapper.MapToInput(entity));
        }

        [HttpPost]
        public virtual ActionResult Edit(TEditInput input)
        {
            if (!ModelState.IsValid)
                return View(EditView, input);
            var e = editMapper.MapToEntity(input, service.Get(input.Id));
            service.Save();
            
            return RedirectToAction("Index");
            
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            service.Delete(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Restore(int id)
        {
            service.Restore(id);
            return (View());
            
        }

    }
}