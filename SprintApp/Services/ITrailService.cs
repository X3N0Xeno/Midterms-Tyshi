namespace SprintApp.Services;

using SprintApp.Models;

public interface ITrailService
{
    IReadOnlyList<FoodSpot> Spots { get; }
    IReadOnlyList<FoodSpot> SelectedStops { get; }
    HashSet<Guid> FavoriteIds { get; }
    int TravelerCount { get; }
    decimal PerPersonExpense { get; }
    decimal GroupTotal { get; }
    int TotalStops { get; }
    int TotalEstimatedMinutes { get; }
    string? ActiveToastMessage { get; }
    
    event Action? OnChange;

    void AddStop(FoodSpot spot);
    void RemoveStop(Guid spotId);
    void MoveEarlier(int index);
    void MoveLater(int index);
    void SetTravelers(int count);
    void ToggleFavorite(Guid spotId);
    bool IsSelected(Guid spotId);
    bool IsFavorite(Guid spotId);
    void TriggerToast(string message, int durationMs = 3500);
    void DismissToast();
}