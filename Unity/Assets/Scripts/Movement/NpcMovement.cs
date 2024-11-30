using UnityEngine;

public class NpcMovement : MonoBehaviour
{
	[SerializeField] private bool isPlayer = false;
	[SerializeField] private PlayerObject playerObj;
	[SerializeField] private AnimationCurve movementRange = new AnimationCurve();
	[SerializeField] private AnimationCurve relocateTimeRange = new AnimationCurve();
	[SerializeField] private float movementSpeed = 2f;
	[SerializeField] private bool moveTo5GLocation = false;

	private Vector3 startPoint = Vector3.zero;
	private Vector3 endPoint = Vector3.zero;

	private float movementTimer = 0f;
	private bool isMoving = false;

	public void RandomizeNewLocation()
	{
		startPoint = transform.position;
		movementTimer = 0f;
		isMoving = true;

		if (moveTo5GLocation)
		{
			FivegPoint newFive = WorldManager.Instance.FiveGees[Random.Range(0, WorldManager.Instance.FiveGees.Count - 1)];
			endPoint = new Vector3(newFive.transform.position.x - startPoint.x, 0f, newFive.transform.position.z - startPoint.z);
			return;
		}
		else
		{ endPoint = new Vector3(movementRange.Evaluate(Random.Range(0, 1f)), 0f, movementRange.Evaluate(Random.Range(0, 1f))); }

		if (playerObj.My5G != null)
		{
			if (!isPlayer && !playerObj.My5G.IsWithingRadius(startPoint + endPoint, false))
			{
				Vector3 finalPoint = startPoint + endPoint;
				Vector3 validPoint = playerObj.My5G.transform.position + ((finalPoint - playerObj.My5G.transform.position).normalized * (playerObj.My5G.Radius * 0.95f));

				endPoint = validPoint - startPoint;
			}
		}

	}
	public void SetNewLocation(Vector3 location)
	{
		startPoint = transform.position;
		endPoint = new Vector3(location.x - startPoint.x, 0f, location.y - startPoint.y);
		movementTimer = 0f;
		isMoving = true;
	}

	private void Update()
	{
		MovementUpdate();
		RelocateUpdate();
	}

	private void MovementUpdate()
	{
		if (isMoving)
		{
			movementTimer += Time.deltaTime * movementSpeed;
			if (movementTimer >= 1f)
			{
				movementTimer = 1f;
				isMoving = false;
			}
			transform.position = startPoint + (endPoint * movementTimer);
			if (isPlayer)
			{
				playerObj.SetLocation(new Vector2(transform.position.x, transform.position.z));
				//playerObj.My5G.SetRadius(10f);
			}
		}
	}

	private float relocateTimer = 1f;

	private void RelocateUpdate()
	{
		relocateTimer -= Time.deltaTime;
		if (relocateTimer <= 0f)
		{
			RandomizeNewLocation();
			relocateTimer = relocateTimeRange.Evaluate(Random.Range(0f, 1f));
			if (isPlayer) { WorldManager.Instance.SpawnGemNear(gameObject); }
		}
	}
}
