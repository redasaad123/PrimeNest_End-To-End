using Core.Interfaces;
using Core.Models;
using Core.Servises;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjectApi.DTOS;

using ProjectApi.NewFolder;
using ProjectApi.Servises;

namespace ProjectApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertyController : ControllerBase
    {
        private readonly PropertyServices _propertyServices;
        private readonly SearchService _searchService;
        private readonly IUnitOfWork<Property> _propertyUnitOfWork;
        private readonly UserManager<AppUser> _userManager;

        public PropertyController(
            PropertyServices propertyServices,
            SearchService searchService,
            IUnitOfWork<Property> propertyUnitOfWork,
            UserManager<AppUser> userManager)
        {
            _propertyServices = propertyServices;
            _searchService = searchService;
            _propertyUnitOfWork = propertyUnitOfWork;
            _userManager = userManager;
        }

        [HttpGet("GetAllProperty")]
        public async Task<IActionResult> GetAllProperty()
        {
            var properties = await _propertyUnitOfWork.Entity.GetAllAsync();
            var filteredProperties = properties.Where(x => x.IsRequested != true).ToList();
            if (!filteredProperties.Any())
                return NotFound("Not Found Any Property");

            var dtoList = filteredProperties.Select(x => new GetPropertyDTO
            {
                Id = x.Id,
                OwnerName = _userManager.Users.Where(u => u.Id == x.user).Select(u => u.Name).FirstOrDefault() ?? x.user,
                TypeContract = x.TypeContract,
                Type = x.Type,
                Address = x.Address,
                Area = x.Area,
                date = x.date,
                Description = x.Description,
                MainPhoto = x.MainPhoto,
                MoreDescription = x.MoreDescription,
                Photo = x.Photo,
                Price = x.Price,
                State = x.State ? "Available" : "Not available"
            });

            return Ok(dtoList);
        }
        [HttpGet("getById/{id}")]
        public async Task<IActionResult> GetPropertyById(string id)
        {
            var property = await _propertyUnitOfWork.Entity.GetAsync(id);
            if (property == null)
                return NotFound("Not Found Any Property");
            var dto = new GetPropertyDTO
            {
                Id = property.Id,
                OwnerName = _userManager.Users.Where(u => u.Id == property.user).Select(u => u.Name).FirstOrDefault() ?? property.user,
                TypeContract = property.TypeContract,
                Type = property.Type,
                Address = property.Address,
                Area = property.Area,
                date = property.date,
                Description = property.Description,
                MainPhoto = property.MainPhoto,
                MoreDescription = property.MoreDescription,
                Photo = property.Photo,
                Price = property.Price,
                State = property.State ? "Available" : "Not available"
            };
            return Ok(dto);

        }



        [HttpPost("Search")]
        public async Task<IActionResult> Search([FromForm] PropertySearchDto dto)
        {
            var result = await _searchService.SearchProperties(dto);
            return Ok(result);
        }

        [HttpPost("AddProperty")]
        public async Task<IActionResult> AddProperty(AddPropertyDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.GetUserAsync(User);
            var property = new Property { Id = Guid.NewGuid().ToString() };
            //Shotgun Surgery smell
            var result = await _propertyServices.AddORUpdateProperty(property, dto, user);
            if (result == null)
                return BadRequest();

            await _propertyUnitOfWork.Entity.AddAsync(property);
            _propertyUnitOfWork.Save();
            return Ok(result);
        }

        [HttpPut("UpdateProperty/{propertyId}")]
        public async Task<IActionResult> UpdateProperty(string propertyId, UpdatePropertyDTO dto)
        {
            var property = await _propertyUnitOfWork.Entity.GetAsync(propertyId);
            if (property == null)
                return BadRequest();

            var user = await _userManager.GetUserAsync(User);
            //Shotgun Surgery smell
            var result = await _propertyServices.AddORUpdateProperty(property, dto, user);
            if (result == null)
                return BadRequest();

            await _propertyUnitOfWork.Entity.UpdateAsync(property);
            _propertyUnitOfWork.Save();
            return Ok(result);
        }

        [HttpDelete("DeleteProperty/{propertyId}")]
        public async Task<IActionResult> DeleteProperty(string propertyId)
        {
            var property = await _propertyUnitOfWork.Entity.GetAsync(propertyId);
            if (property == null)
                return BadRequest();

            _propertyUnitOfWork.Entity.Delete(property);
            _propertyUnitOfWork.Save();
            return Ok(property);
        }
    }
}
