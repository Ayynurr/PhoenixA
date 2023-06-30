using Microsoft.AspNetCore.Http;

namespace Persistance.Extentions;

public static class VideoFileExtension
{
    public static bool IsVideoFile(this IFormFile file)
    {
        var allowedExtensions = new[] { ".mp4", ".avi", ".mkv" };
        var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
        return Array.IndexOf(allowedExtensions, fileExtension) >= 0;
    }
    //public static bool IsVideoFile(this string file)
    //{
    //    string extension = Path.GetExtension(file);
    //    List<string> videoExtensions = new List<string> { ".mp4", ".avi", ".mov", ".mkv", ".wmv" };

    //    return videoExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase);
    //}

    public static async Task<string> VideoUploadAsync(this IFormFile file, params string[] folders)
    {
        string newFileName = Guid.NewGuid().ToString() + file.FileName;
        string pathFolder = Path.Combine(folders);
        string path = Path.Combine(pathFolder, newFileName);
        using (FileStream stream = new(path, FileMode.CreateNew))
        {
            await file.CopyToAsync(stream);
        }
        return newFileName;
    }
}
