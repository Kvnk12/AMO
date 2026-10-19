using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using NTTData.AMOPortal.Domain.Entities;

namespace Service
{
    public static class CoreLib
    {
        public static IEnumerable<SelectListItem> ToSelectListItems(
              this IEnumerable<UserRole> roles, int selectedId)
        {
            return
                roles.OrderBy(role => role.RoleName)
                      .Select(role =>
                          new SelectListItem
                          {
                              Selected = (role.Id == selectedId),
                              Text = role.RoleName,
                              Value = role.Id.ToString()
                          });
        }
    }
}
