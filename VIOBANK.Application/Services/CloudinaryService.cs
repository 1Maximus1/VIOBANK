using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VIOBANK.Application.Services
{
    public class CloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(IConfiguration config)
        {
            var account = new Account(
                config["CloudinarySettings:CloudName"],
                config["CloudinarySettings:ApiKey"],
                config["CloudinarySettings:ApiSecret"]);

            _cloudinary = new Cloudinary(account);
        }

        public async Task<string> UploadImageAsync(Stream fileStream, string fileName, string userId)
        {
            if (fileStream == null || fileStream.Length == 0)
            {
                throw new ArgumentException("File stream is empty or null.");
            }

            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException("User ID cannot be null or empty.");
            }

            await _cloudinary.DeleteResourcesAsync($"avatars/{userId}");

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(fileName, fileStream),
                PublicId = $"avatars/{userId}",
                Invalidate=true,
                Overwrite = true,
                Transformation = new Transformation().Width(300).Height(300).Crop("fill").Gravity("face")
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception("Failed to upload image to Cloudinary.");
            }

            return uploadResult.SecureUrl.ToString();
        }

        public string GetUserAvatarUrl(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException("User ID cannot be null or empty.");
            }

            var getResourceParams = new GetResourceParams($"avatars/{userId}")
            {
                QualityAnalysis = true
            };
            var getResourceResult = _cloudinary.GetResource(getResourceParams);

            return getResourceResult.Url;
        }
    }
}
