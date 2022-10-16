using InsuranceMVC.DAL.Entities;
using InsuranceMVC.DAL.Entities.Enums;
using InsuranceMVC.DAL.Enums;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static System.Net.Mime.MediaTypeNames;

namespace InsuranceMVCWebApp.Models
{
    public class InsuranceModel
    {
        public int? Id { get; set; }
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "This field is Requiered")]
        public DateTime? DateFrom { get; set; }
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "This field is Requiered")]
        public DateTime? DateTo { get; set; }
        [DataType(DataType.Date)]
        public DateTime? CreatedWhen { get; set; }
        public int? PolicyHolderId { get; set; }
        public bool IsPolicyHolderInsuredPerson { get; set; }
        [Required(ErrorMessage = "This field is Requiered")]
        public InsurancesType SelectedInsuranceType { get; set; }

        public static InsuranceModel MapInsuranceToInsuranceModel(Insurance insurance)
        {
            return new InsuranceModel()
            {
                SelectedInsuranceType=insurance.InsuranceType,
                Id = insurance.Id,
                DateFrom = insurance.DateFrom,
                DateTo = insurance.DateTo,
                CreatedWhen = insurance.CreatedWhen
            };
        }
    }
}
