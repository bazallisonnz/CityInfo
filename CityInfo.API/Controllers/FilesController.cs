using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace CityInfo.API.Controllers;

[Route("api/files")]
[ApiController]
public class FilesController : ControllerBase
{
    private readonly FileExtensionContentTypeProvider _fileExtensionContentTypeProvider;

    public FilesController(FileExtensionContentTypeProvider fileExtensionContentTypeProvider)
    {
        _fileExtensionContentTypeProvider = fileExtensionContentTypeProvider
                                            ?? throw new ArgumentNullException(nameof(fileExtensionContentTypeProvider));
    }

    [HttpGet("{fileId}")]
    public ActionResult GetFile(int fileId)
    {
        // look up the actual file, depending on the fileId...
        // demo code
        var pathToFile = "NA-140VG4_NA-148VG4_Operating_Instructions.pdf";

        // Check if the file exists.
        if (!System.IO.File.Exists(pathToFile))
        {
            return NotFound();
        }

        if (!_fileExtensionContentTypeProvider.TryGetContentType(pathToFile, out var contentType))
        {
            contentType = "application/octet-stream";
        }

        var bytes = System.IO.File.ReadAllBytes(pathToFile);
        return File(bytes, contentType, Path.GetFileName(pathToFile));
    }

    [HttpPost]
    public async Task<ActionResult> CreateFile(IFormFile file)
    {
        // IMPORTANT: The file should be stored in a secure location (without Execute privileges), not in the web root.
        // For this course, we are storing the file in the web root for simplicity.

        // Validate the input. Put a limit on file size to avoid large uploads attacks. 
        // Only accept .pdf files (check content-type)
        if (file.Length == 0 || file.Length > 20971520 || file.ContentType != "application/pdf")
        {
            return BadRequest("No file or an invalid one has been inputted.");
        }

        // Create the file path.  Avoid using file.FileName, as an attacker can provide a
        // malicious one, including full paths or relative paths.  
        var path = Path.Combine(Directory.GetCurrentDirectory(), $"uploaded_file_{Guid.NewGuid()}.pdf");

        await using var stream = new FileStream(path, FileMode.Create);
        await file.CopyToAsync(stream);

        return Ok("Your file has been uploaded successfully.");
    }
}
