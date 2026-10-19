using System.Collections.Generic;

using NTTData.AMOPortal.Domain.Entities;
using WebUI.Areas.AlignmentManager.Objects;

namespace WebUI.Areas.AlignmentManager.Models
{
    public class VariableMetadataAddModel : VariableModel
    {
            public VariableMetadataAddModel(int variableId)
                : base(variableId)
                {
                InitializeValues(AlignmentHelper.GetById<mdmVariable>(variableId));
                }

            public VariableMetadataAddModel(mdmVariable variable)
                : base(variable)
                {
                InitializeValues(variable);
                }

        private void InitializeValues(mdmVariable variable)
        {
            Values.Clear();
            Scores.Clear();
            foreach (VariableMDType type in Types)
            {
                if(!Values.ContainsKey(type.TypeId))
                    Values.Add(type.TypeId, string.Empty);
                if(!Scores.ContainsKey(type.TypeId))
                    Scores.Add(type.TypeId, string.Empty);
            }
        }

        public IList<VariableMDType> Types
        {
            get { return AlignmentHelper.All<VariableMDType>(); }
        }

        public Dictionary<int, string> Values = new Dictionary<int, string>();

        public Dictionary<int, string> Scores = new Dictionary<int, string>();
        
    }
}