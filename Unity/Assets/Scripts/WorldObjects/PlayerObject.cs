using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class PlayerObject : MonoBehaviour
{
	[SerializeField] private bool isNpc = false;
	[SerializeField] private FivegPoint my5G;
	public FivegPoint My5G => my5G;
	[SerializeField] private bool isSimulated = false;
	[SerializeField] private Q4ScoreUpdater scoreUpdater;
	[SerializeField] private Collider myCollider;
	private int currentScore;
	public int CurrentScore => currentScore;


	private void Start()
	{
		//SetLocation(WorldManager.Instance.CurrentGpsLocation);
		//WorldManager.Instance.OnGpsLocationUpdate.AddListener(SetLocation);
		currentScore = 0;
	}

	public void SetLocation(Vector2 ingameLocation)
	{
		if (isSimulated) { return; }
		transform.position = new Vector3(ingameLocation.x, 0f, ingameLocation.y);

		if (my5G == null) { return; }

		if (isNpc)
		{
			if (my5G.IsWithingRadius(gameObject, false))
			{
				myCollider.enabled = false;
				myCollider.enabled = true;
			}
		}
		else
		{
			if (my5G.IsWithingRadius(gameObject, true))
			{
				myCollider.enabled = false;
				myCollider.enabled = true;
			}
		}
	}
	public void SetSimulatedLocation(Vector2 ingameLocation)
	{
		transform.position = new Vector3(ingameLocation.x, 0f, ingameLocation.y);

		if (isNpc) { return; }
		my5G.SetCircleMaterial(my5G.YellowMat);
	}
	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.tag == "Crystal" && my5G.IsWithingRadius(gameObject, false))
		{
			CollectableGem gem = col.transform.root.GetComponent<CollectableGem>();
			HandleScore(CurrentScore, CurrentScore + gem.Points);
			currentScore += gem.Points;
			gem.OnCollect();
		}

		else if (col.gameObject.tag == "FiveGPoint")
		{
			FivegPoint five = col.transform.root.GetComponent<FivegPoint>();
			if (five == null) { return; }

			if (My5G != null)
			{
				if (Vector3.Distance(transform.position, My5G.transform.position) < Vector3.Distance(transform.position, five.transform.position))
				{ return; }
				if (!isNpc) { my5G.SetGrey(); }
			}

			my5G = five;
			if (isNpc)
			{ my5G.IsWithingRadius(gameObject, false); }
			else
			{ my5G.IsWithingRadius(gameObject, true); }
		}
	}

	private void HandleScore(int previousScore, int newScore)
	{
		if (scoreUpdater == null) { return; }
		if (scoreUpdater.Preset == null)
		{ scoreUpdater.UpdatePreset(); }
		Keyframe[] newKeys = scoreUpdater.Preset.Evaluatable.keys;
		newKeys[0].value = previousScore;
		newKeys[newKeys.Length - 1].value = newScore;
		scoreUpdater.CustomEvaluatable.keys = newKeys;
		scoreUpdater.UpdatePreset();

		scoreUpdater.Play();
	}
}