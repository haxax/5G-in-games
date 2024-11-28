using UnityEngine;
using System.Collections.Generic;

public class LocationPath : MonoBehaviour
{
    [SerializeField] private LineRenderer rend;
    [SerializeField] private Vector3[] pathPointsInMeters = new Vector3[0];

    public void SetupPath(List<Vector2> ingameLocations)
    {
        pathPointsInMeters = new Vector3[ingameLocations.Count];
        Vector2 converter = new Vector2();
        for (int i = 0; i < ingameLocations.Count; i++)
        {
            converter = ingameLocations[i];
            pathPointsInMeters[i] = new Vector3(converter.x, 0, converter.y);
        }
        rend.positionCount = pathPointsInMeters.Length;
        rend.SetPositions(pathPointsInMeters);
    }

    [ContextMenu("test")]
    public void Test()
    {
        List<Vector2> rndPoints = new List<Vector2>();
        for (int i = 0; i < 20; i++)
        {
            rndPoints.Add(new Vector2(Random.Range(-20, 20), Random.Range(-20, 20)));
        }
        SetupPath(rndPoints);
    }
}