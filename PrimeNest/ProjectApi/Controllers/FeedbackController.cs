using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjectAPI.DTO;

namespace ProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class FeedbackController : ControllerBase
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IUnitOfWork<FeedBack> feedBackUnitOfWork;

        public FeedbackController(UserManager<AppUser> userManager , IUnitOfWork<FeedBack> feedBackUnitOfWork)
        {
            this.userManager = userManager;
            this.feedBackUnitOfWork = feedBackUnitOfWork;
        }
        [HttpGet("GetFeedBack")]
        //[Authorize("AdminRole")]
        public async Task<IActionResult> FeedBack()
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null) 
                return NotFound();

            var feedbacks = await feedBackUnitOfWork.Entity.GetAllAsync();
            if (feedbacks.Count() == 0)
                return NotFound("Not Found Any Message");
            var feedback = feedbacks.Select(x => new GetFeedBackDTO
            {
                Id = x.Id,
                Email = user.Email,
                Message = x.Message,
                Name = user.Name,
                UrlPhoto = user.Photo,

            });

            return Ok(feedback);
        }


        [HttpPost("AddMessage")]
        //[Authorize("UserRole")]
        public async Task<IActionResult> AddMessage(AddMessageDTO dto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);  

            var user = await userManager.GetUserAsync (User);
            if (user == null)
                return NotFound();

            var feedback = new FeedBack
            {
                Id = Guid.NewGuid().ToString(),
                Message = dto.Message,
                UserId = user.Id,
                User = user,
                DateTime = DateTime.Now,

            };

            await feedBackUnitOfWork.Entity.AddAsync(feedback);
            feedBackUnitOfWork.Save();


            return Ok(feedback);

        }


        [HttpPut("UpdateMessage/{feedBackId}")]
        //[Authorize("AdminRole")]
        public async Task<IActionResult> UpdateMessage(string feedBackId , UpdateMessageDTO dto)
        {
            var feedback = await feedBackUnitOfWork.Entity.GetAsync(feedBackId);
            if (feedback == null) 
                return NotFound();

            feedback.Message = dto.Message ?? feedback.Message;

            await feedBackUnitOfWork.Entity.UpdateAsync(feedback);
            feedBackUnitOfWork.Save();

            return Ok(feedback);

        }

        [HttpDelete("DeleteMessage/{feedBackId}")]
        //[Authorize("AdminRole")]
        public async Task< IActionResult> DeleteMessage(string feedBackId)
        {
            var feedback = await feedBackUnitOfWork.Entity.GetAsync(feedBackId);
            if (feedback == null)
                return NotFound();

            feedBackUnitOfWork.Entity.Delete(feedback);
            feedBackUnitOfWork.Save();
            return Ok("The Message Is Deleted");


        }
    }
}
