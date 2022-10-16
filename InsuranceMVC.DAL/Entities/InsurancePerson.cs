using InsuranceMVC.DAL.Enums;

namespace InsuranceMVC.DAL.Entities
{
    public class InsurancePerson
    {
        public int? Id { get; set; }
        public int? InsuranceId { get; set; }
        public int? PersonId { get; set; }
        public PersonType PersonType { get; set; } 
        public Person? Person { get; set; }

        public Insurance? Insurance { get; set; }
    }
}
