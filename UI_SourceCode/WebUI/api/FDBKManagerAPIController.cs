using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using NTTData.AMOPortal.WebUI.Filters;
using WebMatrix.WebData;
using WebUI.Areas.FDBKManager;

namespace WebUI.api
{
    public class FeedbackAPIModel
    {
        public int Typeid { get; set; }
        public int Productid { get; set; }
        public int Digitalid { get; set; }
        public string ShortDescription { get; set; }
        public string DetailDescription { get; set; }
   //     public int ClientId { get; set; }
        public string InternalClientId { get; set; }
        public string CreatedBy { get; set; }
        public string sentiment { get; set; }
    //    public string CreatorName { get; set; }
    }
    public class FDBKManagerAPIController : ApiController
    {
       
        [APIAuthentication(1, 19)]
        public HttpResponseMessage Post(FeedbackAPIModel feedBackInput)
        {
            IEnumerable<string> ApiKeyHeaderValues = null;
            int ClientId=0;
            if (ActionContext.Request.Headers.TryGetValues("X-AMOClientKey", out ApiKeyHeaderValues))
            {
                string[] apiKeyHeaderValue = ApiKeyHeaderValues.First().Split(':');
                int.TryParse(apiKeyHeaderValue[0], out ClientId);
            }
            int UserId = 0;
            UserId = FMHelper.CheckForUser(feedBackInput.CreatedBy, ClientId);
            if (UserId == 0)
            {
                string CreatedByuserName = Thread.CurrentPrincipal.Identity.Name;
                //string CreatedByuserName = feedBackInput.CreatedByUserName; //if we use this then there could be security issue
                UserId = WebSecurity.GetUserId(CreatedByuserName);
            }
            
            int typeid = 1;
            int Productid = feedBackInput.Productid;
            int Digitalid = feedBackInput.Digitalid;
            string ShortDescription = feedBackInput.ShortDescription;
            string DetailDescription = "";
            //  int ClientId = feedBackInput.ClientId;
            string OriginatedFrom = feedBackInput.CreatedBy;
            string InternalClientId = feedBackInput.InternalClientId; //should be any array
            string sentimentName = feedBackInput.sentiment;
            int resultid = FMHelper.saveFeedback(typeid, Productid, Digitalid, ShortDescription, DetailDescription, InternalClientId, "Post", UserId, ClientId,0, OriginatedFrom, sentimentName);
            var message = Request.CreateResponse(HttpStatusCode.Created, resultid);
            message.Headers.Location = new Uri(Request.RequestUri + resultid.ToString());
            return message;
            //insert into feedback
            //return Ok(resultid);
        }

    }
}