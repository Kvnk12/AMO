using Data;
using Domain.Entities;
using NTTData.AMOPortal.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace NttDataOptimizationToolServices
{
    public class ServiceHelper
    {
        public static List<LoggedDevices> getLoggedInDevices(ClientDetail model)
        {
            int UserId = 0;
            var IsUserId = int.TryParse(model.UserName, out UserId);
            if (IsUserId)
            {
                var contextFactory = new DbContextFactory();
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                //Getting all devices for that User- Irrespective of Client
                var deviceList = unitOfWork.MFAUserLoggedInDevice.Get(g => g.UserId == UserId && g.IsDeleted == false)
                    .Select(f=> new LoggedDevices {
                        DeviceName = f.DeviceName,
                        MFADeviceId = f.Id
                }).ToList();
                return deviceList;
            }
            else
            {
                var re = new List<LoggedDevices>();
                return re;
            }
        }

        public static bool deleteDevice(LoggedDevices Device)
        {
            try
            {
                var contextFactory = new DbContextFactory();
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var getEntity = unitOfWork.MFAUserLoggedInDevice.GetByID(Device.MFADeviceId);
                getEntity.IsDeleted = true;
                unitOfWork.MFAUserLoggedInDevice.Update(getEntity);
                unitOfWork.Save();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }

        }

        private static void UserPhoneEmailVerified(int UId, EnumMFAType mfatype)
        {
            try
            {
                var contextFactory = new DbContextFactory();
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var entity = unitOfWork.UserProfile.GetByID(UId);
                if (mfatype == EnumMFAType.sms)
                {
                    entity.IsPhoneVerified = true;
                    unitOfWork.UserProfile.Update(entity);
                }
                else if(mfatype == EnumMFAType.email)
                {
                    entity.IsEmailVerified = true;
                    unitOfWork.UserProfile.Update(entity);
                }
                unitOfWork.Save();
            }
            catch
            {

            }
        }

        public static MFAOTPStatus VerifyOTP(MFAOTP model)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var otpLog= unitOfWork.MFAUserAuthLogging.GetByID(model.otpID);
            if (otpLog.OTP == model.OTP && DateTime.Now<=otpLog.RequestedTime.AddMinutes(3))
            {
                //response
                var retModel= new MFAOTPStatus();
                retModel.Status = true;
                retModel.Message = "Correct OTP";

                //Update the DB wityh the verified time
                otpLog.VerifiedTime = DateTime.Now;
                unitOfWork.MFAUserAuthLogging.Update(otpLog);

                //if user has checked "Don't ask me again", then save the device information into the device log table
                if (model.RememberDevice)
                {
                    var entity = new MFAUserLoggedInDevice()
                    {
                        ClientId = model.Client.ClientId,
                        CreatedDt = DateTime.Now,
                        DeviceIP = model.Client.ClientIP,
                        DeviceName = model.Client.ClientDevice,
                        IsDeleted = false,
                        MFATypeId = model.OTPType,
                        UserId = Convert.ToInt32(model.Client.UserName)
                    };

                    unitOfWork.MFAUserLoggedInDevice.Insert(entity);
                }
                unitOfWork.Save();
                //Marking that user phone number as verified.
                UserPhoneEmailVerified(otpLog.UserId, (EnumMFAType)model.OTPType);
                return retModel;
            }
            else
            {
                var retModel = new MFAOTPStatus();
                retModel.Status = false;
                if (otpLog.OTP == model.OTP)
                {
                    retModel.OTPExpired = true;
                    retModel.Message = "OTP Expired";
                }
                else
                {
                    retModel.OTPExpired = false;
                    retModel.Message = "Wrong OTP";
                }
                return retModel;
                
            }
        }





        public static string GenerateNumericOTP(int iOTPLength = 4)
        {
            string[] saAllowedCharacters = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
            string sOTP = String.Empty;
            string sTempChars = String.Empty;
            Random rand = new Random();
            for (int i = 0; i < iOTPLength; i++)
            {
                int p = rand.Next(0, saAllowedCharacters.Length);
                sTempChars = saAllowedCharacters[rand.Next(0, saAllowedCharacters.Length)];
                sOTP += sTempChars;
            }
            return sOTP;
        }


        public static string GetUserPhone(string userid)
        {
            int userId;
            string PhoneNumber="";
            string CountryCode="";
            var IsUserId = int.TryParse(userid, out userId);
           
            if (IsUserId) {
                var contextFactory = new DbContextFactory();
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var us = unitOfWork.UserProfile.GetByID(userId);
                if (us != null)
                {
                    PhoneNumber = us.Phone;
                    CountryCode = us.CountryCode.ToString();
                    return "+" + CountryCode.Trim() + PhoneNumber.Trim();
                }
            }
            return "";
        }

        public static string GetUserEmail(string userid)
        {
            int userId;
            string emailId = "";
           // string CountryCode;
            var IsUserId = int.TryParse(userid, out userId);

            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            if (IsUserId)
            {
                var us = unitOfWork.UserProfile.GetByID(userId);
                if (us != null)
                {
                    emailId = us.Email;
                }
            }
            
            return emailId;
        }


        

        public static int logOTPRequest(string OTP, int UserId, int ClientId, int MFAType, string DeviceName, string DeviceIP)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var entity = new MFAUserAuthLogging()
            {
                ClientId = ClientId,
                UserId = UserId,
                OTP = OTP,
                MFATypeId = MFAType,
                DeviceIP = DeviceIP,
                DeviceName = DeviceName,
                RequestedTime = DateTime.Now,
            };
            unitOfWork.MFAUserAuthLogging.Insert(entity);
            unitOfWork.Save();
            return entity.Id;
        }

    }
}