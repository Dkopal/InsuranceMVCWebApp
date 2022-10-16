using InsuranceMVC.DAL.Entities;
using InsuranceMVC.DAL.Entities.Enums;
using InsuranceMVC.DAL.Enums;
using System;

namespace InsuranceMVCWebApp.Models
{
    public class InsurancePersonModel
    {
        public Insurance Insurance { get; set; }

        public Person PolicyHolder { get; set; }
        public List<Person> PersonAsInsuredPerson { get; set; }
        public List<InsuredEventModel> InsuredEventModels { get; set; }

        public static InsurancePersonModel MapIsnuranceToInsuranceDTO(Insurance insurance)
        {
            return new InsurancePersonModel()
            {
                Insurance = insurance,
                PolicyHolder=insurance.InsurancePerson.Where(x=>x.PersonType == PersonType.PolicyHolder).Select(x=>x.Person).FirstOrDefault(),
                PersonAsInsuredPerson = insurance.InsurancePerson.Where(x => x.PersonType == PersonType.InsuredPerson).Select(x => x.Person).ToList(),
                InsuredEventModels=insurance.InsuredEvents.Select(x=>InsuredEventModel.MapInsuredEventToInsuredEventModel(x)).ToList(),
            };
        }
    }
}

