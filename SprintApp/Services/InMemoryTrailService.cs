namespace SprintApp.Services;

using SprintApp.Models;

public class InMemoryTrailService : ITrailService
{
    private readonly List<FoodSpot> _selectedStops = new();
    private readonly HashSet<Guid> _favorites = new();
    private CancellationTokenSource? _toastCts;
    public event Action? OnChange;

    public IReadOnlyList<FoodSpot> SelectedStops => _selectedStops.AsReadOnly();
    public HashSet<Guid> FavoriteIds => _favorites;
    public int TravelerCount { get; private set; } = 2;
    public string? ActiveToastMessage { get; private set; }

    public decimal PerPersonExpense => _selectedStops.Sum(s => s.EstimatedPrice);
    public decimal GroupTotal => PerPersonExpense * TravelerCount;
    public int TotalStops => _selectedStops.Count;
    public int TotalEstimatedMinutes => _selectedStops.Sum(s => s.EstTransitMinutes);

    public IReadOnlyList<FoodSpot> Spots { get; } = new List<FoodSpot>
    {
        new() {
            Name = "House of Lechon",
            Category = FoodCategory.LocalDelicacies,
            District = "Acacia St, Cebu City",
            PriceTier = "₱₱₱",
            EstimatedPrice = 420m,
            Rating = 4.9,
            ReviewCount = 1240,
            SuggestedDuration = "1 hr 15 mins",
            StarQuality = "Golden Crisp Skin & Carcar Spiced Gravy",
            MapsQueryAddress = "House of Lechon Acacia St Cebu City",
            Description = "Iconic dining hall serving spit-roasted Cebuano lechon basted with native lemongrass and garlic.",
            ImageUrl = "https://images.unsplash.com/photo-1544025162-d76694265947?w=700&auto=format&fit=crop&q=80",
            IsHighDemand = true,
            EstTransitMinutes = 20,
            FeaturedReview = new() {
                ReviewerName = "Marco Tan",
                AvatarInitials = "MT",
                RatingStars = 5,
                Comment = "The skin was impossibly crisp. Our best first meal in Cebu!"
            },
            MenuHighlights = new() {
                new() { Name = "Spicy Carcar Lechon (1/2 Kilo)", Price = 520, IsBestSeller = true },
                new() { Name = "Baked Scallops with Butter Garlic", Price = 295, IsBestSeller = true },
                new() { Name = "Monggos Soup with Chicharon", Price = 180, IsBestSeller = false }
            }
        },
        new() {
            Name = "Larsian BBQ / Fuente Street Food",
            Category = FoodCategory.ComfortFood,
            District = "Fuente Osmeña, Cebu City",
            PriceTier = "₱",
            EstimatedPrice = 220m,
            Rating = 4.7,
            ReviewCount = 890,
            SuggestedDuration = "45 mins",
            StarQuality = "Signature Sweet Pork Skewers & Hanging Rice",
            MapsQueryAddress = "Fuente Osmeña Circle Cebu City",
            Description = "Open-air barbecue stalls where charcoal grills create authentic smoky Cebu street culture.",
            ImageUrl = "https://images.unsplash.com/photo-1555939594-58d7cb561ad1?w=700&auto=format&fit=crop&q=80",
            IsHighDemand = true,
            EstTransitMinutes = 15,
            FeaturedReview = new() {
                ReviewerName = "Bea Alcantara",
                AvatarInitials = "BA",
                RatingStars = 5,
                Comment = "Lively, smoky, and so much fun. Order the chorizo and extra puso."
            },
            MenuHighlights = new() {
                new() { Name = "Cebu Sweet Chorizo Skewer", Price = 45, IsBestSeller = true },
                new() { Name = "Pork BBQ Skewers (5 pcs)", Price = 110, IsBestSeller = true },
                new() { Name = "Puso (Hanging Rice x 4)", Price = 40, IsBestSeller = false }
            }
        },
        new() {
            Name = "Abaca Baking Co.",
            Category = FoodCategory.LightMeals,
            District = "Cebu IT Park, Lahug",
            PriceTier = "₱₱",
            EstimatedPrice = 280m,
            Rating = 4.8,
            ReviewCount = 560,
            SuggestedDuration = "50 mins",
            StarQuality = "Slow-Fermented Artisan Sourdough",
            MapsQueryAddress = "Abaca Baking Co Crossroads Mall Cebu City",
            Description = "Artisanal bakery and breakfast retreat renowned for flaky viennoiseries and specialty coffee.",
            ImageUrl = "https://images.unsplash.com/photo-1509440159596-0249088772ff?w=700&auto=format&fit=crop&q=80",
            IsHighDemand = false,
            EstTransitMinutes = 25,
            FeaturedReview = new() {
                ReviewerName = "David Sy",
                AvatarInitials = "DS",
                RatingStars = 5,
                Comment = "A calm breakfast stop with excellent coffee and flaky pastries."
            },
            MenuHighlights = new() {
                new() { Name = "Classic Pain au Chocolat", Price = 160, IsBestSeller = true },
                new() { Name = "Truffled Scrambled Eggs on Brioche", Price = 380, IsBestSeller = true },
                new() { Name = "Flat White Single Origin", Price = 165, IsBestSeller = false }
            }
        },
        new() {
            Name = "La Tegola / Busay View",
            Category = FoodCategory.FineDining,
            District = "Busay Hills, Cebu City",
            PriceTier = "₱₱₱",
            EstimatedPrice = 850m,
            Rating = 4.9,
            ReviewCount = 430,
            SuggestedDuration = "2 hrs",
            StarQuality = "Panaromic Skyline Horizon & Wood-Fired Pasta",
            MapsQueryAddress = "La Tegola Cucina Italiana Busay Cebu",
            Description = "Romantic Italian terrace perched in the cooler hills of Busay overlooking the metropolitan lights.",
            ImageUrl = "https://images.unsplash.com/photo-1517248135467-4c7edcad34c4?w=700&auto=format&fit=crop&q=80",
            IsHighDemand = false,
            EstTransitMinutes = 40,
            FeaturedReview = new() {
                ReviewerName = "Camille Cruz",
                AvatarInitials = "CC",
                RatingStars = 5,
                Comment = "Book before sunset—the view made our final dinner unforgettable."
            },
            MenuHighlights = new() {
                new() { Name = "Spaghetti alle Vongole", Price = 490, IsBestSeller = true },
                new() { Name = "Filetto al Pepe Verde", Price = 980, IsBestSeller = true },
                new() { Name = "Signature Tiramisu Tradizionale", Price = 280, IsBestSeller = false }
            }
        },
        new() {
            Name = "Carcar Lechon Market",
            Category = FoodCategory.LocalDelicacies,
            District = "Carcar Public Market",
            PriceTier = "₱₱",
            EstimatedPrice = 450m,
            Rating = 4.9,
            ReviewCount = 1520,
            SuggestedDuration = "1 hr",
            StarQuality = "Direct Jus Injected Skin & Ampao Treats",
            MapsQueryAddress = "Carcar City Public Market Lechon Section",
            Description = "The undisputed lechon capital where warm roast pig is chopped fresh directly on wooden blocks.",
            ImageUrl = "https://images.unsplash.com/photo-1598515214211-89d3c73ae83b?w=700&auto=format&fit=crop&q=80",
            IsHighDemand = true,
            EstTransitMinutes = 55,
            FeaturedReview = new() {
                ReviewerName = "Kenji Morales",
                AvatarInitials = "KM",
                RatingStars = 5,
                Comment = "Completely worth the drive. Rich, tender, and wonderfully local."
            },
            MenuHighlights = new() {
                new() { Name = "Carcar Pork Lechon (1 Kilo)", Price = 650, IsBestSeller = true },
                new() { Name = "Special Chicharon with Meat Bulaklak", Price = 220, IsBestSeller = true },
                new() { Name = "Original Carcar Ampao Pack", Price = 80, IsBestSeller = false }
            }
        }
    };

    public void AddStop(FoodSpot spot)
    {
        if (_selectedStops.All(s => s.Id != spot.Id))
        {
            _selectedStops.Add(spot);
            Notify();
        }
    }

    public void RemoveStop(Guid spotId)
    {
        _selectedStops.RemoveAll(s => s.Id == spotId);
        Notify();
    }

    public void MoveEarlier(int index)
    {
        if (index > 0 && index < _selectedStops.Count)
        {
            var item = _selectedStops[index];
            _selectedStops.RemoveAt(index);
            _selectedStops.Insert(index - 1, item);
            Notify();
        }
    }

    public void MoveLater(int index)
    {
        if (index >= 0 && index < _selectedStops.Count - 1)
        {
            var item = _selectedStops[index];
            _selectedStops.RemoveAt(index);
            _selectedStops.Insert(index + 1, item);
            Notify();
        }
    }

    public void SetTravelers(int count)
    {
        TravelerCount = Math.Clamp(count, 1, 20);
        Notify();
    }

    public void ToggleFavorite(Guid spotId)
    {
        if (!_favorites.Add(spotId))
            _favorites.Remove(spotId);
        Notify();
    }

    public bool IsSelected(Guid spotId) => _selectedStops.Any(s => s.Id == spotId);
    public bool IsFavorite(Guid spotId) => _favorites.Contains(spotId);

    public void TriggerToast(string message, int durationMs = 3500)
    {
        _toastCts?.Cancel();
        _toastCts = new CancellationTokenSource();
        var token = _toastCts.Token;

        ActiveToastMessage = message;
        Notify();

        _ = Task.Run(async () =>
        {
            try
            {
                await Task.Delay(durationMs, token);
                if (!token.IsCancellationRequested)
                {
                    ActiveToastMessage = null;
                    Notify();
                }
            }
            catch (TaskCanceledException) { }
        }, token);
    }

    public void DismissToast()
    {
        _toastCts?.Cancel();
        ActiveToastMessage = null;
        Notify();
    }

    private void Notify() => OnChange?.Invoke();
}