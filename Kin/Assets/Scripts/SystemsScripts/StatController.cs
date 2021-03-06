using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class StatController : MonoBehaviour
{

	private int healthLvlP;
	private int healthLvlO = 1;
	private double health;
	private int stamLvlP;
	private int stamLvlO = 1;
	private double stamina;
	private int strLvlP;
	private int strLvlO = 1;
	private double strength;
	private int wisLvlP;
	private int wisLvlO = 1;
	private double wisdom;

    private PlayerExperience Exp;

	// Use this for initialization
	void Awake ()
	{
		SaveController.GetInstance ().LoadStats ();
        Exp = gameObject.GetComponent<PlayerExperience>();

        resetToBaseStats();
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
	public void levelUpHealth(bool OrderFalsePlayerTrue)
    {
        int levelOrder = getHealthOrder();
        int currentLevel = getHealthLvl();
        //GIVE DESIRED LEVEL

        bool agreeToLevel = true;
        float daysRequired = levelUpFunction(levelOrder, currentLevel, currentLevel + 1);
        Debug.Log("DAYS REQUIRED: " + daysRequired);
		if (agreeToLevel && daysRequired != -1)
        {
            //Subtract Days

            //Level Up Stat By Pursued
			if (OrderFalsePlayerTrue) {
				healthLvlP = currentLevel + 1;
                TimeController.ProgressDay((int)daysRequired);
                
			}else{
                if (subtractEXP(levelOrder))
                {
                    healthLvlO = levelOrder + 1;
                }
			}
			health = 300 + 10 * healthLvlP;
            int difference = (int)health - FindObjectOfType<PlayerHealth>().getMaxHealth();
            FindObjectOfType<PlayerHealth>().setMaxHealth((int)health);
            FindObjectOfType<PlayerHealth>().incrementCurrentHealth(difference);
            FindObjectOfType<TempGameController>().updateHealthStamSliders();
        }
    }

    //level up
	public void levelUpStamina(bool OrderFalsePlayerTrue)
    {
        int levelOrder = getStaminaOrder();
        int currentLevel = getStaminaLvl();
        //GIVE DESIRED LEVEL

        bool agreeToLevel = true;
        float daysRequired = levelUpFunction(levelOrder, currentLevel, currentLevel + 1);
		if (agreeToLevel && daysRequired != -1)
        {
            //Subtract Days
            //TimeController.ProgressDay((int)daysRequired);
            //Level Up Stat By Pursued
			if (OrderFalsePlayerTrue) {
				stamLvlP = currentLevel + 1;
                TimeController.ProgressDay((int)daysRequired);
            }
            else{
                if (subtractEXP(levelOrder))
                {
                    stamLvlO = levelOrder + 1;
                }
			}
			stamina = 1500 + 50 * stamLvlP;
            FindObjectOfType<PlayerStamina>().setMaxStamina((int)stamina);
            FindObjectOfType<TempGameController>().updateHealthStamSliders();
        }
    }

    //level up
	public void levelUpStrength(bool OrderFalsePlayerTrue)
    {
        int levelOrder = getStrengthOrder();
        int currentLevel = getStrength();
        //GIVE DESIRED LEVEL

        bool agreeToLevel = true;
        float daysRequired = levelUpFunction(levelOrder, currentLevel, currentLevel + 1);
		if (agreeToLevel && daysRequired != -1)
        {
            //Subtract Days
            //TimeController.ProgressDay((int)daysRequired);
            //Level Up Stat By Pursued
			if (OrderFalsePlayerTrue) {
				strLvlP = currentLevel + 1;
                TimeController.ProgressDay((int)daysRequired);
            }
            else{
                if (subtractEXP(levelOrder))
                {
                    strLvlO = levelOrder + 1;
                }
			}
			strength = strLvlP;
            FindObjectOfType<PlayerMelee>().setDamage((int)strength);
        }
    }

    //level up
	public void levelUpWisdom(bool OrderFalsePlayerTrue)
    {
        int levelOrder = getWisdomOrder();
        int currentLevel = getWisdom();
        //GIVE DESIRED LEVEL

        bool agreeToLevel = true;
        float daysRequired = levelUpFunction(levelOrder, currentLevel, currentLevel + 1);
		if (agreeToLevel && daysRequired != -1)
        {
            //Subtract Days
            //TimeController.ProgressDay((int)daysRequired);
            //Level Up Stat By Pursued
			if (OrderFalsePlayerTrue) {
				wisLvlP = currentLevel + 1;
                TimeController.ProgressDay((int)daysRequired);
            }
            else{
                if (subtractEXP(levelOrder))
                {
                    wisLvlO = levelOrder + 1;
                }
			}
			wisdom = 0.875 + .25 * wisLvlP;
        }
    }

    int availableLevels(int levelOrder, int currentLevel)
    {
        int maxLevel = 0;
        switch (levelOrder)
        {
            case 0:
                maxLevel = 0;
                //Cannot level up
                break;
            case 1:
                maxLevel = 35;
                break;
            case 2:
                maxLevel = 55;
                break;
            case 3:
                maxLevel = 75;
                break;
            case 4:
                maxLevel = 90;
                break;
            case 5:
                maxLevel = 100;
                break;
        }
        return maxLevel;
    }


    float levelUpFunction(int levelOrder, int currentLevel, int levelPursued)
    {
        //get levelPursused from available levels
        //float nFunc = 0f;
        //if (levelOrder > 1 || levelOrder <= 3)
        //{
        //    nFunc = 0.5f + 0.5f * levelOrder;
        //}
        //if (levelOrder > 3)
        //{
        //    nFunc = levelOrder - 1;
        //}
        //float daysRequired = (450 - 50 * nFunc) * (1 - Mathf.Pow(2, -levelPursued / 20f)) + Mathf.Pow(1.5f, levelPursued / (2f * nFunc));
        //This is in case you can't reach that level
        float daysRequired = 2000000;
        int maxLevel = availableLevels(levelOrder, currentLevel);
        if (levelPursued <= maxLevel)
        {
            daysRequired = 12000 * (Mathf.Exp(0.05f * (levelPursued - maxLevel)) - Mathf.Exp(0.05f * (currentLevel - maxLevel)));
        }
        if (daysRequired > 1872000)
        {
            daysRequired = -1;
            Debug.LogError("Attempted to level up too high: " + currentLevel + ", " + levelPursued);
        }
        return daysRequired;
    }

    //! orderLvl is the current level not the next one
    bool subtractEXP(int orderLvl)
    {
        switch (orderLvl)
        {
            case 1:
                return Exp.decrementExp(2000);
            case 2:
                return Exp.decrementExp(3000);
            case 3:
                return Exp.decrementExp(5000);
            case 4:
                return Exp.decrementExp(7000);
            case 5:
                Debug.Log("At max Order layer!");
                return false;
        }
        return false;
    }

    void resetToBaseStats()
    {
        health = 400;
        stamina = 1500;
        strength = 10;
        wisdom = 10;
        switch (healthLvlO)
        {
            case 1:
                healthLvlP = 10;
                break;
            case 2:
                healthLvlP = 15;
                break;
            case 3:
                healthLvlP = 20;
                break;
            case 4:
                healthLvlP = 35;
                break;
            case 5:
                healthLvlP = 55;
                break;
        }

        switch (stamLvlO)
        {
            case 1:
                stamLvlP = 8;
                break;
            case 2:
                stamLvlP = 13;
                break;
            case 3:
                stamLvlP = 18;
                break;
            case 4:
                stamLvlP = 33;
                break;
            case 5:
                stamLvlP = 53;
                break;
        }

        switch (strLvlO)
        {
            case 1:
                strLvlP = 10;
                break;
            case 2:
                strLvlP = 15;
                break;
            case 3:
                strLvlP = 20;
                break;
            case 4:
                strLvlP = 35;
                break;
            case 5:
                strLvlP = 55;
                break;
        }

        switch (wisLvlO)
        {
            case 1:
                wisLvlP = 5;
                break;
            case 2:
                wisLvlP = 10;
                break;
            case 3:
                wisLvlP = 15;
                break;
            case 4:
                wisLvlP = 30;
                break;
            case 5:
                wisLvlP = 50;
                break;
        }

        strength = strLvlP;
        stamina = 1500 + 50 * stamLvlP;
        health = 300 + 10 * healthLvlP;
        wisdom = 0.875 + .25 * wisLvlP;
    }

    void resetAllTheThings()
    {
        healthLvlO = 1;
        stamLvlO = 1;
        strLvlO = 1;
        wisLvlO = 1;
        resetToBaseStats();
    }

    //Set methods
    public void setHealthLvl(int playerLevel)
	{
		healthLvlP = playerLevel;
	}
		
	public void setHealthOrder(int playerHealthOrder)
	{
		healthLvlO = playerHealthOrder;
	}

	public void setHealth(int playerHealth){
		health = playerHealth;
	}

	public void setStaminaLvl(int staminaLevel)
	{
		stamLvlP = staminaLevel;
	}

	public void setStaminaOrder(int playerStaminaOrder)
	{
		stamLvlO = playerStaminaOrder;
	}

	public void setStamina(int playerStamina)
	{
		stamina = playerStamina;
	}

	public void setStrengthLvl(int strengthLvl)
	{
		strLvlP = strengthLvl;
	}

	public void setStrengthOrder(int playerStrengthOrder)
	{
		strLvlO = playerStrengthOrder;
	}

	public void setStrength(int playerStrength)
	{
		strength = playerStrength;
	}

	public void setWisdomLvl(int wisdomLevel)
	{
		wisLvlP = wisdomLevel;
	}

	public void setWisdomOrder(int playerWisdomOrder)
	{
		wisLvlO = playerWisdomOrder;
	}

	public void setWisdom(int playerWisdom)
	{
		wisdom = playerWisdom;
	}

    //Get methods
    public int getHealth()
    {
		return (int)health;
    }

    public int getHealthOrder()
    {
        return healthLvlO;
    }

	public int getHealthLvl()
	{
		return healthLvlP;
	}

    public int getStamina()
    {
		return (int)stamina;
    }

    public int getStaminaOrder()
    {
        return stamLvlO;
    }

	public int getStaminaLvl()
	{
		return stamLvlP;
	}

    public int getStrength()
    {
		return (int)strength;
    }

    public int getStrengthOrder()
    {
        return strLvlO;
    }

	public int getStrengthLvl()
	{
		return strLvlP;
	}

    public int getWisdom()
    {
		return (int)wisdom;
    }

    public int getWisdomOrder()
    {
        return wisLvlO;
    }

    public int getWisdomLvl()
    {
        return wisLvlP;
    }
}

