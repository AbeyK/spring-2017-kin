﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IxtabMusicTrigger : MonoBehaviour {

	public GameObject DNH;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	public void OnTriggerEnter2D(Collider2D coll){
		if (coll.tag == "Player" && DNH.GetComponent<MusicController>().state != MusicController.MusicState.Boss) {
			DNH.GetComponent<MusicController> ().InterruptForBoss ("Ixtab");
		}
	}

	public void OnTriggerExit2D(Collider2D coll){
		if (coll.tag == "Player") {
			DNH.GetComponent<MusicController> ().InterruptForWorld ();
		}
	}
}
