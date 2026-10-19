using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//Added by Anandhi for Client Profile Begin
using System.ComponentModel.DataAnnotations.Schema;
using NTTData.AMOPortal.Domain.Entities;
using NTTData.AMOPortal.Domain.Service;
using NTTData.AMOPortal.WebUI.Dto;
using NTTData.AMOPortal.WebUI.Mapper;
using NTTData.AMOPortal.Data;
using System.Globalization;
//Added by Raj for validation
using Foolproof;
using System.ComponentModel;
using PagedList;
using Data;
using WebUI.App_Start;
using WebUI.Models;
//using NTTData.AMOPortal.Resources;

namespace NTTData.AMOPortal.WebUI.Dto
{
    public class Input
    {
        public int Id { get; set; }
    }


    public class RegisterExternalLoginModel : Input
    {
        [Required]
        [Display(Name = "Email Address")]
        public string UserName { get; set; }

        public string ExternalLoginData { get; set; }
    }

    public class LocalPasswordModel : Input
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current Password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        [RegularExpression("^(?=.*[A-Z])(?=.*[!@#$&*])(?=.*[0-9])(?=.*[a-z]).{8,15}$", ErrorMessage = "Password requires alteast one upper and lower case alphanumeric and Special character and 8 to 15 characters long.")]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        //[Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
    //added by Raj 10/11/2017
    public class ResetPasswordModel : Input
    {


        [Required]
        [Display(Name = "Token")]
        public string Token { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        [RegularExpression("^(?=.*[A-Z])(?=.*[!@#$&*])(?=.*[0-9])(?=.*[a-z]).{8,15}$", ErrorMessage = "Password requires alteast one upper and lower case alphanumeric and Special character and 8 to 15 characters long.")]

        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        //[Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }


    public class ForgotPasswordModel : Input
    {
        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }


    }

    public class PasswordSuccessModel : Input
    {
    }
    public class LoginModel : Input
    {
        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }

        public string btnsignin { get; set; }
    }



    public class RegisterModel : Input
    {
        [Required]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid e-mail adress")]
        [Display(Name = "User Name")]
        [Remote("IsUserAlreadySigned", "Account", HttpMethod = "POST", ErrorMessage = "User name already exists, do you want to save this user to the client? Click 'Save' to continue.")]
        public string UserName { get; set; }

        // [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        [RegularExpression("^(?=.*[A-Z])(?=.*[!@#$&*])(?=.*[0-9])(?=.*[a-z]).{8,15}$", ErrorMessage = "Password requires alteast one upper-lower case alphanumeric, Special character and  8 to 15 characters long.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        // [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        //[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        //  [Display(Name = "Email")]
        //  [Required]
        //   [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid e-mail adress")]
        //   [Remote("IsAlreadySigned", "Account", HttpMethod = "POST", ErrorMessage = "EmailId already exists in database.")]
        [StringLength(50)]
        public string Email { get; set; }

        [Display(Name = "First Name")]
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Display(Name = "Job Title")]
        [StringLength(50)]
        public string JobTitle { get; set; }

        [Display(Name = "Phone")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Please enter a valid 10 digits phone number")]
        [StringLength(50)]
        public string Phone { get; set; }

        public int? CountryCode { get; set; }
        public IEnumerable<SelectListItem> CountryList
        {
            get
            {
                return Helper.GetCountriesList();
            }
        }

        [Display(Name = "Active")]
        //  [Required]
        [StringLength(50)]
        public string Active { get; set; }

        public string[] selectedRoles { get; set; }

        [Display(Name = "Reporting Manager")]
        //[Required]
        [StringLength(50)]
        public string ReportingManager { get; set; }

        public bool NTTDataResource { get; set; }

        public MultiSelectList GetRoles(string[] selectedRoles)
        {
            DbContextFactory db = new DbContextFactory();
            Repo<UserRole> Roles = new Data.Repo<UserRole>(db);
            List<UserRole> RoleList = Roles.GetAll().ToList();

            return new MultiSelectList(RoleList, "RoleName", "RoleName", selectedRoles);
        }

        public int? Createdby { get; set; }
        public DateTime? CreatedDt { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDt { get; set; }

        public MultiSelectList ListRoles { get; set; }

        public RoleModel userrole;

        public RegisterModel()
        {
            userrole = new RoleModel();
            ListRoles = GetRoles(null);
        }
    }

    public class RoleModel : Input
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }

    //added by Raj 10/11/2017
    public class webpages_Membership : Input
    {
        [Key]
        public int UserId { get; set; }
        public DateTime CreateDate { get; set; }
        public string ConfirmationToken { get; set; }
        public bool IsConfirmed { get; set; }
        public DateTime LastPasswordFailureDate { get; set; }
        public int PasswordFailuresSinceLastSuccess { get; set; }
        public string Password { get; set; }
        public DateTime PasswordChangeDate { get; set; }
        public string PasswordSalt { get; set; }
        public string PasswordVerificationToken { get; set; }
        public DateTime PasswordVerificationTokenExpirationDate { get; set; }
    }

    public class UserPasswordLogModel : Input
    {
        [Required]
        public int UserId { get; set; }
        public DateTime PasswordChangedDatetime { get; set; }
        public string IPAddress { get; set; }
    }

    public class UserProfileModel : Input
    {
        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        [RegularExpression("^(?=.*[A-Z])(?=.*[!@#$&*])(?=.*[0-9])(?=.*[a-z]).{8,15}$", ErrorMessage = "Password requires alteast one upper-lower case alphanumeric, Special character and 8 to 15 characters long.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [System.Web.Mvc.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Email")]
        //  [Required]
        //  [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid e-mail adress")]
        [StringLength(100)]
        public string Email { get; set; }

        [Display(Name = "First Name")]
        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        [Display(Name = "Job Title")]
        [StringLength(100)]
        public string JobTitle { get; set; }

        [Display(Name = "Phone")]
        //[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Please enter a valid phone number")]
        
        public string Phone { get; set; }
        public string OldPhone { get; set; }

        [Display(Name = "Active")]
        [Required]
        [StringLength(10)]
        public string Active { get; set; }

        [Display(Name = "Reporting Manager")]
        //[Required]
        [StringLength(50)]
        public string ReportingManager { get; set; }

     //   public int Id { get; set; }

        public int DefaultClientId { get; set; }
        public string NTTDataResourceString { get; set; }
        public bool NTTDataResource { get; set; }
        // public string Role { get; set; }

        public int? CountryCode { get; set; }
        public bool IsPhoneVerified { get; set; }

        public int? Createdby { get; set; }
        public DateTime? CreatedDt { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDt { get; set; }

        public IEnumerable<SelectListItem> CountryList
        {
            get
            {
                return Helper.GetCountriesList();
            }
        }
        public string[] selectedRoles { get; set; }

        // Modified by Neeraj to fix the user role
        public IEnumerable<SelectListItem> GetRoles(int[] selectedRoles)
        {
            DbContextFactory db = new DbContextFactory();
            Repo<UserRole> Roles = new Data.Repo<UserRole>(db);
            List<UserRole> RoleList = Roles.GetAll().ToList();

            var Items = RoleList.Select(Role => new SelectListItem
            {
                Value = Role.Id.ToString(),
                Text = Role.RoleName.ToString()

            }).ToList();

            if (!System.Web.Security.Roles.IsUserInRole("Sysadmin"))
            {
                Items.Remove((Items.Find(x => x.Text == "Sysadmin")));
            }
            return Items;
        }


        //Added by Ram for UserApplication Access

        public MultiSelectList GetUsers()
        {
            DbContextFactory db = new DbContextFactory();
            Repo<UserProfile> users = new Data.Repo<UserProfile>(db);
            List<UserProfile> usersList = users.GetAll().ToList();

            return new MultiSelectList(usersList, "id", "Username", UserNames);
        }

        public UserApplications UId { get; set; }
        public UserApplications AppId { get; set; }


        public string ApplicationNames { get; set; }

        public MultiSelectList ListApplications { get; set; }

        //   public IEnumerable<SelectListItem> ListselectedApplications { get; set; }

        //added by neeraj
        public IEnumerable<SelectListItem> ApplicationStoreApplications { get; set; }
        public IEnumerable<SelectListItem> GetAppStoreApplications()
        {
            DbContextFactory db = new DbContextFactory();
            Repo<AppStoreApplications> selApplications = new Data.Repo<AppStoreApplications>(db);
            List<AppStoreApplications> selApplicationsList = selApplications.GetAll().OrderBy(m => m.ApplicationName).ToList();
            var Items = selApplicationsList.Select(AppList => new SelectListItem
            {
                Value = AppList.Id.ToString(),
                Text = AppList.ApplicationName.ToString()
            });

            return Items;
        }
        //added by neeraj

        //modified by neeraj starts     
        public MultiSelectList GetApplications()
        {
            DbContextFactory db = new DbContextFactory();
            Repo<UserApplications> applications = new Data.Repo<UserApplications>(db);
            List<UserApplications> applicationsList = applications.GetAll().ToList();

            return new MultiSelectList(applicationsList, "id", "ApplicationName", ApplicationNames);
        }
        //modified by neeraj ends

        public int[] selectedApps { get; set; }

        public IEnumerable<SelectListItem> GetSelectedApps(int[] selectedApps)
        {
            DbContextFactory db = new DbContextFactory();
            Repo<UserApplications> selectedApplications = new Data.Repo<UserApplications>(db);
            List<UserApplications> selectedApplicationsList = selectedApplications.GetAll().ToList();
            var Items = selectedApplicationsList.Select(USAList => new SelectListItem
            {
                //Value = USAList.UserId.ToString(),
                Value = USAList.Id.ToString(),
                Text = USAList.ApplicationId.ToString()
            });

            return Items;
        }

        public string UserNames { get; set; }

        //public IEnumerable<SelectListItem> UsersList
        //{
        //    get
        //    {
        //        DbContextFactory Context = new DbContextFactory();
        //        Repo<NTTData.AMOPortal.Domain.Entities.UserProfile> Users = new Data.Repo<Domain.Entities.UserProfile>(Context);
        //        List<NTTData.AMOPortal.Domain.Entities.UserProfile> UsersList = Users.GetAll().OrderBy(m => m.UserName).ToList();
        //        var Items = UsersList.Select(UList => new SelectListItem
        //        {
        //            Value = UList.Id.ToString(),
        //            Text = UList.UserName.ToString()
        //        });
        //        return Items;

        //    }
        //}

        public UserApplicationsInput SelectedUserApplicationInput { get; set; }

        public List<UserApplicationsDto> SelectedAppsList
        {
            get; set;
            ////{
            ////    DbContextFactory Context = new DbContextFactory();
            ////    Repo<NTTData.AMOPortal.Domain.Entities.UserApplications> Apps = new Data.Repo<Domain.Entities.UserApplications>(Context);
            ////    List<NTTData.AMOPortal.Domain.Entities.UserApplications> SelAppsList = Apps.GetAll().Where(w=>w.UserId==this.Id).ToList();
            ////    //var Items = SelAppsList.Select(AList => new SelectListItem
            ////    //{
            ////    //    Value = AList.UserId.ToString(),
            ////    //    Text = AList.ApplicationId.ToString()
            ////    //});
            ////    //return Items;
            ////    return SelAppsList;

            ////}

        }

        public IEnumerable<SelectListItem> ListRoles { get; set; }

        public MultiSelectList ListUsers { get; set; }


        public RoleModel userrole;
        UserApplicationModel uam = new UserApplicationModel();
        public UserProfileModel()
        {
            userrole = new RoleModel();
            ListRoles = GetRoles(null);
           // ListUsers = GetUsers();
           // ListApplications = GetApplications();
            ApplicationStoreApplications = GetAppStoreApplications();
            // ListselectedApplications = GetSelectedApps(null);

        }
    }

    public class UserApplicationModel : Input
    {
        public int? UserId { get; set; }
        public int? ApplicationId { get; set; }
    }

    public class ExternalLogin : Input
    {
        public string Provider { get; set; }
        public string ProviderDisplayName { get; set; }
        public string ProviderUserId { get; set; }
    }

    //Added by Anandhi for  Client Profile Begin
    public class Address : Input
    {
        public int CountryId { get; set; }
        public int? CityId { get; set; }
        public int RegionId { get; set; }
    }

    public class Engagements : Input, IValidatableObject
    {
        [DataType(DataType.Date)]//mm/dd/YYYY
        public DateTime? EngagementStartDate { get; set; }

        [DataType(DataType.Date)]//mm/dd/YYYY
        public DateTime? EngagementEndDate { get; set; }

        //NUMERIC
        //public int? DurationOfRelationship { get; set; }
        public decimal DurationOfRelationship { get; set; } //Modified as per the requirement to show DurationOfRelationship in decimal

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {

            if (this.EngagementEndDate <= this.EngagementStartDate)
            {

                yield return new ValidationResult(string.Format("Engagement End Date Should be greater than {0:MM/dd/yyyy}", this.EngagementStartDate), new[] { "EngagementStartDate" });

            }

        }
    }


    public class ClientInput : Input
    {
        [Required(ErrorMessage = "Please enter a Client Name")]
        [DataType(DataType.Text)]
        public string ClientName { get; set; }
        [ForeignKey("PrimaryAddressId")]
        public Address PrimaryAddress { get; set; }
        [DataType(DataType.MultilineText)]
        [StringLength(4000, ErrorMessage = "Brief description of Client Business exceeds the maximum length")]
        public string BusinessDesc { get; set; }

        public int LoginUser { get; set; }

        [Required(ErrorMessage = "Please select Vertical ID")]
        public int VerticalId { get; set; }
        [Required(ErrorMessage = "Please select SubVertical ID")]
        public int SubVerticalId { get; set; }

        public int? Currency { get; set; }
        public string TotalContractValue { get; set; }
        public string TotalContractValueToDate { get; set; }
        public string AvgAnnualRevenue { get; set; }
        //public int EngagementId { get; set; }
        //[ForeignKey("EngagementId")]
        //public Engagements Engagement { get; set; }

        [DataType(DataType.Text)]
        public string GlobalClientManager { get; set; }

        [DataType(DataType.Text)]
        public string GlobalDeliveryManager { get; set; }

        [DataType(DataType.Text)]
        [StringLength(2000, ErrorMessage = "Client Title exceeds the maximum length")]
        public string ClientTitle { get; set; }
        public int PrimaryAddressId { get; set; }



        [Required(ErrorMessage = "Please select a Client Status")]
        public int ClientStatus { get; set; }
        public int? IsReferenceable { get; set; }
        public int? HasReferred { get; set; }

        [DataType(DataType.MultilineText)]
        [StringLength(265, ErrorMessage = "List of the potential clients referred Max Length is 265")]
        public string ReferredClientName { get; set; }

        [DataType(DataType.Date)]
        public DateTime? CustomerSurveyDate { get; set; }

        public int? CustomerRating { get; set; }

        public string AvgAnnualFTEs { get; set; }
        public string AvgAnnualFTEsOnShore { get; set; }
        public string AvgAnnualFTEsNearShore { get; set; }
        public string AvgAnnualFTEsOffShore { get; set; }

        [DataType(DataType.MultilineText)]
        [StringLength(4000, ErrorMessage = "Max Length is 4000")]
        public string Notes { get; set; }
        private DateTime _DateCreated = DateTime.Now;
        public DateTime DateCreated { get { return _DateCreated; } set { _DateCreated = value; } }
        public DateTime _DateUpdated = DateTime.Now;
        public DateTime DateUpdated { get { return _DateCreated; } set { _DateCreated = value; } }
        public string ClientNames { get; set; }
        public bool AutoCalculatedFTE { get; set; } // added by Neeraj
        public bool IsDeleted { get; set; } // added by Neeraj
        //private int loginUser;
        public bool SupportEcoSystem { get; set; }  // added by Raj
        public bool EnableMFA { get; set; }
        public bool MultipleApprovals { get; set; }
        public bool BusinessCalender { get; set; }
        public bool IMAutoApprovalSelection { get; set; }

        //public void ClientInput(int sessionLoginUserId)
        //{
        //    loginUser = sessionLoginUserId;
        //}

        public IEnumerable<SelectListItem> IndustryVerticalList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.Verticals> IndustryVerticals = new Data.Repo<Domain.Entities.Verticals>(Context);
                List<NTTData.AMOPortal.Domain.Entities.Verticals> IndustryVerticalsList = IndustryVerticals.GetAll().OrderBy(x => x.VerticalName).ToList();
                var Items = IndustryVerticalsList.Select(CList => new SelectListItem
                {
                    Value = CList.Id.ToString(),
                    Text = CList.VerticalName.ToString()
                }).ToList();
                Items.Add(new SelectListItem() { Selected = false, Text = "", Value = "0" });
                return Items;

            }

        }

        public IEnumerable<SelectListItem> IndustrySubVerticalList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.SubVerticals> IndustrySubVerticals = new Data.Repo<Domain.Entities.SubVerticals>(Context);
                List<NTTData.AMOPortal.Domain.Entities.SubVerticals> IndustrySubVerticalsList = IndustrySubVerticals.GetAll().OrderBy(x=>x.SubVerticalName).ToList();
                var Items = IndustrySubVerticalsList.Select(CList => new SelectListItem
                {
                    Value = CList.Id.ToString(),
                    Text = CList.SubVerticalName.ToString()
                }).ToList();
                Items.Add(new SelectListItem() { Selected = false, Text = "", Value = "0" });
                return Items;

            }

        }

        public IEnumerable<SelectListItem> CurrencyList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.Currencies> Currency = new Data.Repo<Domain.Entities.Currencies>(Context);
                List<NTTData.AMOPortal.Domain.Entities.Currencies> CurrencyList = Currency.GetAll().OrderBy(m => m.CurrencyCode).ToList();
                var Items = CurrencyList.Select(CList => new SelectListItem
                {
                    Value = CList.Id.ToString(),
                    Text = CList.CurrencyCode.ToString()
                });
                return Items;

            }
        }

   
        public IEnumerable<SelectListItem> RegionList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.Regions> Regions = new Data.Repo<Domain.Entities.Regions>(Context);
                List<NTTData.AMOPortal.Domain.Entities.Regions> RegionList = Regions.GetAll().OrderBy(x=>x.RegionName).ToList();
                var Items = RegionList.Select(CList => new SelectListItem
                {
                    Value = CList.Id.ToString(),
                    Text = CList.RegionName.ToString()
                }).ToList();
                Items.Add(new SelectListItem() { Selected = false, Text = "", Value = "0" });
                return Items;
            }
        }

        public IEnumerable<SelectListItem> CientStatusList
        {
            get
            {

                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.ClientStatus> ClientStatuses = new Data.Repo<Domain.Entities.ClientStatus>(Context);
                List<NTTData.AMOPortal.Domain.Entities.ClientStatus> ClientStatusList = ClientStatuses.GetAll().OrderBy(x=>x.Status).ToList();
                var Items = ClientStatusList.Select(CList => new SelectListItem
                {
                    Value = CList.Id.ToString(),
                    Text = CList.Status.ToString()
                });
                return Items;

            }
        }

        public IEnumerable<SelectListItem> ReferencableList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.Referencable> RefAble = new Data.Repo<Domain.Entities.Referencable>(Context);
                List<NTTData.AMOPortal.Domain.Entities.Referencable> RefAbleList = RefAble.GetAll().ToList();
                var Items = RefAbleList.Select(CList => new SelectListItem
                {
                    Value = CList.Id.ToString(),
                    Text = CList.IsReferenceable.ToString()
                });
                return Items;

            }
        }

        public IEnumerable<SelectListItem> ReferredClientList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.Referred> Ref = new Data.Repo<Domain.Entities.Referred>(Context);
                List<NTTData.AMOPortal.Domain.Entities.Referred> RefList = Ref.GetAll().ToList();
                var Items = RefList.Select(CList => new SelectListItem
                {
                    Value = CList.Id.ToString(),
                    Text = CList.HasReferred.ToString()
                });
                return Items;
            }
        }

        public IEnumerable<SelectListItem> CustomerRatingList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.CustomerRating> CustomerRatings = new Data.Repo<Domain.Entities.CustomerRating>(Context);
                List<NTTData.AMOPortal.Domain.Entities.CustomerRating> CustomerRatingsList = CustomerRatings.GetAll().ToList();
                var Items = CustomerRatingsList.Select(CList => new SelectListItem
                {
                    Value = CList.Id.ToString(),
                    Text = CList.Rating.ToString()
                });
                return Items;
            }
        }

        public IEnumerable<SelectListItem> UserClientList
        {
            get
            {
                //////DbContextFactory Context = new DbContextFactory();
                //////Repo<NTTData.AMOPortal.Domain.Entities.Client> Clients = new Data.Repo<Domain.Entities.Client>(Context);
                ////////List<NTTData.AMOPortal.Domain.Entities.Client> ClientsList = Clients.GetAll().OrderBy(m => m.ClientName).ToList();
                //////List<NTTData.AMOPortal.Domain.Entities.Client> ClientsList = Clients.GetAll().Where(m => m.IsDeleted == false ).OrderBy(m => m.ClientName).ToList();
             
                var db = new Db();
                List<Client> clientList = new List<Client>();
                
                var roleId = (from up in db.UserProfiles
                              where up.Id == LoginUser && up.RoleId == "superuser"
                              select up).Any();
                if(roleId!=false)
                {
                    clientList = (from c1 in db.Clients where c1.IsDeleted==false select c1).OrderBy(x => x.ClientName).ToList();
                }
                else
                {
                     clientList = (from cl in db.Clients
                                   join ua in db.UserApplications on cl.Id equals ua.ClientId
                                   where ua.UserId == LoginUser
                                   select cl).Distinct().OrderBy(x => x.ClientName).ToList();
                                  
                }
  
                //for super user there will not be any clients assigned
                //if (clientList==null || clientList.Count()<=0)
                //{
                //    clientList = (from cl in db.Clients where cl.IsDeleted == false select cl).ToList();
                //}

                var Items = clientList.Select(CList => new SelectListItem
                {
                    Value = CList.Id.ToString(),
                    Text = CList.ClientName.ToString()
                }).GroupBy(x => x.Value, (key, group) => group.First()).ToList();
                return Items;
            }
        }

        //modified by Neeraj
        public IEnumerable<SelectListItem> ClientList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.Client> Clients = new Data.Repo<Domain.Entities.Client>(Context);
                //List<NTTData.AMOPortal.Domain.Entities.Client> ClientsList = Clients.GetAll().OrderBy(m => m.ClientName).ToList();
                List<NTTData.AMOPortal.Domain.Entities.Client> ClientsList = Clients.GetAll().Where(m => m.IsDeleted == false).OrderBy(m => m.ClientName).ToList();
                var Items = ClientsList.Select(CList => new SelectListItem
                {
                    Value = CList.Id.ToString(),
                    Text = CList.ClientName.ToString()
                });
                return Items;
            }
        }
        //public byte[] Attachment { get; set; }
        //public string AttachmentName { get; set; }
       // public string CPMFilename { get; set; }
        public string Filename { get; set; }
        public HttpPostedFileBase httpPostedTemplateFile { get; set; }
        public byte[] CPMFile { get; set; }

        public string EmailTemplateImageName { get; set; }
        public HttpPostedFileBase httpPostedEmailTemplateFile { get; set; }
        public byte[] EmailTemplateImageCPMFile { get; set; }
    }
   
       //Added by Anandhi for Client Profile End




    //Added by Raj
    public class MetricInput : Input
    {
        public string MetricName { get; set; }
        public int MetricUnitId { get; set; }


    }



    public class MetricSpecificationInput : Input
    {

        [Display(Name = "Application")]
        public int ApplicationId { get; set; }

        [Display(Name = "End Date")]
        //[GreaterThan("StartDate",ErrorMessage="The end date must fall after the start date")]
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        public Boolean HasSLA { get; set; }

        [Required(ErrorMessage = "Please select a Client")]
        public int? ClientId { get; set; }

        
        public int? AccountId { get; set; }
        public int? ContractId { get; set; }

        public double? MetricBaseline { get; set; }

        [Required]
        [Display(Name = "Metric")]
        public int? MetricId { get; set; }

        public int? MetricOwnerId { get; set; }

        [RequiredIfTrue("HasSLA", ErrorMessage = "The Metric Target field is required")]
        public double? MetricTarget { get; set; }


        [Display(Name = "Start Date")]

        //[Required]
        public DateTime? StartDate { get; set; }


        //[GreaterThan("MetricTarget",ErrorMessage="The Upper Limit must be above the Target value")]
        public double? TargetUpperLimit { get; set; }


        //[LessThan("MetricTarget", ErrorMessage = "The Lower Limit must be below the Target value")]
        public double? TargetLowerLimit { get; set; }

        [Required]
        [Display(Name = "Metric Status")]
        public int MetricStatusId { get; set; }

        public string MetricName { get; set; }

        //[Required]
        [Display(Name = "Metric SLA Name")]
        public string MetricSLAName { get; set; }

        public string CalculationData { get; set; }
        public string ComplianceUnits { get; set; }
        public string ComplianceBaseline { get; set; }
        public string ComplianceTarget { get; set; }
        public string ExceedsExpectation { get; set; }
        public string MetExpectationlowerlimit { get; set; }
        public string MetExpectationhigherlimit { get; set; }
        public string BelowExpectation { get; set; }

        public string IFiledColValue { get; set; }
        public int IPriority { get; set; }
        public string InputFiled { get; set; }

        public string Type { get; set; }
        public int Clocksetup { get; set; }
        public int Priority { get; set; }


        public IEnumerable<SelectListItem> MetricSLAList
        {

            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.MetricSpecification> SLAName = new Data.Repo<Domain.Entities.MetricSpecification>(Context);
                List<NTTData.AMOPortal.Domain.Entities.MetricSpecification> SLANameList = new List<MetricSpecification>();
                SLANameList = SLAName.GetAll().Where(a => a.ClientId == ClientId).ToList();
                SLANameList = SLANameList.Where(a => a.MetricId == MetricId).ToList();
                var Items = SLANameList.Select(ServiceL => new SelectListItem
                {
                    Value = ServiceL.Id.ToString(),
                    Text = ServiceL.MetricSLAName.ToString()
                });
                return Items;

            }
        }

        public string MetricStatuses { get; set; }

        public string Personnels { get; set; }

        public ICollection<MetricInput> Metrics { get; set; }
        public string MetricUnit { get; set; }
        public string action_type { get; set; }

        public string HasSlACheckBox { get; set; }
        public bool ISChecked { get; set; }

        public bool IsRecordFound { get; set; }
        public string MetricOwner { get; set; }

        public List<int> MetricsSpecClientList { get; set; }

        public bool CheckAllApps { get; set; }
        public string CreatedBy { get; set; }
        public DateTime eCreatedDate = DateTime.Now;
        public DateTime CreatedDate { get { return eCreatedDate; } set { eCreatedDate = value; } }
        public string UpdatedBy { get; set; }
        public DateTime eUpdatedDate = DateTime.Now;
        public DateTime UdatedDate { get { return eUpdatedDate; } set { eUpdatedDate = value; } }





        public IEnumerable<SelectListItem> SlaFieldCol
        {

            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.SlaFieldCol> SLAField = new Data.Repo<Domain.Entities.SlaFieldCol>(Context);
                List<NTTData.AMOPortal.Domain.Entities.SlaFieldCol> SLAFieldlist = SLAField.GetAll().ToList();
                var Items = SLAFieldlist.Select(Field => new SelectListItem
                {
                    Value = Field.IFiledColValue.ToString(),
                    Text = Field.IFiledColValue.ToString()
                });
                return Items;

            }
        }

        public List<NTTData.AMOPortal.Domain.Entities.SlaCalSetup> CalSetupList
        {

            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.SlaCalSetup> Metrics = new Data.Repo<Domain.Entities.SlaCalSetup>(Context);
                List<NTTData.AMOPortal.Domain.Entities.SlaCalSetup> list = Metrics.GetAll().Where(a => a.ClientId == ClientId).OrderBy(o => o.InputFiled).ToList();

                return list;

            }

        }

        public List<SelectListItem> ClientAccounts
        {

            get
            {
                Db db = new Db();
                var list = (from clientaccount in db.ClientAccountSetup
                            where clientaccount.ClientId ==ClientId
                            select clientaccount).ToList();
                var Items = list.Select(Field => new SelectListItem
                {
                    Value = Field.Id.ToString(),
                    Text = Field.AccountName.ToString()
                });

                return Items.ToList();

            }
        }


        public List<SelectListItem> ClientAccountsContracts
        {

            get
            {
                Db db = new Db();
                var list = (from contract in db.ClientAccountContract
                            where contract.ClientId == ClientId && contract.AccountId==AccountId
                            select contract).ToList();
                var Items = list.Select(Field => new SelectListItem
                {
                    Value = Field.Id.ToString(),
                    Text = Field.ContractName.ToString()
                });

                return Items.ToList();

            }

        }


        public IEnumerable<SelectListItem> SLAClockTypeList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.SLAClockType> SLAClockType = new Data.Repo<Domain.Entities.SLAClockType>(Context);
                List<NTTData.AMOPortal.Domain.Entities.SLAClockType> SLAClockTypeList = SLAClockType.GetAll().ToList();

                var Items = SLAClockTypeList.Select(SLAList => new SelectListItem
                {
                    Value = SLAList.Id.ToString(),
                    Text = SLAList.Type.ToString()
                });
                return Items;

            }

        }

 
        public IEnumerable<SelectListItem> MetricStatusList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.MetricStatus> MetricStatuses = new Data.Repo<Domain.Entities.MetricStatus>(Context);
                List<NTTData.AMOPortal.Domain.Entities.MetricStatus> MetricStatusesList = MetricStatuses.GetAll().ToList();

                var Items = MetricStatusesList.Select(MList => new SelectListItem
                {
                    Value = MList.Id.ToString(),
                    Text = MList.StatusValue.ToString()
                });
                return Items;

            }

        }

        public IEnumerable<SelectListItem> PersonnelList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.Personnel> MetricStatuses = new Data.Repo<Domain.Entities.Personnel>(Context);
                List<NTTData.AMOPortal.Domain.Entities.Personnel> MetricStatusesList = MetricStatuses.GetAll().ToList();

                var Items = MetricStatusesList.Select(MList => new SelectListItem
                {
                    Value = MList.Id.ToString(),
                    Text = MList.PersonnelName.ToString()
                });
                return Items;

            }

        }

       
        public IList<MetricCatelogList> MetricServiceIdList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.Metric> MetricLists = new Data.Repo<NTTData.AMOPortal.Domain.Entities.Metric>(Context);
                List<NTTData.AMOPortal.Domain.Entities.Metric> e_MetricList = MetricLists.GetAll().ToList();

                var ServiceSet = from metricSpec in e_MetricList
                                 where ((metricSpec.ServiceId == 1) && (metricSpec.inUse == true) && metricSpec.ServiceCategoryId == 1)
                                 select metricSpec;

                List<NTTData.AMOPortal.Domain.Entities.Metric> metricService = ServiceSet.ToList();

                var catelogSet = from p in metricService
                                 select new MetricCatelogList
                                   (
                                       p.Id,
                                       p.MetricName.ToString(),
                                       p.inUse
                                    );
                return catelogSet.ToList();
            }
        }

        public IList<MetricCatelogList> MetricCapIdList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.Metric> MetricLists = new Data.Repo<NTTData.AMOPortal.Domain.Entities.Metric>(Context);
                List<NTTData.AMOPortal.Domain.Entities.Metric> e_MetricList = MetricLists.GetAll().ToList();

                var ServiceSet = from metricSpec in e_MetricList
                                 where ((metricSpec.ServiceId == 2) && (metricSpec.inUse == true) && metricSpec.ServiceCategoryId == 1)
                                 select metricSpec;

                List<NTTData.AMOPortal.Domain.Entities.Metric> metricService = ServiceSet.ToList();

                var catelogSet = from p in metricService
                                 select new MetricCatelogList
                                   (
                                       p.Id,
                                       p.MetricName.ToString(),
                                       p.inUse
                                    );
                return catelogSet.ToList();
            }
        }

        public IList<MetricCatelogList> MetricAvailIdList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.Metric> MetricLists = new Data.Repo<NTTData.AMOPortal.Domain.Entities.Metric>(Context);
                List<NTTData.AMOPortal.Domain.Entities.Metric> e_MetricList = MetricLists.GetAll().ToList();

                var ServiceSet = from metricSpec in e_MetricList
                                 where ((metricSpec.ServiceId == 3) && (metricSpec.inUse == true) && metricSpec.ServiceCategoryId == 1)
                                 select metricSpec;

                List<NTTData.AMOPortal.Domain.Entities.Metric> metricService = ServiceSet.ToList();

                var catelogSet = from p in metricService
                                 select new MetricCatelogList
                                   (
                                       p.Id,
                                       p.MetricName.ToString(),
                                       p.inUse
                                    );
                return catelogSet.ToList();
            }
        }

        public IList<MetricCatelogList> MetricITSecIdList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.Metric> MetricLists = new Data.Repo<NTTData.AMOPortal.Domain.Entities.Metric>(Context);
                List<NTTData.AMOPortal.Domain.Entities.Metric> e_MetricList = MetricLists.GetAll().ToList();

                var ServiceSet = from metricSpec in e_MetricList
                                 where ((metricSpec.ServiceId == 4) && (metricSpec.inUse == true) && metricSpec.ServiceCategoryId == 1)
                                 select metricSpec;

                List<NTTData.AMOPortal.Domain.Entities.Metric> metricService = ServiceSet.ToList();

                var catelogSet = from p in metricService
                                 select new MetricCatelogList
                                   (
                                       p.Id,
                                       p.MetricName.ToString(),
                                       p.inUse
                                    );
                return catelogSet.ToList();
            }
        }

        public IList<MetricCatelogList> MetricIncidentIdList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.Metric> MetricLists = new Data.Repo<NTTData.AMOPortal.Domain.Entities.Metric>(Context);
                List<NTTData.AMOPortal.Domain.Entities.Metric> e_MetricList = MetricLists.GetAll().ToList();

                var ServiceSet = from metricSpec in e_MetricList
                                 where ((metricSpec.ServiceId == 5) && (metricSpec.inUse == true) && metricSpec.ServiceCategoryId == 1)
                                 select metricSpec;

                List<NTTData.AMOPortal.Domain.Entities.Metric> metricService = ServiceSet.ToList();

                var catelogSet = from p in metricService
                                 select new MetricCatelogList
                                   (
                                       p.Id,
                                       p.MetricName.ToString(),
                                       p.inUse
                                    );
                return catelogSet.ToList();
            }
        }

        public IList<MetricCatelogList> MetricServDeskIdList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.Metric> MetricLists = new Data.Repo<NTTData.AMOPortal.Domain.Entities.Metric>(Context);
                List<NTTData.AMOPortal.Domain.Entities.Metric> e_MetricList = MetricLists.GetAll().ToList();

                var ServiceSet = from metricSpec in e_MetricList
                                 where ((metricSpec.ServiceId == 6) && (metricSpec.inUse == true) && metricSpec.ServiceCategoryId == 1)
                                 select metricSpec;

                List<NTTData.AMOPortal.Domain.Entities.Metric> metricService = ServiceSet.ToList();

                var catelogSet = from p in metricService
                                 select new MetricCatelogList
                                   (
                                       p.Id,
                                       p.MetricName.ToString(),
                                       p.inUse
                                    );
                return catelogSet.ToList();
            }
        }

        public IList<MetricCatelogList> MetricConfMgmtIdList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.Metric> MetricLists = new Data.Repo<NTTData.AMOPortal.Domain.Entities.Metric>(Context);
                List<NTTData.AMOPortal.Domain.Entities.Metric> e_MetricList = MetricLists.GetAll().ToList();

                var ServiceSet = from metricSpec in e_MetricList
                                 where ((metricSpec.ServiceId == 7) && (metricSpec.inUse == true) && metricSpec.ServiceCategoryId == 1)
                                 select metricSpec;

                List<NTTData.AMOPortal.Domain.Entities.Metric> metricService = ServiceSet.ToList();

                var catelogSet = from p in metricService
                                 select new MetricCatelogList
                                   (
                                       p.Id,
                                       p.MetricName.ToString(),
                                       p.inUse
                                    );
                return catelogSet.ToList();
            }
        }

        public IList<MetricCatelogList> MetricpblmgmtIdList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.Metric> MetricLists = new Data.Repo<NTTData.AMOPortal.Domain.Entities.Metric>(Context);
                List<NTTData.AMOPortal.Domain.Entities.Metric> e_MetricList = MetricLists.GetAll().ToList();

                var ServiceSet = from metricSpec in e_MetricList
                                 where ((metricSpec.ServiceId == 8) && (metricSpec.inUse == true) && metricSpec.ServiceCategoryId == 1)
                                 select metricSpec;

                List<NTTData.AMOPortal.Domain.Entities.Metric> metricService = ServiceSet.ToList();

                var catelogSet = from p in metricService
                                 select new MetricCatelogList
                                   (
                                       p.Id,
                                       p.MetricName.ToString(),
                                       p.inUse
                                    );
                return catelogSet.ToList();
            }
        }

        public IList<MetricCatelogList> MetricChgmgmtIdList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.Metric> MetricLists = new Data.Repo<NTTData.AMOPortal.Domain.Entities.Metric>(Context);
                List<NTTData.AMOPortal.Domain.Entities.Metric> e_MetricList = MetricLists.GetAll().ToList();

                var ServiceSet = from metricSpec in e_MetricList
                                 where ((metricSpec.ServiceId == 9) && (metricSpec.inUse == true) && metricSpec.ServiceCategoryId == 1)
                                 select metricSpec;

                List<NTTData.AMOPortal.Domain.Entities.Metric> metricService = ServiceSet.ToList();

                var catelogSet = from p in metricService
                                 select new MetricCatelogList
                                   (
                                       p.Id,
                                       p.MetricName.ToString(),
                                       p.inUse
                                    );
                return catelogSet.ToList();
            }
        }

        public IList<MetricCatelogList> MetricRlsmgmtIdList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.Metric> MetricLists = new Data.Repo<NTTData.AMOPortal.Domain.Entities.Metric>(Context);
                List<NTTData.AMOPortal.Domain.Entities.Metric> e_MetricList = MetricLists.GetAll().ToList();

                var ServiceSet = from metricSpec in e_MetricList
                                 where ((metricSpec.ServiceId == 10) && (metricSpec.inUse == true) && metricSpec.ServiceCategoryId == 1)
                                 select metricSpec;

                List<NTTData.AMOPortal.Domain.Entities.Metric> metricService = ServiceSet.ToList();

                var catelogSet = from p in metricService
                                 select new MetricCatelogList
                                   (
                                       p.Id,
                                       p.MetricName.ToString(),
                                       p.inUse
                                    );
                return catelogSet.ToList();
            }
        }

        public IList<MetricCatelogList> MetricMRmgmtIdList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.Metric> MetricLists = new Data.Repo<NTTData.AMOPortal.Domain.Entities.Metric>(Context);
                List<NTTData.AMOPortal.Domain.Entities.Metric> e_MetricList = MetricLists.GetAll().ToList();

                var ServiceSet = from metricSpec in e_MetricList
                                 where ((metricSpec.ServiceId == 16) && (metricSpec.inUse == true) && metricSpec.ServiceCategoryId == 1)
                                 select metricSpec;

                List<NTTData.AMOPortal.Domain.Entities.Metric> metricService = ServiceSet.ToList();

                var catelogSet = from p in metricService
                                 select new MetricCatelogList
                                   (
                                       p.Id,
                                       p.MetricName.ToString(),
                                       p.inUse
                                    );
                return catelogSet.ToList();
            }
        }

        public IList<MetricCatelogList> PortfolioIdList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.Metric> MetricLists = new Data.Repo<NTTData.AMOPortal.Domain.Entities.Metric>(Context);
                List<NTTData.AMOPortal.Domain.Entities.Metric> e_MetricList = MetricLists.GetAll().ToList();

                var ServiceSet = from metricSpec in e_MetricList
                                 where (metricSpec.ServiceId == 11 && metricSpec.ServiceCategoryId == 2 && metricSpec.inUse == true)
                                 select metricSpec;

                List<NTTData.AMOPortal.Domain.Entities.Metric> metricService = ServiceSet.ToList();

                var catelogSet = from p in metricService
                                 select new MetricCatelogList
                                   (
                                       p.Id,
                                       p.MetricName.ToString(),
                                       p.inUse
                                    );
                return catelogSet.ToList();
            }
        }
        public IList<MetricCatelogList> TestingIdList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.Metric> MetricLists = new Data.Repo<NTTData.AMOPortal.Domain.Entities.Metric>(Context);
                List<NTTData.AMOPortal.Domain.Entities.Metric> e_MetricList = MetricLists.GetAll().ToList();

                var ServiceSet = from metricSpec in e_MetricList
                                 where (metricSpec.ServiceId == 12 && metricSpec.ServiceCategoryId == 3 && metricSpec.inUse == true)
                                 select metricSpec;

                List<NTTData.AMOPortal.Domain.Entities.Metric> metricService = ServiceSet.ToList();

                var catelogSet = from p in metricService
                                 select new MetricCatelogList
                                   (
                                       p.Id,
                                       p.MetricName.ToString(),
                                       p.inUse
                                    );
                return catelogSet.ToList();
            }
        }
        public IList<MetricCatelogList> SDIdList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.Metric> MetricLists = new Data.Repo<NTTData.AMOPortal.Domain.Entities.Metric>(Context);
                List<NTTData.AMOPortal.Domain.Entities.Metric> e_MetricList = MetricLists.GetAll().ToList();

                var ServiceSet = from metricSpec in e_MetricList
                                 where (metricSpec.ServiceId == 13 && metricSpec.ServiceCategoryId == 4 && metricSpec.inUse == true)
                                 select metricSpec;

                List<NTTData.AMOPortal.Domain.Entities.Metric> metricService = ServiceSet.ToList();

                var catelogSet = from p in metricService
                                 select new MetricCatelogList
                                   (
                                       p.Id,
                                       p.MetricName.ToString(),
                                       p.inUse
                                    );
                return catelogSet.ToList();
            }
        }
        public IList<MetricCatelogList> InfraIMList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.Metric> MetricLists = new Data.Repo<NTTData.AMOPortal.Domain.Entities.Metric>(Context);
                List<NTTData.AMOPortal.Domain.Entities.Metric> e_MetricList = MetricLists.GetAll().ToList();

                var ServiceSet = from metricSpec in e_MetricList
                                 where (metricSpec.ServiceId == 14 && metricSpec.ServiceCategoryId == 5)
                                 select metricSpec;

                List<NTTData.AMOPortal.Domain.Entities.Metric> metricService = ServiceSet.ToList();

                var catelogSet = from p in metricService
                                 select new MetricCatelogList
                                   (
                                       p.Id,
                                       p.MetricName.ToString(),
                                       p.inUse
                                    );
                return catelogSet.ToList();
            }
        }

        public IList<MetricCatelogList> InfraFulfilList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.Metric> MetricLists = new Data.Repo<NTTData.AMOPortal.Domain.Entities.Metric>(Context);
                List<NTTData.AMOPortal.Domain.Entities.Metric> e_MetricList = MetricLists.GetAll().ToList();

                var ServiceSet = from metricSpec in e_MetricList
                                 where (metricSpec.ServiceId == 17 && metricSpec.ServiceCategoryId == 5)
                                 select metricSpec;

                List<NTTData.AMOPortal.Domain.Entities.Metric> metricService = ServiceSet.ToList();

                var catelogSet = from p in metricService
                                 select new MetricCatelogList
                                   (
                                       p.Id,
                                       p.MetricName.ToString(),
                                       p.inUse
                                    );
                return catelogSet.ToList();
            }
        }

        public IList<MetricCatelogList> InfraManagementList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.Metric> MetricLists = new Data.Repo<NTTData.AMOPortal.Domain.Entities.Metric>(Context);
                List<NTTData.AMOPortal.Domain.Entities.Metric> e_MetricList = MetricLists.GetAll().ToList();

                var ServiceSet = from metricSpec in e_MetricList
                                 where (metricSpec.ServiceId == 18 && metricSpec.ServiceCategoryId == 5)
                                 select metricSpec;

                List<NTTData.AMOPortal.Domain.Entities.Metric> metricService = ServiceSet.ToList();

                var catelogSet = from p in metricService
                                 select new MetricCatelogList
                                   (
                                       p.Id,
                                       p.MetricName.ToString(),
                                       p.inUse
                                    );
                return catelogSet.ToList();
            }
        }


        public IList<MetricCatelogList> InfraChangeManagementList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.Metric> MetricLists = new Data.Repo<NTTData.AMOPortal.Domain.Entities.Metric>(Context);
                List<NTTData.AMOPortal.Domain.Entities.Metric> e_MetricList = MetricLists.GetAll().ToList();

                var ServiceSet = from metricSpec in e_MetricList
                                 where (metricSpec.ServiceId == 19 && metricSpec.ServiceCategoryId == 5)
                                 select metricSpec;

                List<NTTData.AMOPortal.Domain.Entities.Metric> metricService = ServiceSet.ToList();

                var catelogSet = from p in metricService
                                 select new MetricCatelogList
                                   (
                                       p.Id,
                                       p.MetricName.ToString(),
                                       p.inUse
                                    );
                return catelogSet.ToList();
            }
        }

        public IList<MetricCatelogList> InfraSystemAvailList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.Metric> MetricLists = new Data.Repo<NTTData.AMOPortal.Domain.Entities.Metric>(Context);
                List<NTTData.AMOPortal.Domain.Entities.Metric> e_MetricList = MetricLists.GetAll().ToList();

                var ServiceSet = from metricSpec in e_MetricList
                                 where (metricSpec.ServiceId == 20 && metricSpec.ServiceCategoryId == 5)
                                 select metricSpec;

                List<NTTData.AMOPortal.Domain.Entities.Metric> metricService = ServiceSet.ToList();

                var catelogSet = from p in metricService
                                 select new MetricCatelogList
                                   (
                                       p.Id,
                                       p.MetricName.ToString(),
                                       p.inUse
                                    );
                return catelogSet.ToList();
            }
        }
        public IList<MetricCatelogList> EnhanceIdList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.Metric> MetricLists = new Data.Repo<NTTData.AMOPortal.Domain.Entities.Metric>(Context);
                List<NTTData.AMOPortal.Domain.Entities.Metric> e_MetricList = MetricLists.GetAll().ToList();

                var ServiceSet = from metricSpec in e_MetricList
                                 where (metricSpec.ServiceId == 15 && metricSpec.ServiceCategoryId == 6 && metricSpec.inUse == true)
                                 select metricSpec;

                List<NTTData.AMOPortal.Domain.Entities.Metric> metricService = ServiceSet.ToList();

                var catelogSet = from p in metricService
                                 select new MetricCatelogList
                                   (
                                       p.Id,
                                       p.MetricName.ToString(),
                                       p.inUse
                                    );
                return catelogSet.ToList();
            }
        }



        public List<NTTData.AMOPortal.Domain.Entities.Metric> MetricsList
        {

            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.Metric> Metrics = new Data.Repo<Domain.Entities.Metric>(Context);
                List<NTTData.AMOPortal.Domain.Entities.Metric> list = Metrics.GetAll().ToList();

                return list;

            }

        }

        public IEnumerable<NTTData.AMOPortal.Domain.Entities.MetricUnit> MetricUnitsList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.MetricUnit> MetricUnitLists = new Data.Repo<NTTData.AMOPortal.Domain.Entities.MetricUnit>(Context);
                List<NTTData.AMOPortal.Domain.Entities.MetricUnit> list = MetricUnitLists.GetAll().ToList();

                return list;
            }
        }
        public IEnumerable<SelectListItem> ClientList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.Client> ClientLists = new Data.Repo<NTTData.AMOPortal.Domain.Entities.Client>(Context);
                List<NTTData.AMOPortal.Domain.Entities.Client> e_ClientList = ClientLists.GetAll().OrderBy(m => m.ClientName).ToList();

                var Items = e_ClientList.Select(ServiceL => new SelectListItem
                {
                    Value = ServiceL.Id.ToString(),
                    Text = ServiceL.ClientName.ToString()

                }).ToList();
                return Items;
            }
        }
    }


    public class ApplicationInput : Input
    {
        public string ApplicationName { get; set; }
        public int ClientId { get; set; }
        public bool HasSetup { get; set; }
    }

    //public class AppSLASetup
    //{
    //    public AppSLASetup(int id, string name)
    //    {
    //        ID = id;
    //        Name = name;
    //    }

    //    public int ID { get; set; }
    //    public String Name { get; set; }
    //}




    public class MetricActualInput : Input
    {
        // public int Id { get; set; }
        [Required]
        [Display(Name = "Metric")]
        public int MetricId { get; set; }
        public int? ClientId { get; set; }
        [Required]
        [Display(Name = "Application")]
        public int ApplicationId { get; set; }
        public double? Actual { get; set; }
        public string Period { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Comments { get; set; }

        public int? MetricStatusId { get; set; }
        public string MetricName { get; set; }
        public MetricInput Metrices { get; set; }
        public string ApplicationNames { get; set; }
        public string MetricUnit { get; set; }
        public ICollection<MetricInput> Metrics { get; set; }
        public string action_type { get; set; }
        public bool IsRecordFound { get; set; }


      

        public List<NTTData.AMOPortal.Domain.Entities.Metric> MetricsList
        {

            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.Metric> Metrics = new Data.Repo<Domain.Entities.Metric>(Context);
                List<NTTData.AMOPortal.Domain.Entities.Metric> list = Metrics.GetAll().ToList();

                return list;

            }

        }

        public IEnumerable<NTTData.AMOPortal.Domain.Entities.MetricUnit> MetricUnitsList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.MetricUnit> MetricUnitLists = new Data.Repo<NTTData.AMOPortal.Domain.Entities.MetricUnit>(Context);
                List<NTTData.AMOPortal.Domain.Entities.MetricUnit> list = MetricUnitLists.GetAll().ToList();

                return list;
            }
        }

        public IEnumerable<SelectListItem> MonthList
        {
            get
            {
                return DateTimeFormatInfo
                       .InvariantInfo
                       .MonthNames
                       .Select((monthName, index) => new SelectListItem
                       {
                           Value = monthName,
                           Text = monthName
                       });
            }
        }

        public IEnumerable<SelectListItem> ClientList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.Client> ClientLists = new Data.Repo<NTTData.AMOPortal.Domain.Entities.Client>(Context);
                List<NTTData.AMOPortal.Domain.Entities.Client> e_ClientList = ClientLists.GetAll().ToList();

                var Items = e_ClientList.Select(ServiceL => new SelectListItem
                {
                    Value = ServiceL.Id.ToString(),
                    Text = ServiceL.ClientName.ToString()

                }).ToList();

                Items.Insert(0, new SelectListItem { Text = "-Select-", Value = "" });
                return Items;
            }
        }
    }

    public class ResultLine
    {

        public ResultLine() { }

        public string AppName { get; set; }
        public string count { get; set; }
        public string time { get; set; }
        public string timev { get; set; }
        public long IsMissed { get; set; }

    }


    public class IncidentInput : Input
    {
        [Required]
        public string IncidentId { get; set; }
        [Required]
        public string RecordIncluded { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public DateTime? AcknowledgedDateTime { get; set; }
        public DateTime? ResolutionDateTime { get; set; }
        public DateTime? ServiceRestoredDateTime { get; set; }
        public DateTime? LastUpdatedDateTime { get; set; }
        public DateTime? UploadDate { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public string Priority { get; set; }
        public string ImpactScope { get; set; }
        public string Urgency { get; set; }
        public string Type { get; set; }
        public string OutageType { get; set; }
        public string DivisionImpacted { get; set; }
        public string BusinessUnitImpacted { get; set; }
        public string ServiceCategory { get; set; }
        public string ServiceSubcategory { get; set; }
        public string ServiceArea { get; set; }
        public string RegionImpacted { get; set; }
        public string LocationsImpacted { get; set; }
        public string CustomerDept { get; set; }
        public string CustomerLastName { get; set; }
        public string CustomerFirstName { get; set; }
        public string CustomerTitle { get; set; }
        public string CustomerSatisfaction { get; set; }
        public string ImpactType { get; set; }
        public string ImpactDescription { get; set; }
        public string ConfigItemID { get; set; }
        public string ConfigType { get; set; }
        public string ConfigItemName { get; set; }
        public string EnvironmentImpacted { get; set; }
        public string Module { get; set; }
        public string NodeName { get; set; }
        public string RootCauseType { get; set; }
        public string RootCauseDescription { get; set; }
        public string ResolutionType { get; set; }
        public string ResolutionDescription { get; set; }
        public string SolutionID { get; set; }
        public string DeploymentRelease { get; set; }
        public string RelatedChangeIDs { get; set; }
        public string RelatedIDs { get; set; }
        public string RelatedProblemIDs { get; set; }
        public string Helpdesk { get; set; }
        public string AssignedToPerson { get; set; }
        public string AssignedToSupportGroup { get; set; }
        public string CreatedByGroup { get; set; }
        public string CreatedByPerson { get; set; }
        public string CreatedTimeZone { get; set; }
        public string CreatedTime { get; set; }
        public string CreatedWeekDay { get; set; }
        public string CreatedWeekNum { get; set; }
        public string CreatedMonth { get; set; }
        public string CreatedYear { get; set; }
        public string AcknowlededByGroup { get; set; }
        public string AcknowlededByPerson { get; set; }
        public string AcknowledgedTimeZone { get; set; }
        public string AcknowledgedTime { get; set; }
        public string AcknowledgedWeekDay { get; set; }
        public string AcknowledgedWeekNum { get; set; }
        public string AcknowledgedMonth { get; set; }
        public string AcknowledgedYear { get; set; }
        [Required]
        public string ResolvedByGroup { get; set; }
        public string ResolvedByPerson { get; set; }
        [Required]
        public string ResolvedByVendor { get; set; }
        public string ResolutionEffortHours { get; set; }
        public string ResolvedTimeZone { get; set; }
        public string ResolutionTime { get; set; }
        public string ResolutionWeekDay { get; set; }
        public string ResolutionWeekNum { get; set; }
        [Required]
        public string ResolutionMonth { get; set; }
        [Required]
        public string ResolutionYear { get; set; }
        public string ServiceRestoredByGroup { get; set; }
        public string ServiceRestoredByPerson { get; set; }
        public string ServiceRestoredTimeZone { get; set; }
        public string ServiceRestoredTime { get; set; }
        public string ServiceRestoredWeekDay { get; set; }
        public string ServiceRestoredWeekNum { get; set; }
        public string ServiceRestoredMonth { get; set; }
        public string ServiceRestoredYear { get; set; }
        public string LastUpdatedByPerson { get; set; }
        public string ModificationNotes { get; set; }
        public string DataSource { get; set; }
        public string DataSourceNotes { get; set; }
        public string ImpectDescription { get; set; }
        public string UploadedBy { get; set; }
        public int ClientId { get; set; }
        [ForeignKey("ClientId")]
        public Client Clients { get; set; }
        //public List<Incident> incidentLists { get; set; }
        public MetricInput Metrics { get; set; }
        public MetricSpecificationInput MetricSpecifications { get; set; }
        public SLAMetricInput SLAMetrics { get; set; }
        public string SlALable { get; set; }
        public int metricId { get; set; }
        public string TargetValue { get; set; }
        public IEnumerable<string> SelectedPriority { get; set; }
        public IEnumerable<string> SelectedYear { get; set; }
        public IEnumerable<string> SelectedMonth { get; set; }
        public List<ResultLine> GetResult { get; set; }
        public string Resolvedindicator { get; set; }
        [Required]
        public string Waivedindicator { get; set; }
        public string ReportName { get; set; }
        public int Datasetid { get; set; }

        public string Assignedbygroup { get; set; }
        public string Assignedbyperson { get; set; }
        public DateTime? Assigneddatetime { get; set; }
        public DateTime? Esitmatedresponsetimebygroup { get; set; }
        public DateTime? Esitmatedresponsedatetime { get; set; }
        public DateTime? Esitmatedresolutiondate { get; set; }
        public int? Totalsuspenddurationassignedtoack { get; set; }
        public int? Totalsuspenddurationacktoer { get; set; }
        public int? Totalsuspenddurationertoresolution { get; set; }

        public IEnumerable<SelectListItem> Datasetlist
        {
            get
            {

                List<SelectListItem> list = new List<SelectListItem>();
                list.Insert(0, new SelectListItem() { Value = "0", Text = "" });
                list.Insert(0, new SelectListItem() { Value = "1", Text = "Incident Management" });
                list.Insert(0, new SelectListItem() { Value = "2", Text = "Problem Management" });
                list.Insert(0, new SelectListItem() { Value = "3", Text = "Maintenance Request" });
                return new SelectList((IEnumerable<SelectListItem>)list.OrderBy(x => x.Value), "Value", "Text");
            }
        }

        public IEnumerable<SelectListItem> RecordIncludedList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.Incident> IncidentC = new Data.Repo<Domain.Entities.Incident>(Context);
                IEnumerable<Incident> PList = IncidentC.GetAll().Where(a => a.ClientId == ClientId).GroupBy(o => o.RecordIncluded).Select(g => g.FirstOrDefault()).ToList();

                var SRCList = (from p in PList
                               select p.RecordIncluded);

                var Items = SRCList.Select((RecordIncluded) => new SelectListItem
                {
                    Text = RecordIncluded.ToString(),
                    Value = RecordIncluded.ToString()
                });
                return Items;
            }
        }

        public IEnumerable<SelectListItem> ResolvedList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.Incident> IncidentC = new Data.Repo<Domain.Entities.Incident>(Context);
                IEnumerable<Incident> PList = IncidentC.GetAll().Where(a => a.ClientId == ClientId).GroupBy(o => o.Resolvedindicator).Select(g => g.FirstOrDefault()).ToList();

                var SRCList = (from p in PList
                               select p.Resolvedindicator);

                var Items = SRCList.Select((Resolvedindicator) => new SelectListItem
                {
                    Text = Resolvedindicator.ToString(),
                    Value = Resolvedindicator.ToString()
                });
                return Items;
            }
        }

        public IEnumerable<SelectListItem> ResolvedbyVendorList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.Incident> IncidentC = new Data.Repo<Domain.Entities.Incident>(Context);
                IEnumerable<Incident> PList = IncidentC.GetAll().Where(a => a.ClientId == ClientId && a.Resolvedindicator == "Y" && a.Waivedindicator == "N").GroupBy(o => o.ResolvedByVendor).Select(g => g.FirstOrDefault()).ToList();

                var SRCList = (from p in PList
                               select p.ResolvedByVendor);

                var Items = SRCList.Select((ResolvedByVendor) => new SelectListItem
                {
                    Text = ResolvedByVendor.ToString(),
                    Value = ResolvedByVendor.ToString()
                });
                return Items;
            }
        }
        public IEnumerable<SelectListItem> ResolvedbyGroupList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.Incident> IncidentC = new Data.Repo<Domain.Entities.Incident>(Context);
                IEnumerable<Incident> PList = IncidentC.GetAll().Where(a => a.ClientId == ClientId && a.Resolvedindicator == "Y" && a.Waivedindicator == "N").GroupBy(o => o.ResolvedByGroup).Select(g => g.FirstOrDefault()).ToList();

                var SRCList = (from p in PList
                               select p.ResolvedByGroup);

                var Items = SRCList.Select((ResolvedByGroup) => new SelectListItem
                {
                    Text = ResolvedByGroup.ToString(),
                    Value = ResolvedByGroup.ToString()
                });
                return Items;
            }
        }
        public IEnumerable<SelectListItem> WaivedList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.Incident> IncidentC = new Data.Repo<Domain.Entities.Incident>(Context);
                IEnumerable<Incident> PList = IncidentC.GetAll().Where(a => a.ClientId == ClientId && a.Resolvedindicator == "Y" && a.Waivedindicator == "N").GroupBy(o => o.Waivedindicator).Select(g => g.FirstOrDefault()).ToList();

                var SRCList = (from p in PList
                               select p.Waivedindicator);

                var Items = SRCList.Select((Waivedindicator) => new SelectListItem
                {
                    Text = Waivedindicator.ToString(),
                    Value = Waivedindicator.ToString()
                });
                return Items;
            }
        }

        public IEnumerable<SelectListItem> ResolvedyearList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.Incident> IncidentC = new Data.Repo<Domain.Entities.Incident>(Context);
                IEnumerable<Incident> PList = IncidentC.GetAll().Where(a => a.ClientId == ClientId && a.Resolvedindicator == "Y" && a.Waivedindicator == "N").GroupBy(o => o.ResolutionYear).Select(g => g.FirstOrDefault()).ToList();

                var SRCList = (from p in PList
                               select p.ResolutionYear);

                var Items = SRCList.Select((ResolutionYear) => new SelectListItem
                {
                    Text = ResolutionYear.ToString(),
                    Value = ResolutionYear.ToString()
                });
                return Items;
            }
        }

        public IEnumerable<SelectListItem> ResolvedmonthList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.Incident> IncidentC = new Data.Repo<Domain.Entities.Incident>(Context);
                IEnumerable<Incident> PList = IncidentC.GetAll().Where(a => a.ClientId == ClientId && a.Resolvedindicator == "Y" && a.Waivedindicator == "N").GroupBy(o => o.ResolutionMonth).Select(g => g.FirstOrDefault()).ToList();

                var SRCList = (from p in PList
                               select p.ResolutionMonth);

                var Items = SRCList.Select((ResolutionMonth) => new SelectListItem
                {
                    Text = ResolutionMonth.ToString(),
                    Value = ResolutionMonth.ToString()
                });
                return Items;
            }
        }

        public IPagedList<Incident> GetPagedList(int pageNumber = 1)
        {

            var mrlist = incidentLists;

            int noOfRows = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["DataTableNoOfRowsDisplay"].ToString());

            var list = mrlist.ToPagedList(pageNumber, noOfRows);

            return list;

        }

        public List<Incident> incidentLists
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.Incident> IncList = new Data.Repo<NTTData.AMOPortal.Domain.Entities.Incident>(Context);
                List<NTTData.AMOPortal.Domain.Entities.Incident> e_IncList = IncList.GetAll().ToList();

                var incList = from p in e_IncList
                              where p.ClientId == ClientId && Id < 136
                              select p;

                return incList.ToList();
            }
        }


        public IEnumerable<SelectListItem> ClientsList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.Client> ClientLists = new Data.Repo<NTTData.AMOPortal.Domain.Entities.Client>(Context);
                List<NTTData.AMOPortal.Domain.Entities.Client> e_ClientList = ClientLists.GetAll().ToList();

                var Items = e_ClientList.Select(ServiceL => new SelectListItem
                {
                    Value = ServiceL.Id.ToString(),
                    Text = ServiceL.ClientName.ToString()

                }).ToList();

                Items.Insert(0, new SelectListItem { Text = "-Select-", Value = "" });
                return Items;
            }
        }
        public List<NTTData.AMOPortal.Domain.Entities.Metric> MetricsList
        {

            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.Metric> Metrics = new Data.Repo<Domain.Entities.Metric>(Context);
                List<NTTData.AMOPortal.Domain.Entities.Metric> list = Metrics.GetAll().ToList();

                return list;

            }

        }
        public IEnumerable<SelectListItem> MetricAvailIdList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.Metric> MetricLists = new Data.Repo<NTTData.AMOPortal.Domain.Entities.Metric>(Context);
                List<NTTData.AMOPortal.Domain.Entities.Metric> e_MetricList = MetricLists.GetAll().ToList();

                var ServiceSet = from metricSpec in e_MetricList
                                 where metricSpec.ServiceId == 3
                                 select metricSpec;

                List<NTTData.AMOPortal.Domain.Entities.Metric> metricService = ServiceSet.ToList();

                var Items = metricService.Select(ServiceL => new SelectListItem
                {
                    Text = ServiceL.MetricName.ToString(),
                    Value = ServiceL.Id.ToString()
                });
                return Items;
            }
        }
        public List<NTTData.AMOPortal.Domain.Entities.MetricSpecification> MetricsSpecList
        {

            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.MetricSpecification> MetricsSpc = new Data.Repo<Domain.Entities.MetricSpecification>(Context);
                List<NTTData.AMOPortal.Domain.Entities.MetricSpecification> list = MetricsSpc.GetAll().ToList();

                return list;

            }

        }




        public IEnumerable<SelectListItem> GetPriority
        {
            get
            {
                //DbContextFactory db = new DbContextFactory();
                //Repo<NTTData.AMOPortal.Domain.Entities.Incident> priority = new Data.Repo<Incident>(db);

                Repo<NTTData.AMOPortal.Domain.Entities.Incident> incident = GetIncidentEntity.GetInstance();

                //List<Incident> PList = priority.GetAll().ToList();

                //var list = (from p in PList
                //            select p.Priority)
                //                              .Distinct().ToList();


                IEnumerable<Incident> PList = incident.Where(c => c.ClientId == ClientId).GroupBy(o => o.Priority).Select(g => g.FirstOrDefault());

                var list = (from p in PList
                            select p.Priority);

                var Items = list.Select((priorityName) => new SelectListItem
                {
                    Text = priorityName,
                    Value = priorityName
                }).ToList();
                Items.Insert(0, new SelectListItem { Text = "All", Value = "" });

                return Items;
            }
        }

        public IEnumerable<SelectListItem> GetYear
        {
            get
            {
                //DbContextFactory db = new DbContextFactory();
                //Repo<NTTData.AMOPortal.Domain.Entities.Incident> incident = new Data.Repo<Incident>(db);

                Repo<NTTData.AMOPortal.Domain.Entities.Incident> incident = GetIncidentEntity.GetInstance();

                IEnumerable<Incident> PList = incident.Where(c => c.ClientId == ClientId).GroupBy(o => o.CreatedYear).Select(g => g.FirstOrDefault());

                var list = (from p in PList
                            select p.CreatedYear);


                //List<Incident> YList = incident.GetAll().ToList();
                //var list = (from p in YList
                //            select p.CreatedYear)
                //                               .Distinct().ToList();
                var Items = list.Select((year) => new SelectListItem
                {
                    Text = year,
                    Value = year
                }).ToList();
                Items.Insert(0, new SelectListItem { Text = "All", Value = "" });
                return Items;
            }
        }

        public IEnumerable<SelectListItem> GetMonth
        {
            get
            {
                //DbContextFactory db = new DbContextFactory();
                //Repo<NTTData.AMOPortal.Domain.Entities.Incident> incident = new Data.Repo<Incident>(db);

                Repo<NTTData.AMOPortal.Domain.Entities.Incident> incident = GetIncidentEntity.GetInstance();
                IEnumerable<Incident> PList = incident.Where(c => c.ClientId == ClientId).GroupBy(o => o.CreatedMonth).Select(g => g.FirstOrDefault());

                var list = (from p in PList
                            select p.CreatedMonth);

                var Items = list.Select((month) => new SelectListItem
                {
                    Text = month,
                    Value = month
                }).ToList();
                Items.Insert(0, new SelectListItem { Text = "All", Value = "" });

                return Items;
            }
        }



        public IEnumerable<SelectListItem> GetImpactType
        {
            get
            {
                Repo<NTTData.AMOPortal.Domain.Entities.Incident> incident = GetIncidentEntity.GetInstance();

                //List<Incident> MList = incident.GetAll().ToList();

                IEnumerable<Incident> PList = incident.GetAll().GroupBy(o => o.ImpactType).Select(g => g.FirstOrDefault());

                var list = (from p in PList
                            select p.ImpactType);

                //var list = (from p in MList
                //            where p.ImpactType != null
                //            select p.ImpactType)
                //                               .Distinct().ToList();
                var Items = list.Select((impactType) => new SelectListItem
                {
                    Text = impactType,
                    Value = impactType
                });
                return Items;
            }
        }

        public IEnumerable<SelectListItem> GetOutageType
        {
            get
            {
                Repo<NTTData.AMOPortal.Domain.Entities.Incident> incident = GetIncidentEntity.GetInstance();

                //List<Incident> MList = incident.GetAll().ToList();
                //var list = (from p in MList
                //            where p.OutageType != null
                //            select p.OutageType)
                //                               .Distinct().ToList();

                IEnumerable<Incident> PList = incident.GetAll().GroupBy(o => o.OutageType).Select(g => g.FirstOrDefault());

                var list = (from p in PList
                            select p.OutageType);

                var Items = list.Select((outageType) => new SelectListItem
                {
                    Text = outageType,
                    Value = outageType
                });
                return Items;
            }
        }

        public IEnumerable<SelectListItem> GetEnvironmentImpected
        {
            get
            {
                //DbContextFactory db = new DbContextFactory();
                //Repo<NTTData.AMOPortal.Domain.Entities.Incident> incident = new Data.Repo<Incident>(db);
                Repo<NTTData.AMOPortal.Domain.Entities.Incident> incident = GetIncidentEntity.GetInstance();

                IEnumerable<Incident> PList = incident.GetAll().GroupBy(o => o.EnvironmentImpacted).Select(g => g.FirstOrDefault());

                var list = (from p in PList
                            select p.EnvironmentImpacted);

                //List<Incident> MList = incident.GetAll().ToList();
                //var list = (from p in MList
                //            where p.EnvironmentImpacted != null
                //            select p.EnvironmentImpacted)
                //                               .Distinct().ToList();


                var Items = list.Select((environmentImpacted) => new SelectListItem
                {
                    Text = environmentImpacted,
                    Value = environmentImpacted
                });
                return Items;
            }
        }

        public IEnumerable<SelectListItem> GetResolutionType
        {
            get
            {
                //DbContextFactory db = new DbContextFactory();
                //Repo<NTTData.AMOPortal.Domain.Entities.Incident> incident = new Data.Repo<Incident>(db);


                //List<Incident> MList = incident.GetAll().ToList();
                //var list = (from p in MList
                //            where p.ResolutionType != null
                //            select p.ResolutionType)
                //                               .Distinct().ToList();

                Repo<NTTData.AMOPortal.Domain.Entities.Incident> incident = GetIncidentEntity.GetInstance();
                IEnumerable<Incident> PList = incident.GetAll().GroupBy(o => o.ResolutionType).Select(g => g.FirstOrDefault());

                var list = (from p in PList
                            select p.ResolutionType);

                var Items = list.Select((resolutionType) => new SelectListItem
                {
                    Text = resolutionType,
                    Value = resolutionType
                });
                return Items;
            }
        }
    }

    public static class GetIncidentEntity
    {
        private static Repo<Incident> _repoIncidentEntity = null;

        static GetIncidentEntity()
        {
        }

        public static Repo<Incident> GetInstance()
        {
            if (_repoIncidentEntity == null)
            {
                DbContextFactory db = new DbContextFactory();
                _repoIncidentEntity = new Data.Repo<Incident>(db);

            }
            return _repoIncidentEntity;
        }
    }

    // Added by Raj 

    // Added by Raj 

    public class MetricCatelogList
    {
        public MetricCatelogList(int id, string name, bool availability)
        {
            ID = id;
            Name = name;
            Availability = availability;
        }

        public int ID { get; set; }
        public String Name { get; set; }
        public bool Availability { get; set; }
    }


    public class SLAMetricInput : Input
    {

        public IList<MetricCatelogList> PortfolioIdList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.Metric> MetricLists = new Data.Repo<NTTData.AMOPortal.Domain.Entities.Metric>(Context);
                List<NTTData.AMOPortal.Domain.Entities.Metric> e_MetricList = MetricLists.GetAll().ToList();

                var ServiceSet = from metricSpec in e_MetricList
                                 where (metricSpec.ServiceId == 11 && metricSpec.ServiceCategoryId == 2)
                                 select metricSpec;

                List<NTTData.AMOPortal.Domain.Entities.Metric> metricService = ServiceSet.ToList();

                var catelogSet = from p in metricService
                                 select new MetricCatelogList
                                   (
                                       p.Id,
                                       p.MetricName.ToString(),
                                       p.inUse
                                    );
                return catelogSet.ToList();
            }
        }
        public IList<MetricCatelogList> TestingIdList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.Metric> MetricLists = new Data.Repo<NTTData.AMOPortal.Domain.Entities.Metric>(Context);
                List<NTTData.AMOPortal.Domain.Entities.Metric> e_MetricList = MetricLists.GetAll().ToList();

                var ServiceSet = from metricSpec in e_MetricList
                                 where (metricSpec.ServiceId == 12 && metricSpec.ServiceCategoryId == 3)
                                 select metricSpec;

                List<NTTData.AMOPortal.Domain.Entities.Metric> metricService = ServiceSet.ToList();

                var catelogSet = from p in metricService
                                 select new MetricCatelogList
                                   (
                                       p.Id,
                                       p.MetricName.ToString(),
                                       p.inUse
                                    );
                return catelogSet.ToList();
            }
        }
        public IList<MetricCatelogList> SDIdList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.Metric> MetricLists = new Data.Repo<NTTData.AMOPortal.Domain.Entities.Metric>(Context);
                List<NTTData.AMOPortal.Domain.Entities.Metric> e_MetricList = MetricLists.GetAll().ToList();

                var ServiceSet = from metricSpec in e_MetricList
                                 where (metricSpec.ServiceId == 13 && metricSpec.ServiceCategoryId == 4)
                                 select metricSpec;

                List<NTTData.AMOPortal.Domain.Entities.Metric> metricService = ServiceSet.ToList();

                var catelogSet = from p in metricService
                                 select new MetricCatelogList
                                   (
                                       p.Id,
                                       p.MetricName.ToString(),
                                       p.inUse
                                    );
                return catelogSet.ToList();
            }
        }
        public IList<MetricCatelogList> InfraIMList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.Metric> MetricLists = new Data.Repo<NTTData.AMOPortal.Domain.Entities.Metric>(Context);
                List<NTTData.AMOPortal.Domain.Entities.Metric> e_MetricList = MetricLists.GetAll().ToList();

                var ServiceSet = from metricSpec in e_MetricList
                                 where (metricSpec.ServiceId == 14 && metricSpec.ServiceCategoryId == 5)
                                 select metricSpec;

                List<NTTData.AMOPortal.Domain.Entities.Metric> metricService = ServiceSet.ToList();

                var catelogSet = from p in metricService
                                 select new MetricCatelogList
                                   (
                                       p.Id,
                                       p.MetricName.ToString(),
                                       p.inUse
                                    );
                return catelogSet.ToList();
            }
        }

        public IList<MetricCatelogList> InfraFulfilList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.Metric> MetricLists = new Data.Repo<NTTData.AMOPortal.Domain.Entities.Metric>(Context);
                List<NTTData.AMOPortal.Domain.Entities.Metric> e_MetricList = MetricLists.GetAll().ToList();

                var ServiceSet = from metricSpec in e_MetricList
                                 where (metricSpec.ServiceId == 17 && metricSpec.ServiceCategoryId == 5)
                                 select metricSpec;

                List<NTTData.AMOPortal.Domain.Entities.Metric> metricService = ServiceSet.ToList();

                var catelogSet = from p in metricService
                                 select new MetricCatelogList
                                   (
                                       p.Id,
                                       p.MetricName.ToString(),
                                       p.inUse
                                    );
                return catelogSet.ToList();
            }
        }

        public IList<MetricCatelogList> InfraManagementList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.Metric> MetricLists = new Data.Repo<NTTData.AMOPortal.Domain.Entities.Metric>(Context);
                List<NTTData.AMOPortal.Domain.Entities.Metric> e_MetricList = MetricLists.GetAll().ToList();

                var ServiceSet = from metricSpec in e_MetricList
                                 where (metricSpec.ServiceId == 18 && metricSpec.ServiceCategoryId == 5)
                                 select metricSpec;

                List<NTTData.AMOPortal.Domain.Entities.Metric> metricService = ServiceSet.ToList();

                var catelogSet = from p in metricService
                                 select new MetricCatelogList
                                   (
                                       p.Id,
                                       p.MetricName.ToString(),
                                       p.inUse
                                    );
                return catelogSet.ToList();
            }
        }


        public IList<MetricCatelogList> InfraChangeManagementList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.Metric> MetricLists = new Data.Repo<NTTData.AMOPortal.Domain.Entities.Metric>(Context);
                List<NTTData.AMOPortal.Domain.Entities.Metric> e_MetricList = MetricLists.GetAll().ToList();

                var ServiceSet = from metricSpec in e_MetricList
                                 where (metricSpec.ServiceId == 19 && metricSpec.ServiceCategoryId == 5)
                                 select metricSpec;

                List<NTTData.AMOPortal.Domain.Entities.Metric> metricService = ServiceSet.ToList();

                var catelogSet = from p in metricService
                                 select new MetricCatelogList
                                   (
                                       p.Id,
                                       p.MetricName.ToString(),
                                       p.inUse
                                    );
                return catelogSet.ToList();
            }
        }

        public IList<MetricCatelogList> InfraSystemAvailList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.Metric> MetricLists = new Data.Repo<NTTData.AMOPortal.Domain.Entities.Metric>(Context);
                List<NTTData.AMOPortal.Domain.Entities.Metric> e_MetricList = MetricLists.GetAll().ToList();

                var ServiceSet = from metricSpec in e_MetricList
                                 where (metricSpec.ServiceId == 20 && metricSpec.ServiceCategoryId == 5)
                                 select metricSpec;

                List<NTTData.AMOPortal.Domain.Entities.Metric> metricService = ServiceSet.ToList();

                var catelogSet = from p in metricService
                                 select new MetricCatelogList
                                   (
                                       p.Id,
                                       p.MetricName.ToString(),
                                       p.inUse
                                    );
                return catelogSet.ToList();
            }
        }
        public IList<MetricCatelogList> EnhanceIdList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.Metric> MetricLists = new Data.Repo<NTTData.AMOPortal.Domain.Entities.Metric>(Context);
                List<NTTData.AMOPortal.Domain.Entities.Metric> e_MetricList = MetricLists.GetAll().ToList();

                var ServiceSet = from metricSpec in e_MetricList
                                 where (metricSpec.ServiceId == 15 && metricSpec.ServiceCategoryId == 6)
                                 select metricSpec;

                List<NTTData.AMOPortal.Domain.Entities.Metric> metricService = ServiceSet.ToList();

                var catelogSet = from p in metricService
                                 select new MetricCatelogList
                                   (
                                       p.Id,
                                       p.MetricName.ToString(),
                                       p.inUse
                                    );
                return catelogSet.ToList();
            }
        }
        public IList<MetricCatelogList> MetricServiceIdList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.Metric> MetricLists = new Data.Repo<NTTData.AMOPortal.Domain.Entities.Metric>(Context);
                List<NTTData.AMOPortal.Domain.Entities.Metric> e_MetricList = MetricLists.GetAll().ToList();

                var ServiceSet = from metricSpec in e_MetricList
                                 where (metricSpec.ServiceId == 1 && metricSpec.ServiceCategoryId == 1)
                                 select metricSpec;

                List<NTTData.AMOPortal.Domain.Entities.Metric> metricService = ServiceSet.ToList();

                var catelogSet = from p in metricService
                                 select new MetricCatelogList
                                   (
                                       p.Id,
                                       p.MetricName.ToString(),
                                       p.inUse
                                    );
                return catelogSet.ToList();
            }
        }

        public IList<MetricCatelogList> MetricCapIdList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.Metric> MetricLists = new Data.Repo<NTTData.AMOPortal.Domain.Entities.Metric>(Context);
                List<NTTData.AMOPortal.Domain.Entities.Metric> e_MetricList = MetricLists.GetAll().ToList();

                var ServiceSet = from metricSpec in e_MetricList
                                 where (metricSpec.ServiceId == 2 && metricSpec.ServiceCategoryId == 1)
                                 select metricSpec;

                List<NTTData.AMOPortal.Domain.Entities.Metric> metricService = ServiceSet.ToList();

                var catelogSet = from p in metricService
                                 select new MetricCatelogList
                                   (
                                       p.Id,
                                       p.MetricName.ToString(),
                                       p.inUse
                                    );
                return catelogSet.ToList();
            }
        }

        public IList<MetricCatelogList> MetricAvailIdList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.Metric> MetricLists = new Data.Repo<NTTData.AMOPortal.Domain.Entities.Metric>(Context);
                List<NTTData.AMOPortal.Domain.Entities.Metric> e_MetricList = MetricLists.GetAll().ToList();

                var ServiceSet = from metricSpec in e_MetricList
                                 where (metricSpec.ServiceId == 3 && metricSpec.ServiceCategoryId == 1)
                                 select metricSpec;

                List<NTTData.AMOPortal.Domain.Entities.Metric> metricService = ServiceSet.ToList();

                var catelogSet = from p in metricService
                                 select new MetricCatelogList
                                   (
                                       p.Id,
                                       p.MetricName.ToString(),
                                       p.inUse
                                    );
                return catelogSet.ToList();
            }
        }

        public IList<MetricCatelogList> MetricITSecIdList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.Metric> MetricLists = new Data.Repo<NTTData.AMOPortal.Domain.Entities.Metric>(Context);
                List<NTTData.AMOPortal.Domain.Entities.Metric> e_MetricList = MetricLists.GetAll().ToList();

                var ServiceSet = from metricSpec in e_MetricList
                                 where (metricSpec.ServiceId == 4 && metricSpec.ServiceCategoryId == 1)
                                 select metricSpec;

                List<NTTData.AMOPortal.Domain.Entities.Metric> metricService = ServiceSet.ToList();

                var catelogSet = from p in metricService
                                 select new MetricCatelogList
                                   (
                                       p.Id,
                                       p.MetricName.ToString(),
                                       p.inUse
                                    );
                return catelogSet.ToList();
            }
        }

        public IList<MetricCatelogList> MetricIncidentIdList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.Metric> MetricLists = new Data.Repo<NTTData.AMOPortal.Domain.Entities.Metric>(Context);
                List<NTTData.AMOPortal.Domain.Entities.Metric> e_MetricList = MetricLists.GetAll().ToList();

                var ServiceSet = from metricSpec in e_MetricList
                                 where (metricSpec.ServiceId == 5 && metricSpec.ServiceCategoryId == 1)
                                 select metricSpec;

                List<NTTData.AMOPortal.Domain.Entities.Metric> metricService = ServiceSet.ToList();

                var catelogSet = from p in metricService
                                 select new MetricCatelogList
                                   (
                                       p.Id,
                                       p.MetricName.ToString(),
                                       p.inUse
                                    );
                return catelogSet.ToList();
            }
        }

        public IList<MetricCatelogList> MetricServDeskIdList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.Metric> MetricLists = new Data.Repo<NTTData.AMOPortal.Domain.Entities.Metric>(Context);
                List<NTTData.AMOPortal.Domain.Entities.Metric> e_MetricList = MetricLists.GetAll().ToList();

                var ServiceSet = from metricSpec in e_MetricList
                                 where (metricSpec.ServiceId == 6 && metricSpec.ServiceCategoryId == 1)
                                 select metricSpec;

                List<NTTData.AMOPortal.Domain.Entities.Metric> metricService = ServiceSet.ToList();

                var catelogSet = from p in metricService
                                 select new MetricCatelogList
                                   (
                                       p.Id,
                                       p.MetricName.ToString(),
                                       p.inUse
                                    );
                return catelogSet.ToList();
            }
        }

        public IList<MetricCatelogList> MetricConfMgmtIdList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.Metric> MetricLists = new Data.Repo<NTTData.AMOPortal.Domain.Entities.Metric>(Context);
                List<NTTData.AMOPortal.Domain.Entities.Metric> e_MetricList = MetricLists.GetAll().ToList();

                var ServiceSet = from metricSpec in e_MetricList
                                 where (metricSpec.ServiceId == 7 && metricSpec.ServiceCategoryId == 1)
                                 select metricSpec;

                List<NTTData.AMOPortal.Domain.Entities.Metric> metricService = ServiceSet.ToList();

                var catelogSet = from p in metricService
                                 select new MetricCatelogList
                                   (
                                       p.Id,
                                       p.MetricName.ToString(),
                                       p.inUse
                                    );
                return catelogSet.ToList();
            }
        }

        public IList<MetricCatelogList> MetricpblmgmtIdList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.Metric> MetricLists = new Data.Repo<NTTData.AMOPortal.Domain.Entities.Metric>(Context);
                List<NTTData.AMOPortal.Domain.Entities.Metric> e_MetricList = MetricLists.GetAll().ToList();

                var ServiceSet = from metricSpec in e_MetricList
                                 where (metricSpec.ServiceId == 8 && metricSpec.ServiceCategoryId == 1)
                                 select metricSpec;

                List<NTTData.AMOPortal.Domain.Entities.Metric> metricService = ServiceSet.ToList();

                var catelogSet = from p in metricService
                                 select new MetricCatelogList
                                   (
                                       p.Id,
                                       p.MetricName.ToString(),
                                       p.inUse
                                    );
                return catelogSet.ToList();
            }
        }

        public IList<MetricCatelogList> MetricChgmgmtIdList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.Metric> MetricLists = new Data.Repo<NTTData.AMOPortal.Domain.Entities.Metric>(Context);
                List<NTTData.AMOPortal.Domain.Entities.Metric> e_MetricList = MetricLists.GetAll().ToList();

                var ServiceSet = from metricSpec in e_MetricList
                                 where (metricSpec.ServiceId == 9 && metricSpec.ServiceCategoryId == 1)
                                 select metricSpec;

                List<NTTData.AMOPortal.Domain.Entities.Metric> metricService = ServiceSet.ToList();

                var catelogSet = from p in metricService
                                 select new MetricCatelogList
                                   (
                                       p.Id,
                                       p.MetricName.ToString(),
                                       p.inUse
                                    );
                return catelogSet.ToList();
            }
        }

        public IList<MetricCatelogList> MetricRlsmgmtIdList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.Metric> MetricLists = new Data.Repo<NTTData.AMOPortal.Domain.Entities.Metric>(Context);
                List<NTTData.AMOPortal.Domain.Entities.Metric> e_MetricList = MetricLists.GetAll().ToList();

                var ServiceSet = from metricSpec in e_MetricList
                                 where (metricSpec.ServiceId == 10 && metricSpec.ServiceCategoryId == 1)
                                 select metricSpec;

                List<NTTData.AMOPortal.Domain.Entities.Metric> metricService = ServiceSet.ToList();

                var catelogSet = from p in metricService
                                 select new MetricCatelogList
                                   (
                                       p.Id,
                                       p.MetricName.ToString(),
                                       p.inUse
                                    );
                return catelogSet.ToList();
            }
        }

        public IList<MetricCatelogList> MetricMRmgmtIdList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.Metric> MetricLists = new Data.Repo<NTTData.AMOPortal.Domain.Entities.Metric>(Context);
                List<NTTData.AMOPortal.Domain.Entities.Metric> e_MetricList = MetricLists.GetAll().ToList();

                var ServiceSet = from metricSpec in e_MetricList
                                 where (metricSpec.ServiceId == 16 && metricSpec.ServiceCategoryId == 1)
                                 select metricSpec;

                List<NTTData.AMOPortal.Domain.Entities.Metric> metricService = ServiceSet.ToList();

                var catelogSet = from p in metricService
                                 select new MetricCatelogList
                                   (
                                       p.Id,
                                       p.MetricName.ToString(),
                                       p.inUse
                                    );
                return catelogSet.ToList();
            }
        }

        public int ClientId { get; set; }
        public IEnumerable<SelectListItem> ClientList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.Client> ClientLists = new Data.Repo<NTTData.AMOPortal.Domain.Entities.Client>(Context);
                List<NTTData.AMOPortal.Domain.Entities.Client> e_ClientList = ClientLists.GetAll().ToList();

                var Items = e_ClientList.Select(ServiceL => new SelectListItem
                {
                    Value = ServiceL.Id.ToString(),
                    Text = ServiceL.ClientName.ToString()
                });
                return Items;
            }
        }

        public int? MetricDataSetId { get; set; }
        public IEnumerable<SelectListItem> MetricDataSetIdList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.Metric> MetricLists = new Data.Repo<NTTData.AMOPortal.Domain.Entities.Metric>(Context);
                List<NTTData.AMOPortal.Domain.Entities.Metric> e_MetricList = MetricLists.GetAll().ToList();

                var Items = e_MetricList.Select(ServiceL => new SelectListItem
                {
                    Value = ServiceL.Id.ToString(),
                    Text = ServiceL.MetricName.ToString()
                });
                return Items;
            }
        }

        [Required]
        [DisplayName("Metric Name")]
        public string MetricName { get; set; }
        public string Objective { get; set; }

        public int ObjectiveTypeId { get; set; }
        public SLAObjTypeInput ObjectiveTypeIds { get; set; }

        public int CalculationId { get; set; }
        public SLACalcInput CalculationIds { get; set; }

        public int CalculationDataId { get; set; }
        public SLACalcDataInput CalculationDataIds { get; set; }

        public int ContextId { get; set; }
        public SLAContextInput ContextIds { get; set; }

        public bool inUse { get; set; }

        [Required]
        [DisplayName("Service")]
        public int ServiceId { get; set; }
        public IEnumerable<SelectListItem> ServiceNameList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.Service> ServiceNameLists = new Data.Repo<NTTData.AMOPortal.Domain.Entities.Service>(Context);
                List<NTTData.AMOPortal.Domain.Entities.Service> e_ServiceNameList = ServiceNameLists.GetAll().ToList();
                var Items = e_ServiceNameList.Select(ServiceL => new SelectListItem
                {
                    Value = ServiceL.Id.ToString(),
                    Text = ServiceL.Name.ToString()
                });
                return Items;
            }
        }
        public IEnumerable<SelectListItem> ServiceNameEList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.Service> ServiceNameLists = new Data.Repo<NTTData.AMOPortal.Domain.Entities.Service>(Context);
                List<NTTData.AMOPortal.Domain.Entities.Service> e_ServiceNameList = ServiceNameLists.GetAll().Where(a => a.ServiceCategoryId == ServiceCategoryId).ToList();
                var Items = e_ServiceNameList.Select(ServiceL => new SelectListItem
                {
                    Value = ServiceL.Id.ToString(),
                    Text = ServiceL.Name.ToString()
                });
                return Items;
            }
        }

        [Required]
        [DisplayName("Service Category")]
        public int ServiceCategoryId { get; set; }
        public IEnumerable<SelectListItem> ServiceCategoryList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.ServiceCategory> ServiceCategoryLists = new Data.Repo<NTTData.AMOPortal.Domain.Entities.ServiceCategory>(Context);
                List<NTTData.AMOPortal.Domain.Entities.ServiceCategory> e_ServiceCategoryList = ServiceCategoryLists.GetAll().ToList();
                var Items = e_ServiceCategoryList.Select(sCategoryL => new SelectListItem
                {
                    Value = sCategoryL.Id.ToString(),
                    Text = sCategoryL.Name.ToString()
                });
                return Items;
            }
        }

        [DisplayName("Service Component")]
        public int? ServiceComponentId { get; set; }
        public IEnumerable<SelectListItem> ServiceComponentList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.ServiceComponent> ServiceComponentLists = new Data.Repo<NTTData.AMOPortal.Domain.Entities.ServiceComponent>(Context);
                List<NTTData.AMOPortal.Domain.Entities.ServiceComponent> e_ServiceComponentList = ServiceComponentLists.GetAll().ToList();
                var Items = e_ServiceComponentList.Select(ServiceL => new SelectListItem
                {
                    Value = ServiceL.Id.ToString(),
                    Text = ServiceL.Name.ToString()
                });
                return Items;
            }
        }

        public int? MetricPerspectiveId { get; set; }
        public IEnumerable<SelectListItem> MetricPerspectiveList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.MetricPerspective> MetricPerspectiveLists = new Data.Repo<NTTData.AMOPortal.Domain.Entities.MetricPerspective>(Context);
                List<NTTData.AMOPortal.Domain.Entities.MetricPerspective> e_MetricPerspectiveList = MetricPerspectiveLists.GetAll().ToList();
                var Items = e_MetricPerspectiveList.Select(ServiceL => new SelectListItem
                {
                    Value = ServiceL.Id.ToString(),
                    Text = ServiceL.PerspectiveName.ToString()
                });
                return Items;
            }
        }

        public int? MetricTypeId { get; set; }
        public IEnumerable<SelectListItem> MetricTypeList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.MetricType> MetricTypeLists = new Data.Repo<NTTData.AMOPortal.Domain.Entities.MetricType>(Context);
                List<NTTData.AMOPortal.Domain.Entities.MetricType> e_MetricTypeList = MetricTypeLists.GetAll().ToList();
                var Items = e_MetricTypeList.Select(ServiceL => new SelectListItem
                {
                    Value = ServiceL.Id.ToString(),
                    Text = ServiceL.TypeName.ToString()
                });
                return Items;
            }
        }

        public int ChartIndicatorId { get; set; }
        public SLAChartIndInput ChartIndicatorIds { get; set; }

        public int MetricUnitId { get; set; }
        public SLAMetricUnit MetricUnitIds { get; set; }

     
        public int RulesId { get; set; }
        public SLARulesInput RulesIds { get; set; }

        public int PrimaryAudienceId { get; set; }
        public SLAPYAudInput PrimaryAudienceIds { get; set; }

        public string CreatedBy { get; set; }
        public DateTime eCreatedDate = DateTime.Now;
        public DateTime CreatedDate { get { return eUpdatedDate; } set { eUpdatedDate = value; } }
        public string UpdatedBy { get; set; }
        public DateTime eUpdatedDate = DateTime.Now;
        public DateTime UpdatedDate { get { return eUpdatedDate; } set { eUpdatedDate = value; } }
    }


    public class MetricChartIndicatorInput : Input
    {
        public SLAMetricInput MetricIds { get; set; }
        public int MetricId { get; set; }
        public SLAChartIndInput ChartIndicatorIds { get; set; }
        public int ChartIndicatorId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime eCreatedDate = DateTime.Now;
        public DateTime CreatedDate { get { return eCreatedDate; } set { eCreatedDate = value; } }
        public string UpdatedBy { get; set; }
        public DateTime eUpdatedDate = DateTime.Now;
        public DateTime UpdatedDate { get { return eUpdatedDate; } set { eUpdatedDate = value; } }
    }

    public class SLAObjTypeInput : Input
    {
        public string ObjectiveName { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public DateTime eCreatedDate = DateTime.Now;
        public DateTime CreatedDate { get { return eCreatedDate; } set { eCreatedDate = value; } }
        public string UpdatedBy { get; set; }
        public DateTime eUpdatedDate = DateTime.Now;
        public DateTime UpdatedDate { get { return eUpdatedDate; } set { eUpdatedDate = value; } }
    }

    public class SLAContextInput : Input
    {
        public string ElementName { get; set; }
        public string Description { get; set; }
        public DateTime eCreatedDate = DateTime.Now;
        public DateTime CreatedDate { get { return eCreatedDate; } set { eCreatedDate = value; } }
        public string UpdatedBy { get; set; }
        public DateTime eUpdatedDate = DateTime.Now;
        public DateTime UpdatedDate { get { return eUpdatedDate; } set { eUpdatedDate = value; } }
    }

    public class SLACalcInput : Input
    {
        public string CalculationName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime eCreatedDate = DateTime.Now;
        public DateTime CreatedDate { get { return eCreatedDate; } set { eCreatedDate = value; } }
        public string UpdatedBy { get; set; }
        public DateTime eUpdatedDate = DateTime.Now;
        public DateTime UpdatedDate { get { return eUpdatedDate; } set { eUpdatedDate = value; } }
    }

    public class SLACalcDataInput : Input
    {
        public string ElementName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime eCreatedDate = DateTime.Now;
        public DateTime CreatedDate { get { return eCreatedDate; } set { eCreatedDate = value; } }
        public string UpdatedBy { get; set; }
        public DateTime eUpdatedDate = DateTime.Now;
        public DateTime UpdatedDate { get { return eUpdatedDate; } set { eUpdatedDate = value; } }
    }

    public class SLARulesInput : Input
    {
        public string RuleName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime eCreatedDate = DateTime.Now;
        public DateTime CreatedDate { get { return eCreatedDate; } set { eCreatedDate = value; } }
        public string UpdatedBy { get; set; }
        public DateTime eUpdatedDate = DateTime.Now;
        public DateTime UpdatedDate { get { return eUpdatedDate; } set { eUpdatedDate = value; } }
    }

    public class SLAChartIndInput : Input
    {
        public string IsChart { get; set; }
        public string IsIndicator { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public DateTime eDate = DateTime.Now;
        public DateTime CreatedDate { get { return eDate; } set { eDate = value; } }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get { return eDate; } set { eDate = value; } }
    }

    public class SLAPYAudInput : Input
    {
        public string PrimaryAudiences { get; set; }
        public string CreatedBy { get; set; }
        public DateTime eDate = DateTime.Now;
        public DateTime CreatedDate { get { return eDate; } set { eDate = value; } }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get { return eDate; } set { eDate = value; } }
    }

    public class SLAMetricUnit : Input
    {
        public string UnitName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime eDate = DateTime.Now;
        public DateTime CreatedDate { get { return eDate; } set { eDate = value; } }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get { return eDate; } set { eDate = value; } }
    }




}