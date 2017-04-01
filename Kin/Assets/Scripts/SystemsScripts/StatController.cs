using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class StatController : MonoBehaviour
{

	private int healthLvlP;
	private int healthLvlO;
	private int stamLvlP;
	private int stamLvlO;
	private int strLvlP;
	private int strLvlO;
	private int wisLvlP;
	private int wisLvlO;

	// Use this for initialization
	void Awake ()
	{
		healthLvlO = 0;
		healthLvlP = 447;
		stamLvlO = 0;
		stamLvlP = 2000;
		strLvlO = 0;
		strLvlP = 0;
		wisLvlO = 0;
		wisLvlP = 57;
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
    void levelUpHealth()
    {
        int levelOrder = getHealthOrder();
        int currentLevel = getHealth();
        //GIVE DESIRED LEVEL

        bool agreeToLevel = true;
        float daysRequired = levelUpFunction(levelOrder, currentLevel, currentLevel + 1);
        if (agreeToLevel)
        {
            //Subtract Days

            //Level Up Stat By Pursued
            setHealth(currentLevel + 1);
        }
    }

    //level up
    void levelUpStamina()
    {
        int levelOrder = getStaminaOrder();
        int currentLevel = getStamina();
        //GIVE DESIRED LEVEL

        bool agreeToLevel = true;
        float daysRequired = levelUpFunction(levelOrder, currentLevel, currentLevel + 1);
        if (agreeToLevel)
        {
            //Subtract Days
            //TimeController.ProgressDay((int)daysRequired);
            //Level Up Stat By Pursued
            setStamina(currentLevel + 1);
        }
    }

    //level up
    void levelUpStrength()
    {
        int levelOrder = getStrengthOrder();
        int currentLevel = getStrength();
        //GIVE DESIRED LEVEL

        bool agreeToLevel = true;
        float daysRequired = levelUpFunction(levelOrder, currentLevel, currentLevel + 1);
        if (agreeToLevel)
        {
            //Subtract Days
            //TimeController.ProgressDay((int)daysRequired);
            //Level Up Stat By Pursued
            setStrength(currentLevel + 1);
        }
    }

    //level up
    void levelUpWisdom()
    {
        int levelOrder = getWisdomOrder();
        int currentLevel = getWisdom();
        //GIVE DESIRED LEVEL

        bool agreeToLevel = true;
        float daysRequired = levelUpFunction(levelOrder, currentLevel, currentLevel + 1);
        if (agreeToLevel)
        {
            //Subtract Days
            //TimeController.ProgressDay((int)daysRequired);
            //Level Up Stat By Pursued
            setWisdom(currentLevel + 1);
        }
    }

    IEnumerable<int> availableLevels(int levelOrder, int currentLevel)
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
        IEnumerable<int> range = Enumerable.Range(currentLevel + 1, maxLevel);
        return range;
    }


    float levelUpFunction(int levelOrder, int currentLevel, float levelPursued)
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
        float daysRequired = Mathf.Infinity;
        IEnumerable<int> levelRange = availableLevels(levelOrder, currentLevel);
        int maxLevel = levelRange.Max();
        if (levelRange.Contains<int>((int)levelPursued))
        {
            daysRequired = 53620 * (Mathf.Exp(0.05f * (levelPursued-maxLevel)) - Mathf.Exp(0.05f * (currentLevel-maxLevel)));
        }
        return daysRequired;
    }

    //Set methods
    public void setHealth(int playerHealth)
	{
		healthLvlP = playerHealth;
	}

	public void setHealthOrder(int playerHealthOrder)
	{
		healthLvlO = playerHealthOrder;
	}

	public void setStamina(int playerStamina)
	{
		stamLvlP = playerStamina;
	}

	public void setStaminaOrder(int playerStaminaOrder)
	{
		stamLvlO = playerStaminaOrder;
	}

	public void setStrength(int playerStrength)
	{
		strLvlP = playerStrength;
	}

	public void setStrengthOrder(int playerStrengthOrder)
	{
		strLvlO = playerStrengthOrder;
	}

	public void setWisdom(int playerWisdom)
	{
		wisLvlP = playerWisdom;
	}

	public void setWisdomOrder(int playerWisdomOrder)
	{
		wisLvlO = playerWisdomOrder;
	}

    //Get methods
    public int getHealth()
    {
        return healthLvlP;
    }

    public int getHealthOrder()
    {
        return healthLvlO;
    }

    public int getStamina()
    {
        return stamLvlP;
    }

    public int getStaminaOrder()
    {
        return stamLvlO;
    }

    public int getStrength()
    {
        return strLvlP;
    }

    public int getStrengthOrder()
    {
        return strLvlO;
    }

    public int getWisdom()
    {
        return wisLvlP;
    }

    public int getWisdomOrder()
    {
        return wisLvlO;
    }
}

