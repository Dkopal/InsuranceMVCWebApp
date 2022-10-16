using InsuranceMVC.DAL.Entities;
using InsuranceMVC.DAL.Enums;
using System.ComponentModel.DataAnnotations;

namespace InsuranceMVCWebApp.Models
{
    public class PersonAddressModel
    {
        public int? Id { get; set; }
        [Required(ErrorMessage = "This field is Requiered")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "This field is Requiered")]
        public string? Surname { get; set; }
        [Required(ErrorMessage = "This field is Required")]
        public string? Phone { get; set; }
        [Required(ErrorMessage = "This field is Requiered")]
        //regulární výraz pro email
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$",
        ErrorMessage = "Please enter a valid email address")]
        public string? Email { get; set; }
        //regulární výraz pro Pid-CZ
        [Required(ErrorMessage = "This field is Requiered")]
        [RegularExpression(@"[0-9]{2}[01567][0-9][0-3][0-9][ \/][0-9]{3,4}",
        ErrorMessage = "Please enter a valid personal identification number")]
        public string? Pid { get; set; }
        [Required(ErrorMessage = "This field is Requiered")]
        public string? Street { get; set; }
        [Required(ErrorMessage = "This field is Requiered")]
        public string? StreetNumber { get; set; }
        [Required(ErrorMessage = "This field is Requiered")]
        public string? City { get; set; }
        //regulární výraz pro směrovací číslo ČR
        [Required(ErrorMessage = "This field is Requiered")]
        [RegularExpression(@"\d{3}[ ]?\d{2}",
        ErrorMessage = "Please enter a valid personal identification number")]
        public string? PostalCode { get; set; }

        public static PersonAddressModel MapPersonToPersonAddressModel(Person person)
        {
            return new PersonAddressModel()
            {
                Id = person.Id,
                Name = person.Name,
                Surname = person.Surname,
                Phone = person.Phone,
                Email = person.Email,
                Pid = person.Pid,
                Street = person.Address?.Street,
                StreetNumber = person.Address?.StreetNumber,
                City = person.Address?.City,
                PostalCode = person.Address?.PostalCode
            };
        }
    }
}
