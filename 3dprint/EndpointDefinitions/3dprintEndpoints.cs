using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

public class ms3dPrint : IEndpoints
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapGet("getUsernames", GetFile);
        app.MapPost("uploadFile", UploadFile);
        
            
    }

    public void DefineServices(IServiceCollection services)
    {
        services.AddSingleton<FileService>();
        services.AddSingleton<PurchaceSystem>();
        services.AddSingleton<LoginSystem>();
        services.AddSingleton<ThingverseWrapper>();
    }

    private IResult GetFile(FileService fileService, string fileID)
    {
        return fileService.GetFile(fileID);
    }


    [Consumes("multipart/form-data")]
    private async Task<IActionResult> UploadFile([FromServices] FileService fileService, IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return new BadRequestResult();
        }

        using (var memoryStream = new MemoryStream())
        {
            await file.CopyToAsync(memoryStream);
            var fileBytes = memoryStream.ToArray();
            
            // Save or process the file using your FileService
            var fileId = fileService.SaveFile(fileBytes, file.FileName);

            // Return any relevant information
            return new OkObjectResult(new { FileId = fileId });
        }
    }

    
}

