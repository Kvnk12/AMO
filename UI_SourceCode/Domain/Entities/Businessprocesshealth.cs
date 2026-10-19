using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Domain.Entities
{
    [Table("Businessprocesshealth")]
    public class Businessprocesshealth
    {
        public int Id { get; set; }

        public string ProcessHealth { get; set; }

        public int Clientid { get; set; }
    }
}
