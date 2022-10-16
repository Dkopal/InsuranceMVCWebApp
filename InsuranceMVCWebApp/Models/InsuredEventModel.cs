using InsuranceMVC.DAL.Entities;
using System.ComponentModel.DataAnnotations;

namespace InsuranceMVCWebApp.Models
{
    public class InsuredEventModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "This field is Requiered")]
        public string Descriptions { get; set; }
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "This field is Requiered")]
        public DateTime? CreatedWhen { get; set; }
        public int InsuranceId { get; set; }
        
        public static InsuredEventModel MapInsuredEventToInsuredEventModel(InsuredEvent insuredEvent)
        {

            return new InsuredEventModel()
            {
                Id = insuredEvent.Id,
                Descriptions = insuredEvent.Descriptions,
                CreatedWhen = insuredEvent.CreatedWhen,
                InsuranceId = insuredEvent.InsuranceId
            };
        }
    }
}
