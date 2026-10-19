using Domain.Entities.ClientProfileManager;
using NTTData.AMOPortal.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Abstract
{
    public interface IUnitOfWorks : IDisposable
    {
        #region "Database Tables" 

        DbContext Context { get; set; }

        /// <summary>
        /// Property to return Mapping Template Table.
        /// </summary>
        IGenericRepository<MappingTemplate> MappingTemplate { get; }

        IGenericRepository<StoreApplication> StoreApplication { get; }

        IGenericRepository<AppStoreApplications> AppStoreApplications { get; }

        IGenericRepository<UserProfile> UserProfile { get; }

        IGenericRepository<SourceDataMapping> SourceDataMapping { get; }

        IGenericRepository<SourceFieldConcat> SourceFieldConcat { get; }

        IGenericRepository<Client> Clients { get; }

        IGenericRepository<JobDetail> JobDetails { get; }

        #endregion "Database Tables"

        void Save();

        DataTable GetDataFromStoredProcedure(string StoredProcedureName, string[] ParametersName, params object[] ParametersValues);
    }
}
