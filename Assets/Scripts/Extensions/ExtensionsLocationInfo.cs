using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionsLocationInfo
{
    /// <summary>
    /// Converts Vector2(Longitude, Latitude) to Vector2(Meters X, Meters Y)
    /// </summary>
    public static Vector2 LocationToMeters(this Vector2 location)
    {
        return new Vector2(
            LongitudeToMeters(location.y, location.x),
            LatitudeToMeters(location.y, location.x)
            );
    }
    public static Vector2 LocationToMeters(this LocationInfo location)
    {
        return new Vector2(
            LongitudeToMeters(location.latitude, location.longitude),
            LatitudeToMeters(location.latitude, location.longitude)
            );
    }

    public static float LatitudeToMeters(float latitude, float longitude)
    {  // generally used geo measurement function
        float lat1 = 0;
        float lat2 = latitude;
        float lon1 = longitude;
        float lon2 = longitude;

        var R = 6371008.8f; // Radius of earth in meters
        var dLat = lat2 * Mathf.PI / 180 - lat1 * Mathf.PI / 180;
        var dLon = lon2 * Mathf.PI / 180 - lon1 * Mathf.PI / 180;
        var a = Mathf.Sin(dLat / 2) * Mathf.Sin(dLat / 2) +
        Mathf.Cos(lat1 * Mathf.PI / 180) * Mathf.Cos(lat2 * Mathf.PI / 180) *
        Mathf.Sin(dLon / 2) * Mathf.Sin(dLon / 2);
        var c = 2 * Mathf.Atan2(Mathf.Sqrt(a), Mathf.Sqrt(1 - a));
        var d = R * c;
        return d;
    }

    public static float LongitudeToMeters(float latitude, float longitude)
    {  // generally used geo measurement function
        float lat1 = latitude;
        float lat2 = latitude;
        float lon1 = 0;
        float lon2 = longitude;

        var R = 6371008.8f; // Radius of earth in meters
        var dLat = lat2 * Mathf.PI / 180 - lat1 * Mathf.PI / 180;
        var dLon = lon2 * Mathf.PI / 180 - lon1 * Mathf.PI / 180;
        var a = Mathf.Sin(dLat / 2) * Mathf.Sin(dLat / 2) +
        Mathf.Cos(lat1 * Mathf.PI / 180) * Mathf.Cos(lat2 * Mathf.PI / 180) *
        Mathf.Sin(dLon / 2) * Mathf.Sin(dLon / 2);
        var c = 2 * Mathf.Atan2(Mathf.Sqrt(a), Mathf.Sqrt(1 - a));
        var d = R * c;
        return d;
    }
}