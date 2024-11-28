using UnityEngine;

public class PlayerObject : MonoBehaviour
{
    [SerializeField] private FivegPoint my5G;
    [SerializeField] private bool isSimulated = false;

    private void Start()
    {
        SetLocation(WorldManager.Instance.CurrentGpsLocation);
        WorldManager.Instance.OnGpsLocationUpdate.AddListener(SetLocation);
    }
    public void SetLocation(Vector2 ingameLocation)
    {
        if (isSimulated) { return; }
        transform.position = new Vector3(ingameLocation.x, 0f, ingameLocation.y);
        my5G.IsWithingRadius(gameObject);
    }
    public void SetSimulatedLocation(Vector2 ingameLocation)
    {
        transform.position = new Vector3(ingameLocation.x, 0f, ingameLocation.y);
        my5G.SetCircleMaterial(my5G.YellowMat);
    }
}