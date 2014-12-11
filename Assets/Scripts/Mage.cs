using UnityEngine;
using System.Collections;

public class Mage : MonoBehaviour
{
	// Mage stats
    public float Health;
	public float Shield;
    public float Damage;
    public int[] cooldown;
	public int player;
	
	// Some static value
	private float maxHealth = 10f;
	private float maxDamage = 5f;
	
	// Mage states that prevent cast
    public bool isBroken;
    public bool isWinded;
    public bool isInterrupted;
    public bool isFreezed;
	public bool isDead;
	
	// Animation
	public bool isAnimated;

    // Use this for initialization
    void Start()
    {
		Health = maxHealth;
		Damage = 1f;
        cooldown = new int[8] { 0, 0, 0, 0, 0, 0, 0, 0 }; // A->B->X->Y  Attack->Buff
		isBroken=isDead=isFreezed=isInterrupted=false;
        
		StartCoroutine("DecreaseCooldown");
    }

    // Update is called once per frame
    void Update()
    {
		if (Health <= 0)
			Death();
    }

    // Global cooldown for every skill when mage get interrupted/disabled or after spell casted
    public void SetGlobalCooldown(string type)
    {
        if (type == "Attack")
        {
            for (int i = 0; i < 4; i++)
                if (cooldown[i] <= 1)
                    cooldown[i] = 2;
        }
        else if (type == "Buff")
		{
            for (int j = 4; j < 8; j++)
                if (cooldown[j] <= 1)
                    cooldown[j] = 2;
		} else {
			for (int j = 0; j < 8; j++)
                if (cooldown[j] <= 1)
                    cooldown[j] = 2;
		}
    }

    // Set skill cooldown
    public void SetSkillCooldown(string type, string button)
    {
        switch (button)
        {
            case "A":
                if (type == "Attack")
                    cooldown[0] = 3;
                else
                    cooldown[4] = 3;
                break;
            case "B":
                if (type == "Attack")
                    cooldown[1] = 3;
                else
                    cooldown[5] = 3;
                break;
            case "X":
                if (type == "Attack")
                    cooldown[2] = 3;
                else
                    cooldown[6] = 3;
                break;
            case "Y":
                if (type == "Attack")
                    cooldown[3] = 3;
                else
                    cooldown[7] = 3;
                break;
        }
    }

	// Check is skill on cooldown
    public bool isOnCooldown(string type, string button)
    {
        switch (button)
        {
            case "A":
                if (type == "Attack")
                    if (cooldown[0] != 0)
                        return true;
                    else
                        if (cooldown[4] != 0)
                            return true;
                break;
            case "B":
                if (type == "Attack")
                    if (cooldown[1] != 0)
                        return true;
                    else
                        if (cooldown[5] != 0)
                            return true;
                break;
            case "X":
                if (type == "Attack")
                    if (cooldown[2] != 0)
                        return true;
                    else
                        if (cooldown[6] != 0)
                            return true;
                break;
            case "Y":
                if (type == "Attack")
                    if (cooldown[3] != 0)
                        return true;
                    else
                        if (cooldown[7] != 0)
                            return true;
                break;
        }
        return false;
    }

	// Play animation when mage get hurted
    public void Hurt(float dmg)
    {
		if (Shield <= 0)
			Health -= dmg;
		else {
			Shield -= dmg;
			if (Shield < 0)
				Health -= Shield;
		}
        gameObject.GetComponent<Animator>().SetTrigger("Hurt");
    }

	public void Heal(float Health_copy)
	{
		Health += Health_copy;
        //gameObject.GetComponent<Animator>().SetTrigger("Heal");
		isAnimated = true;
	}
	
	// Status of mage
	public void Death()
	{
        gameObject.GetComponent<Animator>().SetTrigger("Dead");
		StartCoroutine("Disable", "Death");
	}
	public void setInterrupted()
	{
		StartCoroutine ("Disable", "Interrupted");
	}
	public void setBroken()
	{
		StartCoroutine ("Disable", "Broken");
	}
	public void setWinded()
	{
		StartCoroutine ("Disable", "Winded");
	}
	public void setFreezed()
	{
		StartCoroutine ("Disable", "Freezed");
	}
    
	// Decrease cooldown of all skill while it's CD not zero
    IEnumerator DecreaseCooldown()
    {
        for (; ; )
        {
            yield return new WaitForSeconds(1.0f);
            for (int i = 0; i < 8; i++)
                if (cooldown[i] != 0)
                    cooldown[i]--;
        }
    }

	IEnumerator Disable(string Type)
	{
		float timeToWait;
		switch (Type)
		{
		case "Death":
			timeToWait = 8f;
			isDead = true;
			break;
		case "Interrupted":
			timeToWait = 2f;
			isInterrupted = true;
			break;
		case "Broken":
			timeToWait = 3f;
			isBroken = true;
			break;
		case "Winded":
			timeToWait = 6f;
			isWinded = true;
			break;
		case "Freezed":
			timeToWait = 3f;
			isFreezed = true;
			break;
		default:
			break;
		}

		yield return new WaitForSeconds(3.0f);

		switch (Type)
		{
		case "Death":
			isDead = false;
			Health = maxHealth/2;
			break;
		case "Interrupted":
			isInterrupted = false;
			break;
		case "Broken":
			isBroken = false;
			break;
		case "Winded":
			isWinded = false;
			break;
		case "Freezed":
			isFreezed = false;
			break;
		default:
			break;
		}
	}
}