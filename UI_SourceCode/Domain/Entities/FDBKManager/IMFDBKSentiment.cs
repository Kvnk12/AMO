using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.FDBKManager
{
    [Table("IMFDBKSentiment")]
    public class IMFDBKSentiment
    {
        public int Id { get; set; }
        public string SentimentName { get; set; }
    }
}
