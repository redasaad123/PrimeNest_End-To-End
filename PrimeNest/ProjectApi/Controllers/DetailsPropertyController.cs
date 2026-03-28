using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectApi.DTOS.InterfaceDTO;
using ProjectApi.Factory;
using System.Threading.Tasks;

namespace ProjectApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetailsPropertyController : ControllerBase
    {
        private readonly IPropertyFactory propertyFactory;

        public DetailsPropertyController(IPropertyFactory propertyFactory)
        {
            this.propertyFactory = propertyFactory;
        }


        [HttpGet("GetDetailsProperty/{type}")]
        public  async Task<IActionResult> GetDetailsProperty(string type)
        {
            var details = await propertyFactory.DetailsProperty(type);
             var result = await details.GetDetails();
            return Ok(result);

        }

        [HttpGet("GetDetailsPropertyID/{type}/{Id}")]
        public async Task< IActionResult> GetDetailsPropertyId(string type , string Id)
        {

            var details =await propertyFactory.DetailsProperty(type);
            var result = await  details.GetDetailsById(Id);
            return Ok(result);

        }

        [HttpPost("AddDetails")]
        public async Task<IActionResult> AddDetails(DetailsPropertyDTO dto )
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var details = await propertyFactory.DetailsProperty(dto.type);
            var result = await details?.AddDetails(dto);
            return Ok(result);

        }

        [HttpPut("UpdateDetails/{Id}")]
        public async Task<IActionResult> UpdateDetails(DetailsPropertyDTO dto , string Id)
        {
            var details = await propertyFactory.DetailsProperty(dto.type);
            var result = await details?.UpdateProperty(dto, Id);
            return Ok(result);
        }

        [HttpDelete("DeleteDetails/{type}/{Id}")]
        public async Task<IActionResult> DeleteDetails(string type , string Id)
        {

            var details = await propertyFactory.DetailsProperty(type);
            var result = await details?.DeleteDetails(Id);
            return Ok(result);

        }










    }
}
