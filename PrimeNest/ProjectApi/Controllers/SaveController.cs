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
    public class SaveController : ControllerBase
    {
        private readonly IUnitOfWork<Property> propertyUnitOfWork;
        private readonly IUnitOfWork<Save> saveUnitOfWork;
        private readonly UserManager<AppUser> userManager;

        public SaveController(IUnitOfWork<Property> PropertyUnitOfWork, IUnitOfWork<Save> SaveUnitOfWork, UserManager<AppUser> userManager)
        {
            propertyUnitOfWork = PropertyUnitOfWork;
            saveUnitOfWork = SaveUnitOfWork;
            this.userManager = userManager;
        }

        [HttpGet("GetSaves")]
        public async Task<IActionResult> GetSaves()
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null) 
                return NotFound();
            var saves = await saveUnitOfWork.Entity.FindAll(x=>x.UserId == user.Id);
            if (saves == null)
                return NotFound();
             return Ok(saves);

        }


        [HttpPost("AddSave")]
        public async Task<IActionResult > AddSave(AddSaveDTO dto)
        {
            if(!ModelState.IsValid) 
                return BadRequest(ModelState);

            var property = await propertyUnitOfWork.Entity.GetAsync(dto.PropertyId);
            if(property == null)
                return NotFound();

            var user = await userManager.GetUserAsync(User); 
            if (user == null)
                return NotFound();
            var propertySaved = saveUnitOfWork.Entity.Find(x => x.UserId == user.Id && x.PropertyId == dto.PropertyId);
            if (propertySaved != null)
                return Ok("This Property Already Saved ");

            var save = new Save
            {
                Id = Guid.NewGuid().ToString(),
                DateTime = DateTime.Now,
                PropertyId = dto.PropertyId,
                UserId = user.Id,

            };

            await saveUnitOfWork.Entity.AddAsync(save);
            saveUnitOfWork.Save();
            return Ok(save);
        }

        [HttpDelete("DeleteSave/{Id}")]
        public async Task<IActionResult> DeleteSave(string Id)
        {
            var property = await saveUnitOfWork.Entity.GetAsync(Id);
            if (property == null)
                return NotFound();

            saveUnitOfWork.Entity.Delete(property);
            saveUnitOfWork.Save();
            return Ok();

        }

    }
}
