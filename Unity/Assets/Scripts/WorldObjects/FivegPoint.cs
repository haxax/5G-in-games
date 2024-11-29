using UnityEngine;

public class FivegPoint : MonoBehaviour
{
    [SerializeField] private Transform circle;
    [SerializeField] private MeshRenderer circleRend;
    [SerializeField] private Material greenMat;
    public Material GreenMat => greenMat;
    [SerializeField] private Material yellowMat;
    public Material YellowMat => yellowMat;
    [SerializeField] private Material redMat;
    public Material RedMat => redMat;

    public float Radius { get; private set; } = 5f;

    private void Start()
    {
        SetLocation(WorldManager.Instance.Current5gLocation);
        SetRadius(WorldManager.Instance.Current5gRadius);
        WorldManager.Instance.On5gLocationUpdate.AddListener(SetLocation);
        WorldManager.Instance.On5gRadiusUpdate.AddListener(SetRadius);
    }

    public void SetLocation(Vector2 ingameLocation)
    {
        transform.position = new Vector3(ingameLocation.x, 0f, ingameLocation.y);
    }

    public void SetRadius(float meters)
    {
        Radius = meters;
        circle.localScale = new Vector3(Radius, 0.01f, Radius);
    }

    public bool IsWithingRadius(GameObject obj, bool changeColor = true)
    {
        if (Vector2.Distance(new Vector2(obj.transform.position.x, obj.transform.position.y), new Vector2(transform.position.x, transform.position.y)) > Radius/2f)
        {
            SetCircleMaterial(redMat);
            return false;
        }
        SetCircleMaterial(greenMat);
        return true;
    }

    public void SetCircleMaterial(Material mat)
    {
        circleRend.material = mat;
    }
}
