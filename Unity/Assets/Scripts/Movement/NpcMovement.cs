using UnityEngine;

public class NpcMovement : MonoBehaviour
{
    [SerializeField] private bool isPlayer = false;
    [SerializeField] private PlayerObject playerObj;
    [SerializeField] private float movementRange = 5f;
    [SerializeField] private float movementSpeed = 2f;

    private Vector3 startPoint = Vector3.zero;
    private Vector3 endPoint = Vector3.zero;

    private float movementTimer = 0f;
    private bool isMoving = false;

    public void RandomizeNewLocation()
    {
        startPoint = transform.position;
        endPoint = new Vector3(Random.Range(-movementRange, movementRange), 0f, Random.Range(-movementRange, movementRange));
        movementTimer = 0f;
        isMoving = true;
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
                playerObj.SetLocation(transform.position);
                playerObj.My5G.SetRadius(10f);
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
            relocateTimer = Random.Range(1f, 10f);
        }
    }
}
