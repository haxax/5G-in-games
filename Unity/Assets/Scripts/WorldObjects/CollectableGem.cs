using UnityEngine;

public class CollectableGem : MonoBehaviour
{

    [SerializeField] private Q4Player collectAnim;
    [SerializeField] private int points = 100;
    public int Points => points;

    public void OnCollect()
    {
        collectAnim.Play();
    }
    public void OnDespawn()
    {
        Destroy(gameObject);
    }
}
