﻿using UnityEngine;
using System.Collections;

public class BaseGodAI : MonoBehaviour {

	public float awarenessRadius; //Range to change idle->detected
	public GameObject targetObject; //Player target
	public float speed = 1.0f; //Movement speed
	protected Rigidbody2D rb; //God Rigidbody
	public string minionType;
	protected bool spawnOnCd;
	protected float spawnCurrCd;
	public float spawnCd;


	protected virtual void Start()
	{
		targetObject = GameObject.Find("Player");
		//Establish rigid body for minion
		rb = gameObject.GetComponent<Rigidbody2D>();
		if (rb == null)
		{
			Debug.LogError("AI has no RigidBody. AI name is " + gameObject.name + "!");
		}
		if (targetObject == null)
		{
			Debug.LogError("AI has no target. AI name is " + gameObject.name + "!");
		}

		spawnOnCd = true;

		//prefab = AssetDatabase.LoadAssetAtPath("Assets/prefabs.MinionProj", typeof(GameObject));
	}

	protected virtual void Update()
	{

	}

	/// <summary>
	/// Spawn this instance.
	/// </summary>
	protected virtual void Spawn()
	{
		if (!spawnOnCd)
		{
			SpawnMinion(gameObject.transform.position);
			spawnCurrCd = spawnCd;
			spawnOnCd = true;
		}
		else
		{ //Decrease remaining cooldown
			spawnCurrCd -= Time.deltaTime;
			if (spawnCurrCd <= 0.0f)
				spawnOnCd = false;
		}
	}

	/// <summary>
	/// Moves linearly towards target.
	/// </summary>
	protected void MoveTowardsTarget()
	{
		rb.velocity = ((Vector2)(targetObject.transform.position - gameObject.transform.position)).normalized * speed;
	}

	/// <summary>
	/// Moves linearly away from target.
	/// </summary>
	protected void MoveAwayFromTarget()
	{
		//Debug.Log ("in moveaway");
		rb.velocity = ((Vector2)(gameObject.transform.position - targetObject.transform.position)).normalized * speed;
	}

	/// <summary>
	/// Moves linearly towards position
	/// </summary>
	/// <param name="pos">Position.</param>
	protected void MoveTowardsPosition(Vector2 pos)
	{
		rb.velocity = (pos - (Vector2)gameObject.transform.position).normalized;
	}

	/// <summary>
	/// Spawns a minion.
	/// </summary>
	/// <param name="pos">Position.</param>
	protected void SpawnMinion(Vector2 pos)
	{
		Debug.Log("Spawn");
		GameObject newMinion = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/" + minionType,typeof(GameObject)),
			(Vector3)Random.insideUnitCircle + targetObject.transform.position,Quaternion.identity);
		newMinion.GetComponent<BaseMinionAI>().targetObject = targetObject;
	}

    protected void Experience(int amount)
    {
        targetObject.GetComponent<PlayerExperience>().incrementExp(amount);
        GameObject part = (GameObject)(Resources.Load("Prefabs/XPParticles", typeof(GameObject)));
        GameObject instPart = Instantiate(part, transform.position, Quaternion.identity);
        instPart.GetComponent<ParticleEmit>().UpdateParticles();
        instPart.GetComponent<ParticleEmit>().target = targetObject;
        instPart.GetComponent<ParticleEmit>().XPEmit(30);
    }
}