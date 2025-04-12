using API.Interfaces;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.Extensions.Options;
using CloudinaryDotNet.Core;
using API.Settings;
using Microsoft.Extensions.Configuration;

namespace API.Sevices
{
    public class PhotoService : IPhotoService
    {

        private readonly Cloudinary _cloudinary;
        public PhotoService(IOptions<CloudinarySettings> config, IConfiguration configuration)
        {
            var cloudName = configuration["CloudinarySettings:CloudName"];
            var apiKey = configuration["CloudinarySettings:ApiKey"];
            var apiSecret = configuration["CloudinarySettings:ApiSecret"];

            var acc = new Account(cloudName, apiKey, apiSecret);

            //var acc = new Account(config.Value.CloudName, config.Value.ApiKey, config.Value.ApiSecret);
            //var acc = new Account("dekgj3sxv", "863498529523888", "pAwc_-ByWicvL3H_iyH7pPihZaI");
            _cloudinary = new Cloudinary(acc);
        }

        public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
        {
            var uploadResult = new ImageUploadResult();
            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face"),
                    Folder = "da-net8",
                };
                uploadResult = await _cloudinary.UploadAsync(uploadParams);

            }
            ;
            //return await _cloudinary.UploadAsync(uploadParams);
            return uploadResult;
        }

        public async Task<DeletionResult> DeletePhotoAsync(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);
            return await _cloudinary.DestroyAsync(deleteParams);
        }

    }
}