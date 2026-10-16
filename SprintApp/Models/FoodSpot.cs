namespace SprintApp.Models;

public enum FoodCategory
{
    All,
    LightMeals,
    ComfortFood,
    FineDining,
    LocalDelicacies
}

public class Review
{
    public string ReviewerName { get; set; } = string.Empty;
    public string AvatarInitials { get; set; } = string.Empty;
    public int RatingStars { get; set; } = 5;
    public string Comment { get; set; } = string.Empty;
}

public class MenuItem
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public bool IsBestSeller { get; set; }
}

public class FoodSpot
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public FoodCategory Category { get; set; }
    public string District { get; set; } = string.Empty;
    public string PriceTier { get; set; } = "₱₱";
    public decimal EstimatedPrice { get; set; }
    public double Rating { get; set; } = 4.8;
    public int ReviewCount { get; set; }
    public string SuggestedDuration { get; set; } = "45 mins";
    public string Description { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public bool IsHighDemand { get; set; }
    public int EstTransitMinutes { get; set; } = 15;
    public string StarQuality { get; set; } = "Heritage Recipe";
    public string MapsQueryAddress { get; set; } = string.Empty;
    public Review FeaturedReview { get; set; } = new();
    public List<MenuItem> MenuHighlights { get; set; } = new();
}