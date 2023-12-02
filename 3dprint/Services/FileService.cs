using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;


using System.Threading;

public class FileService{

    string uploadFolderPath = "C:\\Users\\ander\\Desktop\\school\\school3DPrint\\3dprint\\Files";
    public IResult GetFile(String filePath){

        return Results.Ok();
    }

   private readonly string fileStoragePath = "C:\\Users\\ander\\Desktop\\school\\school3DPrint\\3dprint\\Files"; // Specify your file storage path

    public string SaveFile(byte[] fileBytes, string fileName)
    {
        // Generate a unique file name or use the original file name as per your requirement
        var uniqueFileName = Guid.NewGuid().ToString() + "_" + fileName;
        var filePath = Path.Combine(fileStoragePath, uniqueFileName);

        // Save the file to the specified path
        File.WriteAllBytes(filePath, fileBytes);

        // Optionally, you might want to store additional information or perform other tasks here

        return uniqueFileName; // Return the unique file name or any other relevant information
    }

}


