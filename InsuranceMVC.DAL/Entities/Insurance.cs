using InsuranceMVC.DAL.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace InsuranceMVC.DAL.Entities
{
    public class Insurance
    {
        public int? Id { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DateFrom { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DateTo { get; set; }
        [DataType(DataType.Date)]
        public DateTime? CreatedWhen { get; set; }

        public InsurancesType InsuranceType { get; set; }
        public ICollection<InsurancePerson> InsurancePerson  { get; set; } = new HashSet<InsurancePerson>();
        public ICollection<InsuredEvent> InsuredEvents { get; set; } = new HashSet<InsuredEvent>();
    }
}
