using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Omu.ValueInjecter;
using NTTData.AMOPortal.Domain.Abstract;
using NTTData.AMOPortal.Domain.Entities;

namespace NTTData.AMOPortal.WebUI.Mapper
{
    public class IntsToEntities : ConventionInjection
    {
        protected override bool Match(ConventionInfo c)
        {
            if (c.SourceProp.Name != c.TargetProp.Name) return false;
            var s = c.SourceProp.Type;
            var t = c.TargetProp.Type;

            if (!s.IsGenericType || !t.IsGenericType
                || s.GetGenericTypeDefinition() != typeof(IEnumerable<>)
                || t.GetGenericTypeDefinition() != typeof(ICollection<>)) return false;

            return s.GetGenericArguments()[0] == (typeof(int))
                   && (t.GetGenericArguments()[0].IsSubclassOf(typeof(Entity)));
        }

        protected override object SetValue(ConventionInfo c)
        {
            return new object(); 
        }
    }
}