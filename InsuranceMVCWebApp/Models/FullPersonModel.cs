using InsuranceMVC.DAL.Entities;
using InsuranceMVC.DAL.Enums;

namespace InsuranceMVCWebApp.Models
{
    public class FullPersonModel
    {
        public Person? Person { get; set; }
        
        public List<Insurance?> InsurancesAsPolicyHolder { get; set; }
        public List<Insurance?> InsurancesAsInsuredPerson { get; set; }

        public static FullPersonModel MapPersonToFullPersonDTO(Person person)
        {
            return new FullPersonModel()
            {
                Person = person,
                InsurancesAsInsuredPerson = person.InsurancePerson.Where(x => x.PersonType == PersonType.InsuredPerson).Select(x => x.Insurance).ToList(),
                InsurancesAsPolicyHolder = person.InsurancePerson.Where(x => x.PersonType == PersonType.PolicyHolder).Select(x => x.Insurance).ToList()
            };
        }
    }

    
}
