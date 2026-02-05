using Microsoft.AspNetCore.Mvc;
using System.Xml.Serialization;
using Lab3.Models;
using XMLValidator.Models;

public class HomeController : Controller
{
    // Path to XML file
    private string XmlPath =>
        Path.GetFullPath("Data/restaurant_reviews.xml");

    // =========================
    // INDEX (Overview Page)
    // =========================
    public IActionResult Index()
    {
        XmlSerializer serializer =
            new XmlSerializer(typeof(restaurantReviews));

        restaurantReviews data;

        using (FileStream fs = new FileStream(XmlPath, FileMode.Open))
        {
            data = (restaurantReviews)serializer.Deserialize(fs)!;
        }

        List<RestaurantOverviewViewModel> model = new();
        int id = 0;

        foreach (var r in data.restaurant)
        {
            model.Add(new RestaurantOverviewViewModel
            {
                Id = id,
                Name = r.name,
                FoodType = r.cuisine,
                Rating = r.review.rating.Value,
                Cost = r.price.tier,
                City = r.location.address.city,
                ProvinceState = r.location.address.province.ToString()
            });

            id++;
        }

        return View(model);
    }

    // =========================
    // EDIT (GET)
    // =========================
    public IActionResult Edit(int? id)
    {
        if (id == null)
            return NotFound();

        XmlSerializer serializer =
            new XmlSerializer(typeof(restaurantReviews));

        restaurantReviews data;

        using (FileStream fs = new FileStream(XmlPath, FileMode.Open))
        {
            data = (restaurantReviews)serializer.Deserialize(fs)!;
        }

        // Safety check
        if (id < 0 || id >= data.restaurant.Length)
            return NotFound();

        var r = data.restaurant[id.Value];

        var model = new RestaurantEditViewModel
        {
            Id = id.Value,
            Name = r.name,
            StreetAddress = r.location.address.street,
            City = r.location.address.city,
            ProvinceState = r.location.address.province,
            PostalZipCode = r.location.address.postalCode,
            Summary = r.review.summary,
            Rating = r.review.rating.Value
        };

        return View(model);
    }

    // =========================
    // EDIT (POST)
    // =========================
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(RestaurantEditViewModel model)
    {
        // ❌ Block save if ANY field is invalid or empty
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        XmlSerializer serializer =
            new XmlSerializer(typeof(restaurantReviews));

        restaurantReviews data;

        using (FileStream fs = new FileStream(XmlPath, FileMode.Open))
        {
            data = (restaurantReviews)serializer.Deserialize(fs)!;
        }

        // Safety check
        if (model.Id < 0 || model.Id >= data.restaurant.Length)
            return NotFound();

        var r = data.restaurant[model.Id];

        // ✅ Update XML only after validation passes
        r.name = model.Name!;
        r.location.address.street = model.StreetAddress!;
        r.location.address.city = model.City!;
        r.location.address.province = model.ProvinceState;
        r.location.address.postalCode = model.PostalZipCode!;
        r.review.summary = model.Summary!;
        r.review.rating.Value = (byte)Math.Floor(model.Rating);

        using (FileStream fs = new FileStream(XmlPath, FileMode.Create))
        {
            serializer.Serialize(fs, data);
        }

        return RedirectToAction(nameof(Index));
    }
}
