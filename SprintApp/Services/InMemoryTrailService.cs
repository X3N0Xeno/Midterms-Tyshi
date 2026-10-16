namespace SprintApp.Services;

using SprintApp.Models;

public class InMemoryTrailService : ITrailService
{
    private readonly List<FoodSpot> _selectedStops = new();
    private readonly HashSet<Guid> _favorites = new();
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
            District = "Gorordo / Acacia",
            PriceTier = "₱₱₱",
            EstimatedPrice = 420m,
            Rating = 4.9,
            ReviewCount = 1240,
            SuggestedDuration = "1 hr 15 mins",
            Description = "Iconic Cebuano dining hall famous for its juicy roast pig with crackling skin and Carcar-style spicy sauce.",
            ImageUrl = "https://images.unsplash.com/photo-1544025162-d76694265947?w=600&auto=format&fit=crop&q=80",
            IsHighDemand = true,
            EstTransitMinutes = 20,
            MapX = 52,
            MapY = 62,
            FeaturedReview = new() {
                ReviewerName = "Marco Tan",
                AvatarInitials = "MT",
                RatingStars = 5,
                Comment = "The skin was impossibly crisp. Our best first meal in Cebu!"
            }
        },
        new() {
            Name = "Larsian BBQ / Fuente Street Food",
            Category = FoodCategory.ComfortFood,
            District = "Fuente Osmeña",
            PriceTier = "₱",
            EstimatedPrice = 220m,
            Rating = 4.7,
            ReviewCount = 890,
            SuggestedDuration = "45 mins",
            Description = "Bustling open-air barbecue strip where skewers of chicken inasal and sweet pork belly are enjoyed with hanging rice (puso).",
            ImageUrl = "https://images.unsplash.com/photo-1555939594-58d7cb561ad1?w=600&auto=format&fit=crop&q=80",
            IsHighDemand = true,
            EstTransitMinutes = 15,
            MapX = 45,
            MapY = 70,
            FeaturedReview = new() {
                ReviewerName = "Bea Alcantara",
                AvatarInitials = "BA",
                RatingStars = 5,
                Comment = "Lively, smoky, and so much fun. Order the chorizo and extra puso."
            }
        },
        new() {
            Name = "Abaca Baking Co.",
            Category = FoodCategory.LightMeals,
            District = "Cebu IT Park",
            PriceTier = "₱₱",
            EstimatedPrice = 280m,
            Rating = 4.8,
            ReviewCount = 560,
            SuggestedDuration = "50 mins",
            Description = "Artisanal bakery and breakfast retreat serving freshly baked croissants, gourmet sandwiches, and specialty espresso.",
            ImageUrl = "https://images.unsplash.com/photo-1509440159596-0249088772ff?w=600&auto=format&fit=crop&q=80",
            IsHighDemand = false,
            EstTransitMinutes = 25,
            MapX = 58,
            MapY = 48,
            FeaturedReview = new() {
                ReviewerName = "David Sy",
                AvatarInitials = "DS",
                RatingStars = 5,
                Comment = "A calm breakfast stop with excellent coffee and flaky pastries."
            }
        },
        new() {
            Name = "La Tegola / Busay View",
            Category = FoodCategory.FineDining,
            District = "Busay Hills",
            PriceTier = "₱₱₱",
            EstimatedPrice = 850m,
            Rating = 4.9,
            ReviewCount = 430,
            SuggestedDuration = "2 hrs",
            Description = "Romantic Italian terrace perched high in the Busay mountains overlooking the sparkling Cebu metropolitan skyline.",
            ImageUrl = "https://images.unsplash.com/photo-1517248135467-4c7edcad34c4?w=600&auto=format&fit=crop&q=80",
            IsHighDemand = false,
            EstTransitMinutes = 40,
            MapX = 70,
            MapY = 22,
            FeaturedReview = new() {
                ReviewerName = "Camille Cruz",
                AvatarInitials = "CC",
                RatingStars = 5,
                Comment = "Book before sunset—the view made our final dinner unforgettable."
            }
        },
        new() {
            Name = "Carcar Lechon Market",
            Category = FoodCategory.LocalDelicacies,
            District = "Carcar City",
            PriceTier = "₱₱",
            EstimatedPrice = 450m,
            Rating = 4.9,
            ReviewCount = 1520,
            SuggestedDuration = "1 hr",
            Description = "The southern heritage epicenter of Cebuano lechon, famous for rich pork jus infused directly back into the tender meat.",
            ImageUrl = "https://images.unsplash.com/photo-1598515214211-89d3c73ae83b?w=600&auto=format&fit=crop&q=80",
            IsHighDemand = true,
            EstTransitMinutes = 55,
            MapX = 25,
            MapY = 88,
            FeaturedReview = new() {
                ReviewerName = "Kenji Morales",
                AvatarInitials = "KM",
                RatingStars = 5,
                Comment = "Completely worth the drive. Rich, tender, and wonderfully local."
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

    public void TriggerToast(string message)
    {
        ActiveToastMessage = message;
        Notify();
    }

    public void DismissToast()
    {
        ActiveToastMessage = null;
        Notify();
    }

    private void Notify() => OnChange?.Invoke();
}