using System.ComponentModel.DataAnnotations;

namespace InsuranceMVC.DAL.Entities.Enums
{
    public enum InsurancesType
    {
        [Display(Name = "Life insurance")]
        LifeInsurance= 1,
        [Display(Name = "Travel insurance")]
        TravelInsurance = 2,
        [Display(Name = "Accident insurance")]
        AccidentInsurance = 3,

    }
}
