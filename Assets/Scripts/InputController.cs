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
	//public bool X, Y, A, B, attack, buff;
	//public float AxisX, AxisY, Trigger;
	
	void Start()
	{
		Targets = new GameObject[4] {
				GameObject.Find("BlueMage"),
				GameObject.Find ("BlueMinion"),
				GameObject.Find ("RedMinion"),
				GameObject.Find("RedMage")
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
			if (Input.GetAxisRaw("Trigger"+player) >= 0.1f || Input.GetKey(KeyCode.Q) || Input.GetButton("attack_button_"+player))
			{
				PaintArrow();
				ChangeDirection();
				GetKeyState();
				StartAttack();
			} else if (Input.GetAxisRaw("Trigger"+player) <= -0.1f || Input.GetKey(KeyCode.A) || Input.GetButton("buff_button_"+player)) {
				ChangeTarget();
				GetKeyState();
				StartBuff();
			} else if (isPressed[0] || isPressed[1] || isPressed[2] || isPressed[3])
			{
				// If trigger is not pressed, but element button is, damage yourself as punishment
				this.GetComponent<Mage>().Hurt(-1.0f);
				this.GetComponent<Mage>().setInterrupted();
				UnpressAll();
			}
		}
		/*----- Debug log. Usefull sometimes! ------
		if (this.GetComponent<Mage>().isDead || this.GetComponent<Mage>().isInterrupted || this.GetComponent<Mage>().isFreezed || this.GetComponent<Mage>().isBroken || this.GetComponent<Mage>().isWinded)
		{
			if (this.GetComponent<Mage>().isDead) Debug.Log("Dead");
			if (this.GetComponent<Mage>().isInterrupted) Debug.Log("Interrupted");
			if (this.GetComponent<Mage>().isFreezed) Debug.Log("Freezed");
			if (this.GetComponent<Mage>().isBroken) Debug.Log("Broken");
			if (this.GetComponent<Mage>().isWinded) Debug.Log("Winded");
		}
		/*--------------------------------------------------
		A = Input.GetButton("A"+player);
		B = Input.GetButton("B"+player);
		X = Input.GetButton("X"+player);
		Y = Input.GetButton("Y"+player);
		AxisX = Input.GetAxisRaw("AxisX"+player);
		AxisY = Input.GetAxisRaw("AxisY"+player);
		Trigger = Input.GetAxisRaw("Trigger"+player);
		buff = Input.GetButton("buff_button_"+player);
		attack = Input.GetButton("attack_button_"+player);
		--------------------------------------------------*/
	}

	void PaintArrow()
	{
		GameObject arrow;
		arrow = GameObject.Find("BlueMageArrow");
		arrow.transform.localScale = new Vector3(power/300f, power/300f,0f);
	}
	
	// Get pressed leftstick. If its pressed, change direction of projectile.
	void ChangeDirection()
	{
		if (Input.GetAxisRaw("AxisY"+player) >= 0.1f  || Input.GetKey(KeyCode.UpArrow))
		{
			if (direction.y > 0) {
				direction.y -= 0.1f;
				if (player == 1)
					GameObject.Find("BlueMageArrow").transform.Rotate(new Vector3(0,0,-1.3f));
				else
					GameObject.Find("RedMageArrow").transform.Rotate(new Vector3(0,0,1.3f));
			} else direction.y = 0;
		} else if (Input.GetAxisRaw("AxisY"+player) <= -0.1f || Input.GetKey(KeyCode.DownArrow))
		{
			if (direction.y < 5f)
			{
				direction.y += 0.1f;
				if (player == 1)
					GameObject.Find("BlueMageArrow").transform.Rotate(new Vector3(0,0,1.3f));
				else
					GameObject.Find("RedMageArrow").transform.Rotate(new Vector3(0,0,-1.3f));
			} else direction.y = 5f;
		}

		if (direction.y >= 4 && direction.y <= 5.2) direction.x = 1.5f;
		if (direction.y >= 3 && direction.y < 4) direction.x = 2;
		if (direction.y >= 2 && direction.y < 3) direction.x = 3;
		if (direction.y >= 1 && direction.y < 2) direction.x = 5f;
		if (direction.y >= -1 && direction.y < 1) direction.x = 7;

		if (player == 2) direction.x *= -1;
		//else { Debug.Log ("LeftStick is not pressed"); }
	}
	
	// Change target if triggered change target event
	void ChangeTarget()
	{
		// Get pressed leftstick. If its pressed, change direction of projectile.
		if (Input.GetAxisRaw("AxisX"+player) >= 0.1f || Input.GetKey(KeyCode.RightArrow))
		{
			// If target not changed for a while (0.5 sec now)
			if (isTargetReadyToChange)
			{
				resetSelectedUnit(target);
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
					target = (player == 1) ? 0 : 3;
					break;
				}
				//Debug.Log ("Target is switched to "+Targets[0].name);
				isTargetReadyToChange = false;
				StartCoroutine("changeTargetState");
				selectedUnit(target);
			}
		} else if (Input.GetAxisRaw("AxisX"+player) <= -0.1f || Input.GetKey(KeyCode.LeftArrow))
		{
			// If target not changed for a while (0.5 sec now)
			if (isTargetReadyToChange)
			{
				resetSelectedUnit(target);
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
					target = (player == 1) ? 0 : 3;
					break;
				}
				//Debug.Log ("Target is switched to "+Targets[0].name);
				isTargetReadyToChange = false;
				StartCoroutine("changeTargetState");
				selectedUnit(target);
			}
		}
	}
	
	// Check for pressed button
	void GetKeyState()
	{
		if (!(isPressed[0] || isPressed[1] || isPressed[2] || isPressed[3]))
		{
			if ((Input.GetButtonDown("A"+player) || Input.GetKey(KeyCode.K)) && this.GetComponent<Mage>().cooldown[0]==0 &&
			    this.GetComponent<Mage>().cooldown[4]==0) {
				isPressed[0] = true; power = Time.deltaTime;
			}
			if ((Input.GetButtonDown("B"+player) || Input.GetKey(KeyCode.L)) && this.GetComponent<Mage>().cooldown[1]==0 &&
			    this.GetComponent<Mage>().cooldown[5]==0) {
				isPressed[1] = true; power = Time.deltaTime;
			}
			if ((Input.GetButtonDown("X"+player) || Input.GetKey(KeyCode.J)) && this.GetComponent<Mage>().cooldown[2]==0 &&
			    this.GetComponent<Mage>().cooldown[6]==0) {
				isPressed[2] = true; power = Time.deltaTime;
			}
			if ((Input.GetButtonDown("Y"+player) || Input.GetKey(KeyCode.I)) && this.GetComponent<Mage>().cooldown[3]==0 &&
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
			if (isPressed[0] && !Input.GetButton("A"+player) && !Input.GetKey(KeyCode.K))
			{
				this.GetComponent<Mage>().SetGlobalCooldown("Attack");
				this.GetComponent<Mage>().SetSkillCooldown("Attack","A");
				GameObject.Find("Earth").GetComponent<ActionScript_Earth>().Attack(direction, power, player, this.transform.position);
				UnpressAll();
			}
			if (isPressed[1] && !Input.GetButton("B"+player) && !Input.GetKey(KeyCode.L))
			{
				this.GetComponent<Mage>().SetGlobalCooldown("Attack");
				this.GetComponent<Mage>().SetSkillCooldown("Attack","B");
				GameObject.Find("Fire").GetComponent<ActionScript_Fire>().Attack(direction, power, player, this.transform.position);
				UnpressAll();
			}
			if (isPressed[2] && !Input.GetButton("X"+player) && !Input.GetKey(KeyCode.J))
			{
				this.GetComponent<Mage>().SetGlobalCooldown("Attack");
				this.GetComponent<Mage>().SetSkillCooldown("Attack","X");
				GameObject.Find("Water").GetComponent<ActionScript_Water>().Attack(direction, power, player, this.transform.position);
				UnpressAll();
			}
			if (isPressed[3] && !Input.GetButton("Y"+player) && !Input.GetKey(KeyCode.I))
			{
				this.GetComponent<Mage>().SetGlobalCooldown("Attack");
				this.GetComponent<Mage>().SetSkillCooldown("Attack","Y");
 				GameObject.Find("Air").GetComponent<ActionScript_Air>().Attack(direction, power, player, this.transform.position);
				UnpressAll();
			}

			if (power != 0 && power < 140f) power += Time.deltaTime*50;
		}
	}
	
	// Command to buff/Debuff target
	void StartBuff()
	{
		if (isPressed[0] || isPressed[1] || isPressed[2] || isPressed[3])
		{
			if (isPressed[0] && !Input.GetButton("A"+player) && !Input.GetKey(KeyCode.K))
			{
				Debug.Log("Earth");
				this.GetComponent<Mage>().SetGlobalCooldown("Buff");
				this.GetComponent<Mage>().SetSkillCooldown("Buff","A");
				GameObject.Find("Earth").GetComponent<ActionScript_Earth>().BuffOrDebuff(player, Targets[target]);
				UnpressAll();
			}
			if (isPressed[1] && !Input.GetButton("B"+player) && !Input.GetKey(KeyCode.L))
			{
				Debug.Log("Fire");
				this.GetComponent<Mage>().SetGlobalCooldown("Buff");
				this.GetComponent<Mage>().SetSkillCooldown("Buff","B");
				GameObject.Find("Fire").GetComponent<ActionScript_Fire>().BuffOrDebuff(player, Targets[target]);
				UnpressAll();
			}
			if (isPressed[2] && !Input.GetButton("X"+player) && !Input.GetKey(KeyCode.J))
			{
				Debug.Log("Water");
				this.GetComponent<Mage>().SetGlobalCooldown("Buff");
				this.GetComponent<Mage>().SetSkillCooldown("Buff","X");
				GameObject.Find("Water").GetComponent<ActionScript_Water>().BuffOrDebuff(player, Targets[target]);
				UnpressAll();
			}
			if (isPressed[3] && !Input.GetButton("Y"+player) && !Input.GetKey(KeyCode.I))
			{
				Debug.Log("Air");
				this.GetComponent<Mage>().SetGlobalCooldown("Buff");
				this.GetComponent<Mage>().SetSkillCooldown("Buff","Y");
				GameObject.Find("Air").GetComponent<ActionScript_Air>().BuffOrDebuff(player, Targets[target]);
				UnpressAll();
			}
		}
	}

	void UnpressAll()
	{
		for (int i=0; i<4; i++)
			isPressed [i] = false;
		power = 0;
		target = (player == 1) ? 0 : 3;
		direction.y=1;
		if (player == 1)
		{
			GameObject.Find("BlueMageArrow").transform.localScale = new Vector3(0,0,0);
			GameObject.Find("BlueMageArrow").transform.rotation = new Quaternion(0,0,0,360);
		} else {
			GameObject.Find("RedMageArrow").transform.localScale = new Vector3(0,0,0);
			GameObject.Find("RedMageArrow").transform.rotation = new Quaternion(0,0,0,147);
		}
	}

	public void Animate (string action, bool value)
	{
		if (action == "Cast")
			this.GetComponent<Animator>().SetBool ("Cast", value);
	}

	public void selectedUnit(int target_copy)
	{
		//for (int i=0; i<4; i++)
		//	Targets[i].renderer.material.color = Color.white;
		if (player == 1)
			Targets[target_copy].renderer.material.color = Color.blue;
		else
			Targets[target_copy].renderer.material.color = Color.red;
	}
	public void resetSelectedUnit(int target_copy)
	{
		Targets[target_copy].renderer.material.color = Color.white;
	}
	
	// Slow target change
	IEnumerator changeTargetState()
	{
		yield return new WaitForSeconds(0.4f);
		isTargetReadyToChange = true;
	}
}
