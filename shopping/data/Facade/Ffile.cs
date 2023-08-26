namespace shopping.data.Facade
{
    public class Ffile
    {
        public async Task<string> UploadImage(IFormFile image,string PathUrl)
        {
            if (image == null || image.Length == 0)
                return null;

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/"+PathUrl, fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            return fileName;
        }

        public void DeleteImage(string? fileName,string PathUrl)
        {
            if (fileName!=null)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/"+PathUrl, fileName);
                  if (File.Exists(path))
                  {
                      File.Delete(path);
                  }

                  }
            
        }
    }
}
