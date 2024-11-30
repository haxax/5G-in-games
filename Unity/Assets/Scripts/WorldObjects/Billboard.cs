using UnityEngine;

public class Billboard : MonoBehaviour
{
	Camera mainCamera;
	private void Start()
	{
		mainCamera = Camera.main;
	}
	void Update()
	{
		if (mainCamera != null)
		{ transform.LookAt(mainCamera.transform); }
	}
}