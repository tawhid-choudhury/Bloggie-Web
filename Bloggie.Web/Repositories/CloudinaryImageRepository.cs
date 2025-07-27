
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace Bloggie.Web.Repositories
{
    public class CloudinaryImageRepository : IImageRepository
    {
        private readonly IConfiguration config;
        private readonly Account account;

        public CloudinaryImageRepository(IConfiguration config)
        {
            this.config = config;
            account = new Account(
                config["Cloudinary:CloudName"],
                config["Cloudinary:ApiKey"],
                config["Cloudinary:ApiSecret"]
            );
        }
        public async Task<string> UploadAsync(IFormFile file)
        {
            Cloudinary cloudinary = new Cloudinary(account);
            cloudinary.Api.Secure = true;

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, file.OpenReadStream()),
                DisplayName = file.FileName,
            };
            var uploadResult = await cloudinary.UploadAsync(uploadParams);

            if (uploadResult != null && uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return uploadResult.SecureUri.ToString();
            }
            return null;

        }
    }
}
