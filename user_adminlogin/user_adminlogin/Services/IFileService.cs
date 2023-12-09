namespace user_adminlogin.Services
{
    public interface IFileService
    {
       Tuple<int, string> SaveImage(IFormFile imageFile);
        public bool DeleteImage(string imageFileName);
    }
}
