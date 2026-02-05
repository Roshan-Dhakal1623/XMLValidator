using System.ComponentModel.DataAnnotations;

namespace Lab3.Models
{
    public class RestaurantEditViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Restaurant name cannot be empty")]
        [Display(Name = "Restaurant Name")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Street address cannot be empty")]
        [Display(Name = "Street Address")]
        public string? StreetAddress { get; set; }
        [Required(ErrorMessage = "City cannot be empty")]
        [RegularExpression(
            @"^[A-Za-z\s]+$",
            ErrorMessage = "City must contain letters only"
        )]
        [Display(Name = "City")]
        public string? City { get; set; }


        [Display(Name = "Province")]
        public CanadianProvinceCodeType ProvinceState { get; set; }

        [Required(ErrorMessage = "Postal code cannot be empty")]
        [RegularExpression(
            @"^[A-Za-z]\d[A-Za-z][ -]?\d[A-Za-z]\d$",
            ErrorMessage = "Postal Code must be in the format A1A 1A1"
        )]
        [Display(Name = "Postal Code")]
        public string? PostalZipCode { get; set; }

        [Required(ErrorMessage = "Summary cannot be empty")]
        [Display(Name = "Summary")]
        public string? Summary { get; set; }

        [Required(ErrorMessage = "Rating is required")]
        [RegularExpression(
            @"^(?:[1-4]\.\d|5\.0)$",
            ErrorMessage = "Rating must be between 1.0 and 5.0 with one decimal place"
        )]
        [Range(1.0, 5.0, ErrorMessage = "Rating must be between 1.0 and 5.0")]
        [Display(Name = "Rating (1.0 to 5.0)")]
        public decimal Rating { get; set; }

    }
}
