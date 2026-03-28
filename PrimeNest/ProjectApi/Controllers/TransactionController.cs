using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjectApi.DTOS;
using ProjectApi.DTOS.InterfaceDTO;

namespace ProjectApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {


        private readonly UserManager<AppUser> userManager;
        private readonly IUnitOfWork<Property> propertyunitOfWork;
        private readonly IUnitOfWork<Transactions> transactionunitOfWork;

        public TransactionController(UserManager<AppUser> userManager, IUnitOfWork<Property> PropertyunitOfWork, IUnitOfWork<Transactions> TransactionunitOfWork)
        {
            this.userManager = userManager;
            propertyunitOfWork = PropertyunitOfWork;
            transactionunitOfWork = TransactionunitOfWork;
        }

        [HttpGet("GetTransaction")]
        public async Task< IActionResult> GetTransaction()
        {
            var Transaction = await transactionunitOfWork.Entity.GetAllAsync();

            if (Transaction == null)
                return NotFound();

            var result = Transaction.Select(x => new GetTransactionDTO
            {
                Id = x.ID,
                Type = x.Type,
                BuyerID = x.BuyerID,
                Date = x.Date,
                Price = x.Price,
                PropertyID = x.PropertyID,
                SellerID = x.SellerID,
            });

            return Ok(result);


        }


        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var Transaction = await transactionunitOfWork.Entity.GetAsync(id);
            if (Transaction == null)
                return NotFound();
            var result = new GetTransactionDTO
            {
                Id = Transaction.ID,
                Type = Transaction.Type,
                BuyerID = Transaction.BuyerID,
                Date = Transaction.Date,
                Price = Transaction.Price,
                PropertyID = Transaction.PropertyID,
                SellerID = Transaction.SellerID,
            };

            return Ok(result);

        }


        [HttpPost("AddTransaction")]
        public async Task<IActionResult> AddTransaction(AddTransactionDTO dto )
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await userManager.GetUserAsync(User);
            Transactions op = new Transactions
            {
                ID = Guid.NewGuid().ToString(),
                Type = "Transaction",
                BuyerID = dto.BuyerID,
                Date = DateTime.Now,
                Price = dto.Price,
                PropertyID = dto.PropertyID,
                SellerID = user.Id,

            };

            await transactionunitOfWork.Entity.AddAsync(op);

            var Message = await ChangeOwner(dto.PropertyID, dto.BuyerID);

            transactionunitOfWork.Save();

            return Ok(op);

        }



        [HttpPut("UpdateTransaction/{id}")] 
        public async Task<IActionResult> UpdateTransaction(UpdateTransactionDTO DTO, string id)
        {
            var Transaction = await transactionunitOfWork.Entity.GetAsync(id);
            if (Transaction == null)
                return NotFound();

            Transaction.Price = DTO.Price ?? Transaction.Price;
            Transaction.BuyerID = DTO.BuyerID ?? Transaction.BuyerID;
            Transaction.PropertyID = DTO.PropertyID ?? Transaction.PropertyID;
            Transaction.Date = DateTime.Now;

            await transactionunitOfWork.Entity.UpdateAsync(Transaction);
            var Message = await ChangeOwner(DTO.PropertyID, DTO.BuyerID);
            transactionunitOfWork.Save();
            return Ok(Transaction);

        }



        [HttpDelete("DeleteTransaction/{id}")]
        public async Task<IActionResult> DeleteTransaction(string id)
        {
            var Transaction = await transactionunitOfWork.Entity.GetAsync(id);
            if (Transaction == null)
                return NotFound();

            transactionunitOfWork.Entity.Delete(Transaction);
            transactionunitOfWork.Save();
            return Ok();

        }


        private async Task<string> ChangeOwner(string PropertyId, string BuyerId)
        {
            var property = await propertyunitOfWork.Entity.GetAsync(PropertyId);
            if (property == null)
                return "This Property Not Found";

            var user = await userManager.FindByIdAsync(BuyerId);
            if (user == null)
                return "This user Not Found";

            property.user = userManager.Users.Where(u => u.Id == BuyerId).Select(u => u.Name).FirstOrDefault() ?? "Yhis User Unknown";

            await propertyunitOfWork.Entity.UpdateAsync(property);
            //propertyunitOfWork.Save();

            return $"The process change successfully for {property.user} ";




        }




    }
}
