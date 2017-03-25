﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof(EnemyHealth), (typeof(Animator)), (typeof(Rigidbody2D)))]
public class IxtabAI : MonoBehaviour, BaseAI {

	public GameObject player;
	public Slider UIHealth;
	public Text UIName;
	public float detectRange;
	public float speed;
	private EnemyHealth enemyHealth;
	private Animator anim;
	private Rigidbody2D rb2;
	private bool calm;

	public float attackRange = 1.0f;

	public float maxAttackCooldown = 1.0f;
	public float maxSpawnCooldown = 10.0f;
	public float maxInvisCooldown = 15.0f;
	public float maxSlamCooldown = 5.0f;

	private float slamCooldown;
	private float swipeCooldown;
	private float invisCooldown;

	private float spawnCooldown;

	private bool isInvis = false;

	private bool facing = true;

	public IxtabController ixtabController;

	public float damageDone = 10.0f;

	private float alphaLerp = 0.0f;
	private const float ALPHA_LERP_TIME = 1.5f;

	private Color baseColor;
	private Color invisColor;

	// Use this for initialization
	void Start () {
		enemyHealth = this.gameObject.GetComponent<EnemyHealth> ();
		anim = this.gameObject.GetComponent<Animator> ();
		rb2 = this.gameObject.GetComponent<Rigidbody2D> ();
		calm = true;

		slamCooldown = maxSlamCooldown;
		swipeCooldown = maxAttackCooldown;
		invisCooldown = maxInvisCooldown;
		spawnCooldown = maxSpawnCooldown;

		baseColor = gameObject.GetComponent<SpriteRenderer>().color;
		invisColor = baseColor;
		invisColor.a = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
		ixtabController.GetComponent<Animator>().SetFloat("DistanceToPlayer", distanceToPlayer());

		checkDeath();

		if (!calm) {
			slamCooldown = Mathf.Max(-1.0f, slamCooldown - Time.deltaTime);
			swipeCooldown = Mathf.Max(-1.0f, swipeCooldown - Time.deltaTime);
			spawnCooldown = Mathf.Max(-1.0f, spawnCooldown - Time.deltaTime);
			invisCooldown = Mathf.Max(-1.0f, invisCooldown - Time.deltaTime);

			if (player.transform.position.y > gameObject.transform.position.y &&
				Mathf.Abs(player.transform.position.x - gameObject.transform.position.x) < 0.5 &&
				distanceToPlayer() < attackRange) {
				slamCooldown = -1.0f;
			}
		}

		if (swipeCooldown < 0) {
			ixtabController.GetComponent<Animator>().SetBool("AttackCD", true);
		}

		if (slamCooldown < 0) {
			ixtabController.GetComponent<Animator>().SetBool("SlamCD", true);
		}

		if (invisCooldown < 0) {
			ixtabController.GetComponent<Animator>().SetBool("InvisCD", true);
		}

		if (!ixtabController.GetComponent<Animator>().GetBool("Dying")) {
			facing = player.transform.position.x > gameObject.transform.position.x;
			gameObject.GetComponent<SpriteRenderer>().flipX = !facing;
		} else {
			rb2.velocity = Vector2.zero;
			foreach(Collider2D coll in GetComponents<Collider2D>()) {
				coll.enabled = false;
			}
			float timer = alphaLerp / ALPHA_LERP_TIME;
			gameObject.GetComponent<SpriteRenderer>().color = Color.Lerp(baseColor, invisColor, timer);
			alphaLerp += Time.deltaTime;

			if (alphaLerp > ALPHA_LERP_TIME) {
				Destroy(this.gameObject);
			}
		}
	}

	public void activateAttCD() {
		swipeCooldown = maxAttackCooldown;
	}

	public void activateSlamCD() {
		slamCooldown = maxSlamCooldown;
	}

	public void activateInvisCD() {
		invisCooldown = maxInvisCooldown;
	}

	public void activateSpawnCD() {
		spawnCooldown = maxSpawnCooldown;
	}

	public void setCalm(bool clm) {
		this.calm = clm;
	}

	public void followPlayer() {
		rb2.velocity = ((Vector2)player.transform.position - (Vector2)this.gameObject.transform.position).normalized * speed;
	}

	public void stopBoss() {
		rb2.velocity = Vector2.zero;
	}

	public void spawnMinions() {
		// Spawn %hp/10% Kamikaze
		//9 - ((((int)(enemyHealth.getHp()/enemyHealth.maxHealth)*100)) % 10)
		foreach(Vector2 vec in calculateAngles((int)(10 - (enemyHealth.getHp()/enemyHealth.maxHealth)/0.1), 1.0f)) {
			Instantiate(Resources.Load("Prefabs/Entities/Kamikaze", typeof(GameObject)) as GameObject, 
				vec, Quaternion.identity);
		}
	}

	public void swipe() {
		if (distanceToPlayer() < attackRange * 3/4) {
			if (Mathf.Abs(player.transform.position.y - gameObject.transform.position.y) < attackRange / 2) {
				player.GetComponent<PlayerHealth>().TakeDamage((int)damageDone);
			}
		}
	}

	public void slam() {
		if (distanceToPlayer() < attackRange) {
			player.GetComponent<PlayerHealth>().TakeDamage((int)damageDone);
		}
		foreach (Vector2 vec in calculateAngles(8, 0.5f)) {
			Instantiate(Resources.Load("Prefabs/Projectiles/LightningBolt", typeof(GameObject)) as GameObject,
				vec, Quaternion.identity);
		}
	}

	public void setInvisible(bool invis) {
		foreach(Collider2D coll in GetComponents<Collider2D>()) {
			coll.enabled = !invis;
		}
		isInvis = invis;
		enemyHealth.setInvinc(invis);
	}

	private float distanceToPlayer() {
		return Vector2.Distance (player.transform.position, this.gameObject.transform.position);
	}

	private List<Vector2> calculateAngles(int num, float distanceAway) {
		if (num == 0) {
			//return new List<Vector2>();
			num = 3;
		}
		int angleDisplacement = 360/num;
		int curAngle = 0;
		List<Vector2> vecList = new List<Vector2>();
		for (int i = 0; i < num; i++) {
			vecList.Add((Vector2)gameObject.transform.position + 
				new Vector2(Mathf.Cos(Mathf.Deg2Rad * curAngle), Mathf.Sin(Mathf.Deg2Rad * curAngle)).normalized * distanceAway);
			curAngle += angleDisplacement;
		}
		return vecList;
	}

	private void checkDeath() {
		if (enemyHealth.getHp() <= 0) {
			ixtabController.GetComponent<Animator>().SetBool("Dying", true);
		}
	}


	void OnDrawGizmos() {
		// Detect Range
		Gizmos.color = new Color(0.0f, 0.0f, 1.0f, 0.15f);
		Gizmos.DrawSphere(gameObject.transform.position, detectRange);
		Gizmos.color = new Color(0.0f, 0.0f, 1.0f, 0.10f);
		Gizmos.DrawSphere(gameObject.transform.position, detectRange + 0.1f);


		// Swipe Range
		Vector3 attackUpBound = new Vector3(gameObject.transform.position.x + (facing ? attackRange * 3/4 : -attackRange * 3/4), 
			gameObject.transform.position.y + (attackRange / 2));
		Vector3 attackLowBound = new Vector3(gameObject.transform.position.x + (facing ? attackRange * 3/4 : -attackRange * 3/4),
			gameObject.transform.position.y - (attackRange / 2));

		Gizmos.color = Color.red;
		Gizmos.DrawLine(gameObject.transform.position + new Vector3(0.0f, attackRange / 2), attackUpBound);
		Gizmos.DrawLine(gameObject.transform.position - new Vector3(0.0f, attackRange / 2), attackLowBound);
		Gizmos.DrawLine(attackUpBound, attackLowBound);
	}

	void BaseAI.recoil(){
		ixtabController.GetComponent<Animator>().SetBool("Recoiling", true);
		anim.SetBool ("Recoiling", true);
	}
}