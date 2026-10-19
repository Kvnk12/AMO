using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Domain.Entities
{
    [Table("BusinessProcessImportance")]
    public class BusinessProcessImportance
    {
        public int Id { get; set; }

        public string Processimportance { get; set; }

        public int Clientid { get; set; }

    }
}
