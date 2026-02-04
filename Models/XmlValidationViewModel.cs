using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace XMLValidator.Models
{
    public class XmlValidationViewModel
    {
        [Display(Name = "XSD Schema File")]
        public IFormFile SchemaFile { get; set; }

        [Display(Name = "XML File")]
        public IFormFile XmlFile { get; set; }

        public string XmlFileName { get; set; } = string.Empty;

        public string XsdFileName { get; set; } = string.Empty;

        public bool? IsValid { get; set; }

        public List<XmlValidationError> Errors { get; } = new();
    }
}