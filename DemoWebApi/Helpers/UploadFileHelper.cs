namespace DemoWebApi.Helpers
{
    public static class UploadFileHelper
    {
        public static async Task<string> UploadAsync(string fileLocation, IFormFile file, bool renameFile = true)
        {
            var fileExt = Path.GetExtension(file.FileName).Substring(1).ToLower();

            var fileName = renameFile ? $"{DateTime.Now.ToFileTime()}.{fileExt}" : file.FileName;

            var filePath = Path.Combine(fileLocation, fileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return fileName;
        }

        public static string CreateFolderIfNotExists(string path1, string path2)
        {
            var fileLocation = Path.Combine(path1, path2);

            if (!Directory.Exists(fileLocation))
                Directory.CreateDirectory(fileLocation);

            return fileLocation;
        }
    }
}
