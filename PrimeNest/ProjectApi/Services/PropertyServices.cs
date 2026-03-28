using Core.Models;
using Core.Servises;
using ProjectApi.DTOS.InterfaceDTO;

namespace ProjectApi.Servises
{
    // Separate this class from class Service
    // Large CLASS Smell
    public class PropertyServices
    {
        private readonly ImageService _imageService;

        //Feature Envy
        public PropertyServices(ImageService imageService)
        {
            _imageService = imageService;

        }



        //Rename METHOD form Help to AddORUpdateProperty
        //long method smell
        
        public async Task<Property> AddORUpdateProperty(Property property, IPropertyDTO dto, AppUser user)
        {
            await AddORUpdatePhotosAsync(property, dto);
            AddORUpdatePropertyFields(property, dto, user);
            await AddORUpdateMainPhotoAsync(property, dto);
            AddORUpdateStateForRent(property, dto);

            return property;
        }

        private async Task AddORUpdatePhotosAsync(Property property, IPropertyDTO dto)
        {
            if (dto.Photo != null && dto.Photo.Count > 0)
            {
                property.Photo = new List<string>();
                foreach (var image in dto.Photo)
                {
                    if (image.Length > 0)
                    {
                        property.Photo.Add(await _imageService.CompressAndSaveImageAsync(image, "Photos"));
                    }
                }
            }
        }

        private void AddORUpdatePropertyFields(Property property, IPropertyDTO dto, AppUser user)
        {
            property.user = !string.IsNullOrEmpty(dto.owner) ? dto.owner : user.Id;
            property.TypeContract = dto.TypeContract ?? property.TypeContract;
            property.Type = dto.Type ?? property.Type;
            property.Address = dto.Address ?? property.Address;
            property.Area = dto.Area ?? property.Area;
            property.date = DateTime.Now;
            property.Description = dto.Description ?? property.Description;
            property.MoreDescription = dto.MoreDescription ?? property.MoreDescription;
            property.Price = dto.Price ?? property.Price;
        }

        private async Task AddORUpdateMainPhotoAsync(Property property, IPropertyDTO dto)
        {
            if (dto.MainPhoto != null)
            {
                property.MainPhoto = await _imageService.CompressAndSaveImageAsync(dto.MainPhoto, "Photos");
            }
        }

        private void AddORUpdateStateForRent(Property property, IPropertyDTO dto)
        {
            if (!string.IsNullOrEmpty(dto.TypeContract) && dto.TypeContract.ToLower().Contains("rent"))
            {
                property.State = false;
            }
        }
    }
}
