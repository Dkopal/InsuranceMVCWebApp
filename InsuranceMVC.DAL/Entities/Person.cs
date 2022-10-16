namespace InsuranceMVC.DAL.Entities
{
    public class Person
    {
        public int? Id { get; set; }
        public string? Name { get; set; } 
        public string? Surname { get; set; } 
        public string? Phone { get; set; }
        public string? Email { get; set; }  
        public string? Pid { get; set; }
        public int? AddressId { get; set; }
        public Address? Address { get; set; }
        public ICollection<InsurancePerson> InsurancePerson { get; set; } = new HashSet<InsurancePerson>();

    }
}
