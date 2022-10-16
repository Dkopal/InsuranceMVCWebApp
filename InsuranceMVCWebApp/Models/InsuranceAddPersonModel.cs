using System.ComponentModel.DataAnnotations;

namespace InsuranceMVCWebApp.Models
{
    public class InsuranceAddPersonModel
    {
        public int InsuranceId { get; set; }
        [Required(ErrorMessage = "This field is Requiered")]
        [RegularExpression(@"[0-9]{2}[01567][0-9][0-3][0-9][ \/][0-9]{3,4}",
       ErrorMessage = "Please enter a valid personal identification number")]
        public string? Pid { get; set; }
    }
}
