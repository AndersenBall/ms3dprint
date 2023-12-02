public class ms3dPrint : IEndpoints
{
    public void DefineEndpoints(WebApplication app)
    {
        ;
        app.MapGet("getUsernames",GetFile);
        
    }

    public void DefineServices(IServiceCollection services)
    {
        services.AddSingleton<FileService>((ServiceProvider) => {
            return new FileService();
        });
        
    }

    private IResult GetFile(FileService fileService,String fileID){
        return fileService.GetFile(fileID);
    }

    

}

