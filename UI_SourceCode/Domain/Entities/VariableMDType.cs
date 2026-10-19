using System.ComponentModel.DataAnnotations.Schema;
using NTTData.AMOPortal.Domain.Entities;

namespace NTTData.AMOPortal.Domain.Entities
{
    [Table("mdmVariableMDType")]
    public class VariableMDType : DelEntity, ITypeClass
    {
        public int TypeId { get { return Id; } }

        public string TypeName { get; set; }
    }
}
