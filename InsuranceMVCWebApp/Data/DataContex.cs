using InsuranceMVC.DAL.Entities;
using InsuranceMVCWebApp.Models;
using Microsoft.EntityFrameworkCore;


namespace InsuranceMVCWebApp.Data
{
    public class DataContex :DbContext
    {
        public DataContex(DbContextOptions<DataContex> options) : base (options)
        {
            
        }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Insurance> Insurances { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<InsurancePerson> InsurancePersons { get; set; }
        public DbSet<InsuredEvent> InsuredEvents { get; set; } 
    }
}
