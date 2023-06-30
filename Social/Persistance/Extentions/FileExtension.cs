
using Microsoft.AspNetCore.Http;

namespace Persistance.Extentions;

public static class FileExtension
{
    public static bool CheckFileType(this IFormFile file, string type)
    {
        return file.ContentType.Contains(type);
    }
    public static bool CheckFileSize(this IFormFile file, int kb)
    {
        return file.Length / 1024 > kb;
    }
    public static async Task<string> FileUploadAsync(this IFormFile file, params string[] folders)
    {
        string newFileName = Guid.NewGuid().ToString() + file.FileName;
        string pathFolder = Path.Combine(folders);
        //string pathFolder = Path.Combine(folders[0], folders[1]);
        string path = Path.Combine(pathFolder, newFileName);
        using (FileStream stream = new(path, FileMode.CreateNew))
        {
            await file.CopyToAsync(stream);
        }
        return newFileName;
    }
}
