using InsuranceMVC.DAL.Entities;
using InsuranceMVC.DAL.Enums;
using InsuranceMVCWebApp.Data;
using InsuranceMVCWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace InsuranceMVCWebApp.Controllers
{
    [Route("api/persons")]
    [ApiController]
    public class FullPersonController : Controller
    {
        private readonly DataContex _context;

        public FullPersonController(DataContex context)
        {
            _context = context;
        }

        [HttpGet("index")]
        public async Task<IActionResult> Index(int? page)
        {
            var persons = _context.Persons
                .Include(p => p.Address);

            var fullPersons = await persons.Select(p => FullPersonModel.MapPersonToFullPersonDTO(p)).ToListAsync();
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return View(fullPersons.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet("details/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || await _context.Persons.FindAsync(id) == null)
            {
                return NotFound();
            }

            var persons = _context.Persons
                .Include(p => p.Address)
            .Include(p => p.InsurancePerson).ThenInclude(p => p.Insurance);

            var fullPerson = await persons.Where(p => p.Id == id).Select(p => FullPersonModel.MapPersonToFullPersonDTO(p)).FirstOrDefaultAsync();

            if (fullPerson == null)
            {
                return NotFound();
            }

            return View(fullPerson);
        }
        // GET: Person/Create
        [HttpGet("create")]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        // POST: Person/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] PersonAddressModel personAddress)
        {
            if (ModelState.IsValid)
            {
                if (!_context.Persons.Any(x => x.Pid == personAddress.Pid))
                {
                    var person = new Person()
                    {
                        Name = personAddress.Name,
                        Surname = personAddress.Surname,
                        Phone = personAddress.Phone,
                        Email = personAddress.Email,
                        Pid = personAddress.Pid
                    };
                    var address = new Address()
                    {
                        Street = personAddress.Street,
                        StreetNumber = personAddress.StreetNumber,
                        City = personAddress.City,
                        PostalCode = personAddress.PostalCode
                    };

                    person.Address = address;
                    _context.Add(person);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return BadRequest("This person already exists!");
                }
            }

            return View(personAddress);
        }

        // GET: Person/Edit/5
        [HttpGet("edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || await _context.Persons.FindAsync(id) == null)
            {
                return NotFound();
            }

            var person = await _context.Persons.FindAsync(id);
            var address = await _context.Addresses.FindAsync(person?.AddressId);

            if (person == null || address == null)
            {
                return NotFound();
            }

            person.Address = address;

            return View(PersonAddressModel.MapPersonToPersonAddressModel(person));
        }

        [HttpPost("edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [FromForm] PersonAddressModel personAddress)
        {
            if (id == null || await _context.Persons.FindAsync(id) == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var person = await _context.Persons.FindAsync(id);
                    var address = await _context.Addresses.FindAsync(person?.AddressId);

                    if (person == null || address == null)
                    {
                        return NotFound();
                    }

                    person.Name = personAddress.Name;
                    person.Surname = personAddress.Surname;
                    person.Phone = personAddress.Phone;
                    person.Email = personAddress.Email;
                    person.Pid = personAddress.Pid;

                    address.Street = personAddress.Street;
                    address.StreetNumber = personAddress.StreetNumber;
                    address.City = personAddress.City;
                    address.PostalCode = personAddress.PostalCode;

                    _context.Update(person);
                    _context.Update(address);
                    await _context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
                return RedirectToAction("Details", new { id = personAddress.Id });
            }
            return View(personAddress);
        }

        [HttpGet("remove/{id}/{secondId}/{personType}")]
        public async Task<IActionResult> Remove(int? id, int? secondId, int? personType)
        {
            var personId = id;
            var insuranceId = secondId;
            if (personId != null && insuranceId != null && personType != null &&
                await _context.Persons.FindAsync(personId) != null &&
                await _context.Insurances.FindAsync(insuranceId) != null)
            {
                _context.Remove(_context.InsurancePersons.Single(x => x.PersonId == personId && x.InsuranceId == insuranceId && x.PersonType == (PersonType)personType));
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Insurance", new { id = insuranceId });
            }

            return BadRequest();
        }


        [HttpGet("add/{id}")]
        public async Task<IActionResult> Add(int? id)
        {
            if (id == null || await _context.Insurances.FindAsync(id) == null)
            {
                return NotFound();
            }
            var insuranceAddPerson = new InsuranceAddPersonModel()
            {
                InsuranceId = id.Value
            };
            return View(insuranceAddPerson);
        }

        [HttpPost("add/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(int? id, [FromForm] InsuranceAddPersonModel insuranceAddPersonModel)
        {
            if (ModelState.IsValid)
            {
                if (_context.Insurances.Any(x => x.Id == insuranceAddPersonModel.InsuranceId))
                {
                    if (_context.Persons.Any(x => x.Pid == insuranceAddPersonModel.Pid))
                    {
                        var person = _context.Persons.Where(x => x.Pid == insuranceAddPersonModel.Pid).FirstOrDefault();
                        var insurancePerson = new InsurancePerson()
                        {
                            InsuranceId = insuranceAddPersonModel.InsuranceId,
                            PersonId = person.Id,
                            PersonType = PersonType.InsuredPerson
                        };
                        _context.Add<InsurancePerson>(insurancePerson);
                        await _context.SaveChangesAsync();
                        return RedirectToAction("Details", "Insurance", new { id = insuranceAddPersonModel.InsuranceId });
                    }
                    else
                    {
                        TempData["InsuranceId"] = insuranceAddPersonModel.InsuranceId;
                        return RedirectToAction("PersonNotExist");
                    }
                }
                else
                {
                    return NotFound();
                }

            }

            return View(insuranceAddPersonModel);
        }

        [HttpGet("personNotExist")]
        public async Task<IActionResult> PersonNotExist()
        {

            return View();
        }

        [HttpGet("delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || await _context.Persons.FindAsync(id) == null)
            {
                return NotFound();
            }

            try
            {
                var person = await _context.Persons.FindAsync(id);
                if (person == null)
                {
                    return NotFound();
                }

                //Remove all insurances as policyHolder
                var insurancesAsPolicyHolder = _context.InsurancePersons.Where(x => x.PersonId == id && x.PersonType == PersonType.PolicyHolder);

                if (insurancesAsPolicyHolder.Any())
                {
                    foreach (var insurance in insurancesAsPolicyHolder)
                    {
                        var insuranceItem = await _context.Insurances.FindAsync(insurance.InsuranceId);
                        _context.RemoveRange(_context.InsurancePersons.Where(x => x.InsuranceId == insuranceItem.Id));
                        _context.RemoveRange(_context.InsuredEvents.Where(x => x.InsuranceId == insuranceItem.Id));
                        _context.RemoveRange(_context.Insurances.Where(x => x.Id == insuranceItem.Id));                       
                    }

                    await _context.SaveChangesAsync();
                }

                //Find out if person has insurances as insured person
                var insurancesAsInsuredPerson = _context.InsurancePersons.Where(x => x.PersonId == id && x.PersonType == PersonType.InsuredPerson);
              
                if(!insurancesAsInsuredPerson.Any())
                {
                    //if person has any insurances we can delete this person
                    _context.Remove<Person>(person);
                    var address = await _context.Addresses.FindAsync(person.AddressId);
                    _context.Remove<Address>(address);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction("Index", "FullPerson");
            }
            catch (Exception e)
            {
                return BadRequest();
            }

        }
    }
}
