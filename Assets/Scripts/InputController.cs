using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputController : MonoBehaviour
{
	public bool[] isPressed; //Is button pressed A B X Y
	public float power; //Store power of projectile
	public Vector2 direction; //Direction of projectile
	public GameObject[] Targets; //Array of Targets to buff or debuff
	public int target; // Currently chosen target to buff or debuff
	public int player;
	public bool isTargetReadyToChange;

	//-----Debug input variables. Very usefull!------
	//public bool X, Y, A, B;
	//public float AxisX, AxisY, Trigger;
	
	void Start()
	{
		Targets = new GameObject[4] {
				GameObject.Find("MageRed"),
				GameObject.Find("MinionsRed"),
				GameObject.Find("MinionsBlue"),
				GameObject.Find("BlueMage")
			};
		direction = new Vector2();
		isPressed = new bool[4];
		if (player == 1)
			direction.x = 2;
		if (player == 2)
			direction.x = -2;
	}
	
	void Update()
	{
		if (player != this.GetComponent<Mage>().player) Debug.Log ("WRONG PLAYER SCRIPT!!");
		// Get pressed triggers.
		if (!(this.GetComponent<Mage>().isDead || this.GetComponent<Mage>().isInterrupted || this.GetComponent<Mage>().isFreezed || this.GetComponent<Mage>().isBroken || this.GetComponent<Mage>().isWinded))
		{
			if (Input.GetAxisRaw("Trigger"+player) >= 0.1f )
			{
				ChangeDirection();
				GetKeyState();
				StartAttack();
			} else if (Input.GetAxisRaw("Trigger"+player) <= -0.1f) {
				ChangeTarget();
				GetKeyState();
				StartBuff();
			} else if (isPressed[0] || isPressed[1] || isPressed[2] || isPressed[3])
			{
				// If trigger is not pressed, but element button is, damage yourself as punishment
				this.GetComponent<Mage>().heal(-1f);
				this.GetComponent<Mage>().setInterrupted();
				UnpressAll();
			}
		} else {
			if (this.GetComponent<Mage>().isDead) Debug.Log("Dead");
			if (this.GetComponent<Mage>().isInterrupted) Debug.Log("Interrupted");
			if (this.GetComponent<Mage>().isFreezed) Debug.Log("Freezed");
			if (this.GetComponent<Mage>().isBroken) Debug.Log("Broken");
			if (this.GetComponent<Mage>().isWinded) Debug.Log("Winded");
		}

		/*------ Debug input variables. Very usefull! ------
		A = Input.GetButton("A"+player);
		B = Input.GetButton("B"+player);
		X = Input.GetButton("X"+player);
		Y = Input.GetButton("Y"+player);
		AxisX = Input.GetAxisRaw("AxisX"+player);
		AxisY = Input.GetAxisRaw("AxisY"+player);
		Trigger = Input.GetAxisRaw("Trigger"+player);
		--------------------------------------------------*/
	}
	
	// Get pressed leftstick. If its pressed, change direction of projectile.
	void ChangeDirection()
	{
		if (Input.GetAxisRaw("AxisY"+player) >= 0.1f)
		{
			direction.y -= 0.1f;
		} else if (Input.GetAxisRaw("AxisY"+player) <= -0.1f)
		{
			direction.y += 0.1f;			
		}
		//else { Debug.Log ("LeftStick is not pressed"); }
	}
	
	// Change target if triggered change target event
	void ChangeTarget()
	{
		// Get pressed leftstick. If its pressed, change direction of projectile.
		if (Input.GetAxisRaw("AxisX"+player) >= 0.1f)
		{
			// If target not changed for a while (0.5 sec now)
			if (isTargetReadyToChange)
			{
				Debug.Log ("+");
				switch(target)
				{
				case 0:
					target = 1;
					break;
				case 1:
					target = 2;
					break;
				case 2:
					target = 3;
					break;
				case 3:
					target = 0;
					break;
				default:
					target = 0;
					break;
				}
				//Debug.Log ("Target is switched to "+Targets[0].name);
				isTargetReadyToChange = false;
				StartCoroutine("changeTargetState");
			}
		} else if (Input.GetAxisRaw("AxisX"+player) <= -0.1f)
		{
			// If target not changed for a while (0.5 sec now)
			if (isTargetReadyToChange)
			{
				Debug.Log ("-");
				// Change target and set cooldown for operation
				switch(target)
				{
				case 0:
					target = 3;
					break;
				case 1:
					target = 0;
					break;
				case 2:
					target = 1;
					break;
				case 3:
					target = 2;
					break;
				default:
					target = 0;
					break;
				}
				//Debug.Log ("Target is switched to "+Targets[0].name);
				isTargetReadyToChange = false;
				StartCoroutine("changeTargetState");
			}
		}
		//else { Debug.Log ("LeftStick is not pressed"); }
	}
	
	// Check for pressed button
	void GetKeyState()
	{
		if (!(isPressed[0] || isPressed[1] || isPressed[2] || isPressed[3]))
		{
			if (Input.GetButtonDown("A"+player) && this.GetComponent<Mage>().cooldown[0]==0 &&
			    this.GetComponent<Mage>().cooldown[4]==0) {
				isPressed[0] = true; power = Time.deltaTime;
			}
			if (Input.GetButtonDown("B"+player) && this.GetComponent<Mage>().cooldown[1]==0 &&
			    this.GetComponent<Mage>().cooldown[5]==0) {
				isPressed[1] = true; power = Time.deltaTime;
			}
			if (Input.GetButtonDown("X"+player) && this.GetComponent<Mage>().cooldown[2]==0 &&
			    this.GetComponent<Mage>().cooldown[6]==0) {
				isPressed[2] = true; power = Time.deltaTime;
			}
			if (Input.GetButtonDown("Y"+player) && this.GetComponent<Mage>().cooldown[3]==0 &&
			    this.GetComponent<Mage>().cooldown[7]==0) {
				isPressed[3] = true; power = Time.deltaTime;
			}
			
			int shitHappens = 0, lastPressed = 0;

			// Check for two pressed buttons
			for (int i=0; i<4; i++)
			{
				if (isPressed[i] == true)
				{
					shitHappens++;
					lastPressed = i;
				}
			}
			// Set less ierarhic pressed button as active (A->B->X->Y)
			if (shitHappens >= 2)
				for(int i=0; i<4; i++)
					if (lastPressed != i)
						isPressed[i] = false;
		}
	}
	
	// Command to launch Projectile
	void StartAttack()
	{
		if (isPressed[0] || isPressed[1] || isPressed[2] || isPressed[3])
		{
			if (isPressed[0] && !Input.GetButton("A"+player))
			{
				this.GetComponent<Mage>().SetGlobalCooldown("Attack");
				this.GetComponent<Mage>().SetSkillCooldown("Attack","A");
				GameObject.Find("Earth").GetComponent<ActionScript_Earth>().Attack(direction, power, player);
				UnpressAll();
			}
			if (isPressed[1] && !Input.GetButton("B"+player))
			{
				this.GetComponent<Mage>().SetGlobalCooldown("Attack");
				this.GetComponent<Mage>().SetSkillCooldown("Attack","B");
				GameObject.Find("Fire").GetComponent<ActionScript_Fire>().Attack(direction, power, player);
				UnpressAll();
			}
			if (isPressed[2] && !Input.GetButton("X"+player))
			{
				this.GetComponent<Mage>().SetGlobalCooldown("Attack");
				this.GetComponent<Mage>().SetSkillCooldown("Attack","X");
				GameObject.Find("Water").GetComponent<ActionScript_Water>().Attack(direction, power, player);
				UnpressAll();
			}
			if (isPressed[3] && !Input.GetButton("Y"+player))
			{
				this.GetComponent<Mage>().SetGlobalCooldown("Attack");
				this.GetComponent<Mage>().SetSkillCooldown("Attack","Y");
				GameObject.Find("Air").GetComponent<ActionScript_Air>().Attack(direction, power, player);
				UnpressAll();
			}

			if (power != 0) power += Time.deltaTime*100;
		}
	}
	
	// Command to buff/Debuff target
	void StartBuff()
	{
		if (isPressed[0] || isPressed[1] || isPressed[2] || isPressed[3])
		{
			Debug.Log ("Buff");
			if (isPressed[0] && !Input.GetButton("A"+player))
			{
				this.GetComponent<Mage>().SetGlobalCooldown("Buff");
				this.GetComponent<Mage>().SetSkillCooldown("Buff","A");
				GameObject.Find("Earth").GetComponent<ActionScript_Earth>().BuffOrDebuff(player, Targets[target]);
				UnpressAll();
			}
			if (isPressed[1] && !Input.GetButton("B"+player))
			{
				this.GetComponent<Mage>().SetGlobalCooldown("Buff");
				this.GetComponent<Mage>().SetSkillCooldown("Buff","B");
				GameObject.Find("Fire").GetComponent<ActionScript_Fire>().BuffOrDebuff(player, Targets[target]);
				UnpressAll();
			}
			if (isPressed[2] && !Input.GetButton("X"+player))
			{
				this.GetComponent<Mage>().SetGlobalCooldown("Buff");
				this.GetComponent<Mage>().SetSkillCooldown("Buff","X");
				GameObject.Find("Water").GetComponent<ActionScript_Water>().BuffOrDebuff(player, Targets[target]);
				UnpressAll();
			}
			if (isPressed[3] && !Input.GetButton("Y"+player))
			{
				this.GetComponent<Mage>().SetGlobalCooldown("Buff");
				this.GetComponent<Mage>().SetSkillCooldown("Buff","Y");
				GameObject.Find("Air").GetComponent<ActionScript_Air>().BuffOrDebuff(player, Targets[target]);
				UnpressAll();
			}
		}
	}

	void UnpressAll()
	{
		Debug.Log ("Zahel");
		for (int i=0; i<4; i++)
			isPressed [i] = false;
		power = 0;
		target = 0;
		direction.y=0;
	}

	public void Animate (string action, bool value)
	{
		if (action == "Cast")
			this.GetComponent<Animator>().SetBool ("Cast", value);
	}
	
	// Slow target change
	IEnumerator changeTargetState()
	{
		yield return new WaitForSeconds(0.4f);
		isTargetReadyToChange = true;
	}
}
