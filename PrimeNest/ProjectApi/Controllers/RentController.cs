using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjectApi.DTOS;

namespace ProjectApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentController : ControllerBase
    {
        private readonly IUnitOfWork<Property> propertyUnitOfWork;
        private readonly UserManager<AppUser> userManager;
        private readonly IUnitOfWork<Rent> rentUnitOfWork;

        public RentController(IUnitOfWork<Property> PropertyUnitOfWork, UserManager<AppUser> userManager, IUnitOfWork<Rent> RentUnitOfWork)
        {
            propertyUnitOfWork = PropertyUnitOfWork;
            this.userManager = userManager;
            rentUnitOfWork = RentUnitOfWork;
        }



        [HttpGet("GetRents")]
        public async Task<IActionResult> GetRents()
        {
            var rents = await rentUnitOfWork.Entity.GetAllAsync();
            if (rents == null)
                return NotFound();

            var result = rents.Select(x => new GetRentsDTO
            {
                ID = x.ID,
                Date = x.Date,
                PropertyID = x.PropertyID,
                DateEnd = x.DateEnd,
                OwnerId = x.OwnerId,
                Price = x.Price,
                TenantId = x.TenantId,
                Type = x.Type,

            });

            return Ok(result);

        }


        [HttpGet("GetById/{id}")]

        public async Task<IActionResult> GetById(string id)
        {
            var rent = await rentUnitOfWork.Entity.GetAsync(id);
            if (rent == null)
                return NotFound();

            var result = new GetRentsDTO
            {
                ID = rent.ID,
                Date = rent.Date,
                PropertyID = rent.PropertyID,
                DateEnd = rent.DateEnd,
                OwnerId = rent.OwnerId,
                Price = rent.Price,
                TenantId = rent.TenantId,
                Type = rent.Type,
            };


            return Ok(result);

        }



        [HttpPost("AddRent")]
        public async Task<IActionResult> AddRent(AddRentDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await userManager.GetUserAsync(User);

            var rent = new Rent
            {
                ID = Guid.NewGuid().ToString(),
                Date = DateTime.Now,
                DateEnd = dto.DateEnd,
                Type = "Rent",
                OwnerId = user.Id,
                Price = dto.Price,
                PropertyID = dto.PropertyID,
                TenantId = dto.TenantId,
                //User = user,
            };
            await rentUnitOfWork.Entity.AddAsync(rent);
            await ChangeState(dto.PropertyID);
            rentUnitOfWork.Save();
            return Ok(rent);

        }

        [HttpPut("UpdateRent/{Id}")]
        public async Task<IActionResult> UpdateRent(string Id, UpdateRentDTO dto)
        {
            var rent = await rentUnitOfWork.Entity.GetAsync(Id);
            if (rent == null)
                return NotFound();

            rent.Date = DateTime.Now;
            rent.DateEnd = dto.DateEnd;
            rent.Price = dto.Price;
            rent.TenantId = dto.TenantId;

            await rentUnitOfWork.Entity.UpdateAsync(rent);

            rentUnitOfWork.Save();
            return Ok(rent);

        }


        [HttpDelete("DeleteRent/{Id}")]
        public async Task<IActionResult> DeleteRent(string PropertyID, string Id)
        {
            var rent = await rentUnitOfWork.Entity.GetAsync(Id);
            if (rent == null)
                return NotFound();
            rentUnitOfWork.Entity.Delete(rent);
            await ChangeState(PropertyID);
            rentUnitOfWork.Save();
            return Ok(rent);

        }
















        private async Task<string> ChangeState(string propretyid)
        {
            var property = await propertyUnitOfWork.Entity.GetAsync(propretyid);
            if (property == null)
                return "This property NotFound";

            property.State = !property.State;

            await propertyUnitOfWork.Entity.UpdateAsync(property);

            return "ChangeState";

        }





    }
}
