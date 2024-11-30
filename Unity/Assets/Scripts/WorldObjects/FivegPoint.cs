using UnityEngine;
using TMPro;

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
	[SerializeField] private Material greyMat;
	public Material GreyMat => greyMat;
	[SerializeField] private TMP_Text titleTxt;
	public TMP_Text TitleTxt => titleTxt;
	[SerializeField] private TMP_Text statusTxt;
	public TMP_Text StatusTxt => statusTxt;

	public float Radius { get; private set; } = 5f;

	private void Start()
	{
		WorldManager.Instance.FiveGees.Add(this);
		// SetLocation(WorldManager.Instance.Current5gLocation);
		// SetRadius(WorldManager.Instance.Current5gRadius);
		// WorldManager.Instance.On5gLocationUpdate.AddListener(SetLocation);
		// WorldManager.Instance.On5gRadiusUpdate.AddListener(SetRadius);
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
		return IsWithingRadius(obj.transform.position, changeColor);
	}
	public bool IsWithingRadius(Vector3 point, bool changeColor = true)
	{
		if (Vector2.Distance(new Vector2(point.x, point.z), new Vector2(transform.position.x, transform.position.z)) > Radius)
		{
			if (changeColor) { SetRed(); }
			return false;
		}
		if (changeColor) { SetGreen(); }
		return true;
	}


	public void SetCircleMaterial(Material mat)
	{
		circleRend.material = mat;
	}

	public void SetGrey()
	{
		SetCircleMaterial(GreyMat);
		TitleTxt.gameObject.SetActive(false);
		StatusTxt.gameObject.SetActive(false);
	}
	public void SetGreen()
	{
		SetCircleMaterial(GreenMat);
		TitleTxt.gameObject.SetActive(true);
		StatusTxt.gameObject.SetActive(true);
		StatusTxt.text = "Within valid range";
		statusTxt.color = Color.green;
	}
	public void SetRed()
	{
		SetCircleMaterial(RedMat);
		TitleTxt.gameObject.SetActive(true);
		StatusTxt.gameObject.SetActive(true);
		StatusTxt.text = "Outside valid range!";
		statusTxt.color = Color.red;
	}
}
