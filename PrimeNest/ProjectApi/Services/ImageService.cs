using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProjectApi.ChainOfResponsibility;
using ProjectApi.DTOS;
using ProjectApi.DTOS.InterfaceDTO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Core.Servises
{
    // ImageService is now separated from any generic Service class

    public class ImageService
    {
        private readonly IUnitOfWork<Property> _propertyUnitOfWork;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hosting;

        public ImageService(IUnitOfWork<Property> propertyUnitOfWork, Microsoft.AspNetCore.Hosting.IHostingEnvironment hosting)
        {
            _propertyUnitOfWork = propertyUnitOfWork;
            _hosting = hosting;
        }

        public async Task<string> CompressAndSaveImageAsync(IFormFile file, string directory, int width = 800, int quality = 50)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is null or empty.", nameof(file));

            string uploads = Path.Combine(_hosting.WebRootPath, directory);
            Directory.CreateDirectory(uploads); // Ensure directory exists
            string filePath = Path.Combine(uploads, file.FileName);

            using (var image = await Image.LoadAsync(file.OpenReadStream()))
            {
                image.Mutate(x => x.Resize(new ResizeOptions
                {
                    Mode = ResizeMode.Max,
                    Size = new Size(width, 0)
                }));

                await using var outputStream = new FileStream(filePath, FileMode.Create);
                await image.SaveAsync(outputStream, new JpegEncoder { Quality = quality });
            }

            return file.FileName;
        }
    }
}
