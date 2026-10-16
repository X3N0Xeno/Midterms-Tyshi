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
    public Review FeaturedReview { get; set; } = new();
    
    // Normalized SVG Coordinates on Cebu Map (X: 10-90%, Y: 10-90%)
    public int MapX { get; set; } = 50;
    public int MapY { get; set; } = 50;
}