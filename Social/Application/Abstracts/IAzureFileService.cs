using Application.FileService;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services;

public interface IAzureFileService
{
    Task<FileUploadResult> UploadFileAsync(string containerName, IFormFile file);
    void Delete(string containerName, string fileName);


}
