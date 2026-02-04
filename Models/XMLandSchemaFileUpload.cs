using Microsoft.AspNetCore.Http;

namespace XMLValidator.Models
{
    public class XMLandSchemaFileUpload
    {
        public IFormFile? SchemaFile { get; set; }
        public IFormFile? XmlFile { get; set; }
    }
}
