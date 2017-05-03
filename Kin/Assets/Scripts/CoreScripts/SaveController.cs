﻿using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.UI;

public class SaveController : MonoBehaviour {
    //on enable on disable for autosave

    public static SaveController s_instance;
	public GameObject Player;
	public GameObject Health;
	public GameObject Stamina;
	public GameObject DNH;

	public GameObject healthtrainer;
	public GameObject staminatrainer;
	public GameObject strengthtrainer;
	public GameObject wisdomtrainer;

    // Use this for initialization
    void Awake()
    {
        if (s_instance == null)
        {
            DontDestroyOnLoad(gameObject); // save object on scene mvm
            s_instance = this;
        }
        else if (s_instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
		if (PreLoader.Instance != null) {
			if (PreLoader.Instance.resume) {
				Load ();
			}
		}
		if (DeathMenuController.Instance != null) {
			if (Player != null) {
				Load ();
			}
		}
    }

	public static SaveController GetInstance(){
		if (s_instance == null) {
			s_instance = new SaveController ();
		}
		return s_instance;
	}
	
    void OnGUI()
    {
        if(GUI.Button(new Rect(10, 100, 100, 30), "Save"))
        {
            Save();
        }
        if(GUI.Button(new Rect(10, 130, 100, 30), "Load"))
        {
            Load();
        }
    }

	public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/saveInfo.dat");
        SaveData data = WriteToData();
        bf.Serialize(file, data);
        file.Close();
    }

	public void Load()
    {
		if(File.Exists(Application.persistentDataPath + "/saveInfo" + ".dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/saveInfo.dat",
                FileMode.Open);
            SaveData data = (SaveData) bf.Deserialize(file);
            file.Close();
        }
    }

	public void LoadStats(){
		if(File.Exists(Application.persistentDataPath + "/saveInfo" + ".dat"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/saveInfo.dat",
				FileMode.Open);
			SaveData data = (SaveData) bf.Deserialize(file);
			file.Close();
			if (data != null) {
				WriteStatsFromData (data);
			}
		}
	}

	private void WriteFromData(SaveData data)
	{
		//Health.GetComponent<Slider>().value = data.health;
		Player.GetComponent<PlayerHealth> ().setCurrentHealth(data.healthval);
		//Stamina.GetComponent<Slider>().value = data.stamina;
		Player.GetComponent<PlayerStamina> ().setCurrentStamina(data.stamval);
		Player.transform.position = new Vector3 (data.x, data.y, Player.transform.position.z);
		Player.GetComponent<PlayerExperience> ().setCurrentExp (data.exp);
		Player.GetComponent<StatController>().setHealth(data.healthLvlP);
		Player.GetComponent<StatController>().setHealthOrder(data.healthLvlO);
		Player.GetComponent<StatController>().setStamina(data.stamLvlP);
		Player.GetComponent<StatController>().setStaminaOrder(data.stamLvlO);
		Player.GetComponent<StatController>().setStrength(data.strLvlP);
		Player.GetComponent<StatController>().setStrengthOrder(data.strLvlO);
		Player.GetComponent<StatController>().setWisdom(data.wisLvlP);
		Player.GetComponent<StatController>().setWisdomOrder(data.wisLvlO);
		//DNH.GetComponent<TimeController>().kin = data.day;
		TimeController.kin = data.day;
		TimeController.CalculateCalendar();
		DNH.GetComponent<DayNightController>().worldTimeHour = data.hour;
		DNH.GetComponent<DayNightController>().minutes = data.minute;

		if (healthtrainer && healthtrainer.GetComponent<DialogueBox> ())
			healthtrainer.GetComponent<DialogueBox> ().diaType = (data.healthtrainerd == 0) ? DialogueBox.DiaType.Init : DialogueBox.DiaType.Greetings;
		if(staminatrainer && staminatrainer.GetComponent<DialogueBox> ())
			staminatrainer.GetComponent<DialogueBox> ().diaType = (data.staminatrainerd == 0) ? DialogueBox.DiaType.Init : DialogueBox.DiaType.Greetings;
		if(strengthtrainer && strengthtrainer.GetComponent<DialogueBox> ())
			strengthtrainer.GetComponent<DialogueBox> ().diaType = (data.strengthtrainerd == 0) ? DialogueBox.DiaType.Init : DialogueBox.DiaType.Greetings;
		if(wisdomtrainer && wisdomtrainer.GetComponent<DialogueBox> ())
			wisdomtrainer.GetComponent<DialogueBox> ().diaType = (data.wisdomtrainerd == 0) ? DialogueBox.DiaType.Init : DialogueBox.DiaType.Greetings;
	}

	private void WriteStatsFromData(SaveData data)
	{

		Player.GetComponent<StatController>().setHealth(data.healthLvlP);
		Player.GetComponent<StatController>().setHealthOrder(data.healthLvlO);
		Player.GetComponent<StatController>().setStamina(data.stamLvlP);
		Player.GetComponent<StatController>().setStaminaOrder(data.stamLvlO);
		Player.GetComponent<StatController>().setStrength(data.strLvlP);
		Player.GetComponent<StatController>().setStrengthOrder(data.strLvlO);
		Player.GetComponent<StatController>().setWisdom(data.wisLvlP);
		Player.GetComponent<StatController>().setWisdomOrder(data.wisLvlO);
	}

    private SaveData WriteToData ()
    {
        SaveData data = new SaveData();
		data.health = Health.GetComponent<Slider>().value;
		data.healthval = Player.GetComponent<PlayerHealth> ().getCurrentHealth ();
		data.stamina = Stamina.GetComponent<Slider>().value;
		data.stamval = Player.GetComponent<PlayerStamina> ().getCurrentStamina ();

		if (Player.transform.position.x == 0 && Player.transform.position.y == 0) {
			data.x = 50;
			data.y = -63;
		} else {
			data.x = Player.transform.position.x;
			data.y = Player.transform.position.y;
		}

		data.exp = Player.GetComponent<PlayerExperience> ().getCurrentExp ();

		data.healthLvlP = Math.Max(Player.GetComponent<StatController>().getHealth(), 1);
		data.healthLvlO = Math.Max(Player.GetComponent<StatController>().getHealthOrder(), 1);
		data.stamLvlP = Math.Max(Player.GetComponent<StatController>().getStamina(), 1);
		data.stamLvlO = Math.Max(Player.GetComponent<StatController>().getStaminaOrder(), 1);
		data.strLvlP = Math.Max(Player.GetComponent<StatController>().getStrength(), 1);
		data.strLvlO = Math.Max(Player.GetComponent<StatController>().getStrengthOrder(), 1);
		data.wisLvlP = Math.Max(Player.GetComponent<StatController>().getWisdom(), 1);
		data.wisLvlO = Math.Max(Player.GetComponent<StatController>().getWisdomOrder(), 1);

		//data.day = DNH.GetComponent<TimeController>().kin;
		data.day = Math.Max(TimeController.kin, 1);
		data.hour = DNH.GetComponent<DayNightController>().worldTimeHour;
		data.minute = DNH.GetComponent<DayNightController>().minutes;

		data.healthtrainerd = (healthtrainer && 
			healthtrainer.GetComponent<DialogueBox> () && 
			healthtrainer.GetComponent<DialogueBox> ().diaType == DialogueBox.DiaType.Init) ? 0 : 1;
		data.staminatrainerd = (staminatrainer && 
			staminatrainer.GetComponent<DialogueBox> () && 
			staminatrainer.GetComponent<DialogueBox> ().diaType == DialogueBox.DiaType.Init) ? 0 : 1;;
		data.strengthtrainerd = (strengthtrainer && 
			strengthtrainer.GetComponent<DialogueBox> () && 
			strengthtrainer.GetComponent<DialogueBox> ().diaType == DialogueBox.DiaType.Init) ? 0 : 1;;
		data.wisdomtrainerd = (wisdomtrainer && 
			wisdomtrainer.GetComponent<DialogueBox> () && 
			wisdomtrainer.GetComponent<DialogueBox> ().diaType == DialogueBox.DiaType.Init) ? 0 : 1;;
		data.savage1d = 0;
		data.savage2d = 0;
		data.savage3d = 0;
		data.savage4d = 0;

        return data;
    }
}

[Serializable]
class SaveData 
{
    public float health;
    public float stamina;
	public int healthval;
	public int stamval;

	public float x;
	public float y;

	public long exp;

    public int healthLvlP;
    public int healthLvlO;
    public int stamLvlP;    //stamina
    public int stamLvlO;
    public int strLvlP;     //strength
    public int strLvlO;
    public int wisLvlP;     //wisdom
    public int wisLvlO;

    public int day;
    public int hour;
	public int minute;

	public int healthtrainerd;
	public int staminatrainerd;
	public int strengthtrainerd;
	public int wisdomtrainerd;
	public int savage1d;
	public int savage2d;
	public int savage3d;
	public int savage4d;
}
