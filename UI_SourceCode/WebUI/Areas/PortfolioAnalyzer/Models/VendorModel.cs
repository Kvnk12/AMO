using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace WebUI.Areas.PortfolioAnalyzer.Models
{
    public class VendorModel
    {
        public int Id { get; set; }
       
        [UIHint("String")]
        public string VendorName { get; set; }
    }
}