namespace SprintApp.Models;

/// <summary>Single source of truth for category display text (previously duplicated in Home + SpotCard).</summary>
public static class FoodCategoryExtensions
{
    public static string ToLabel(this FoodCategory category) => category switch
    {
        FoodCategory.All => "All Spots",
        FoodCategory.LightMeals => "Street & Light",
        FoodCategory.ComfortFood => "Comfort & BBQ",
        FoodCategory.FineDining => "Fine Dining",
        FoodCategory.LocalDelicacies => "Local Delicacies",
        _ => category.ToString()
    };

    public static string ToEmoji(this FoodCategory category) => category switch
    {
        FoodCategory.All => "🍽️",
        FoodCategory.LightMeals => "🥐",
        FoodCategory.ComfortFood => "🍢",
        FoodCategory.FineDining => "🍷",
        FoodCategory.LocalDelicacies => "🐖",
        _ => "🍴"
    };
}
