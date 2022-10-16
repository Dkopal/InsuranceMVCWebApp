using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceMVC.DAL.Entities
{
    public class InsuredEvent
    {
        public int Id { get; set; }
        public string Descriptions { get; set; }
        [DataType(DataType.Date)]
        public DateTime CreatedWhen { get; set; }
        public int InsuranceId { get; set; }
        public Insurance Insurance { get; set; }
    }
}
