using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace XMLValidator.Models
{
    public class XmlValidationViewModel
    {
        [Display(Name = "XSD Schema File")]
        public IFormFile SchemaFile { get; set; }

        [Display(Name = "XML File")]
        public IFormFile XmlFile { get; set; }
    }
}