using Microsoft.AspNetCore.Mvc;
using System.Xml;
using System.Xml.Schema;
using XMLValidator.Models;

namespace XMLValidator.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View(new XmlValidationViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upload(XMLandSchemaFileUpload upload)
        {
            var vm = new XmlValidationViewModel();

            // Basic upload validation
            if (upload.SchemaFile == null || upload.SchemaFile.Length == 0 ||
                upload.XmlFile == null || upload.XmlFile.Length == 0)
            {
                vm.Errors.Add(new XmlValidationError
                {
                    Element = "(upload)",
                    ErrorType = "Error",
                    Line = 0,
                    Column = 0,
                    Message = "Both XML file and Schema (XSD) file must be provided."
                });

                vm.IsValid = false;

                return View("ValidationResult", vm);
            }

            vm.XmlFileName = upload.XmlFile.FileName;
            vm.XsdFileName = upload.SchemaFile.FileName;

            string currentElement = "(unknown)";

            try
            {
                // Setup validation settings
                var settings = new XmlReaderSettings
                {
                    ValidationType = ValidationType.Schema,
                    DtdProcessing = DtdProcessing.Prohibit
                };

                settings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;

                // Load the schema from the uploaded XSD file
                using (var schemaStream = upload.SchemaFile.OpenReadStream())
                using (var schemaReader = XmlReader.Create(schemaStream))
                {
                    // Add schema to settings (namespace must match your XSD targetNamespace)
                    settings.Schemas.Add("http://www.algonquincollege.com/cst8259/labs", schemaReader);
                }

                // Capture validation errors
                settings.ValidationEventHandler += (sender, e) =>
                {
                    var ex = e.Exception; // XmlSchemaException provides line/column

                    vm.Errors.Add(new XmlValidationError
                    {
                        Element = currentElement,
                        ErrorType = e.Severity.ToString(), // "Error" or "Warning"
                        Line = ex?.LineNumber ?? 0,
                        Column = ex?.LinePosition ?? 0,
                        Message = e.Message
                    });
                };

                // Read XML to trigger validation
                using (var xmlStream = upload.XmlFile.OpenReadStream())
                using (var reader = XmlReader.Create(xmlStream, settings))
                {
                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element)
                        {
                            currentElement = reader.LocalName; // track current element for error reporting
                        }
                    }
                }
            }
            catch (XmlException ex)
            {
                // Well-formedness errors (missing tags, etc.)
                vm.Errors.Add(new XmlValidationError
                {
                    Element = "(document)",
                    ErrorType = "Structural Error",
                    Line = ex.LineNumber,
                    Column = ex.LinePosition,
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                vm.Errors.Add(new XmlValidationError
                {
                    Element = "(document)",
                    ErrorType = "Error",
                    Line = 0,
                    Column = 0,
                    Message = ex.Message
                });
            }

            vm.IsValid = vm.Errors.Count == 0;
            return View("ValidationResult", vm);
        }
    }
}
