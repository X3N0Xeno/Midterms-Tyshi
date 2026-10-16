using System.Text;
using SprintApp.Models;

namespace SprintApp.Services;

/// <summary>Builds every Google Maps URL the UI needs, so components stay markup-only.</summary>
public static class MapsUrlBuilder
{
    private static string Enc(string value) => Uri.EscapeDataString(value);

    /// <summary>Opens a single place in Google Maps (new tab).</summary>
    public static string SearchUrl(FoodSpot spot) =>
        $"https://www.google.com/maps/search/?api=1&query={Enc(spot.MapsQueryAddress)}";

    /// <summary>Embeddable iframe URL. One stop = pin, 2+ stops = chained driving route.</summary>
    public static string EmbedUrl(IReadOnlyList<FoodSpot> stops)
    {
        if (stops.Count == 0) return string.Empty;

        if (stops.Count == 1)
            return $"https://maps.google.com/maps?q={Enc(stops[0].MapsQueryAddress)}&t=&z=14&ie=UTF8&iwloc=&output=embed";

        // saddr = stop 1, daddr = stop 2 + to:stop 3 + to:stop 4 ...
        var url = new StringBuilder("https://maps.google.com/maps?saddr=")
            .Append(Enc(stops[0].MapsQueryAddress))
            .Append("&daddr=")
            .Append(Enc(stops[1].MapsQueryAddress));

        for (var i = 2; i < stops.Count; i++)
            url.Append("+to:").Append(Enc(stops[i].MapsQueryAddress));

        return url.Append("&output=embed").ToString();
    }

    /// <summary>Full-page "open in Google Maps" directions link with waypoints.</summary>
    public static string DirectionsUrl(IReadOnlyList<FoodSpot> stops)
    {
        if (stops.Count == 0) return string.Empty;
        if (stops.Count == 1) return SearchUrl(stops[0]);

        var url = new StringBuilder("https://www.google.com/maps/dir/?api=1&travelmode=driving")
            .Append("&origin=").Append(Enc(stops[0].MapsQueryAddress))
            .Append("&destination=").Append(Enc(stops[^1].MapsQueryAddress));

        if (stops.Count > 2)
        {
            var waypoints = string.Join("|", stops.Skip(1).Take(stops.Count - 2).Select(s => s.MapsQueryAddress));
            url.Append("&waypoints=").Append(Enc(waypoints));
        }

        return url.ToString();
    }
}
