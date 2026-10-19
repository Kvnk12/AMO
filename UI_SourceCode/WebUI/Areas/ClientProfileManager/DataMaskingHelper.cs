using Data;
using NTTData.AMOPortal.Data;
using NTTData.AMOPortal.Domain.Entities;
using NTTData.AMOPortal.WebUI.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebUI.Areas.ClientProfileManager.Dto;

namespace WebUI.Areas.ClientProfileManager
{
    public class DataMaskingHelper
    {
        // public static IList<PIDataMaskListModel> GetAllMaskDetails(int ClientId)
        public static IList<PIDataMaskListModel> GetAllMaskDetails()
        {
            IList<PIDataMaskListModel> Mask = new List<PIDataMaskListModel>();
            var entityMapper = new Mapper<PIDataMaskListModel, PIDataMaskList>();

            using (var contextFactory = new DbContextFactory())
            {
                IEnumerable<PIDataMaskList> MaskDataDetails = null;
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                //if (ClientId != null && ClientId != 0)
                // MaskDataDetails = unitOfWork.PIDataMaskList.Get(cid => cid.ClientId == ClientId).ToList();
                MaskDataDetails = unitOfWork.PIDataMaskList.Get().ToList();

                if (MaskDataDetails != null)
                {
                    foreach (var _maskDataDetails in MaskDataDetails)
                    {
                        PIDataMaskListModel p = new PIDataMaskListModel();
                        p.Id = _maskDataDetails.Id;
                        //p.ClientId = _maskDataDetails.ClientId;
                        p.DataFieldName = _maskDataDetails.DataFieldName;
                        p.DataMaskPattern = _maskDataDetails.DataMaskPattern;
                        p.DataFieldDescription = _maskDataDetails.DataFieldDescription;
                        p.DataMaskExample = _maskDataDetails.DataMaskExample;
                        p.ReplacementValue = _maskDataDetails.ReplacementValue;
                        //p.UpdatedBy = _maskDataDetails.UpdatedBy;
                        p.CreatedBy = _maskDataDetails.CreatedBy;
                        //p.CreatedDate = _maskDataDetails.CreatedDate;
                        //p.UpdatedDate = _maskDataDetails.UpdatedDate;
                       
                        var c = unitOfWork.UserProfile.GetByID(p.CreatedBy);
                        if (c != null)
                        {
                            string ss = c.FirstName + " " + c.LastName;
                            p.createdbyName = ss;
                        }
                        Mask.Add(p);
                    }
                }
                return Mask;
            }

        }


        public static int AddData(PIDataMaskListModel datalist)
        {
            int id = 0;
            try
            {
                var entityMapper = new Mapper<PIDataMaskList, PIDataMaskListModel>();
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                        PIDataMaskList pidatamaskDetails = new PIDataMaskList();
                        entityMapper.MapToEntity(datalist, pidatamaskDetails);
                        unitOfWork.PIDataMaskList.Insert(pidatamaskDetails);
                        unitOfWork.Save();
                        id = pidatamaskDetails.Id;
                    
                }
            }

            catch (Exception)
            {
                id = 0;
            }
            return id;
        }


        public static bool UpdateData(PIDataMaskListModel datalist)
        {
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var pidatamaskDetails = unitOfWork.PIDataMaskList.GetByID(datalist.Id);
                    pidatamaskDetails.DataFieldName = datalist.DataFieldName;
                    pidatamaskDetails.DataMaskPattern = datalist.DataMaskPattern;
                    pidatamaskDetails.DataFieldDescription = datalist.DataFieldDescription;
                    pidatamaskDetails.DataMaskExample = datalist.DataMaskExample;
                    pidatamaskDetails.UpdatedBy = datalist.UpdatedBy;
                    pidatamaskDetails.UpdatedDate = datalist.UpdatedDate;
                    pidatamaskDetails.ReplacementValue = datalist.ReplacementValue;
                    unitOfWork.PIDataMaskList.Update(pidatamaskDetails);
                    unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool DeleteData(int id)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                unitOfWork.PIDataMaskList.Delete(id);
                unitOfWork.Save();

                //TempData["SLACreatedDeleted"] = "Deleted successfully.";
                return true;
            }
        }

    }
}