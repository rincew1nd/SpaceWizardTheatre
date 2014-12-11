using UnityEngine;
using System.Collections;

public class ActionScript_Water : ActionScript
{
	private Vector2 direction;
	private float power;
	private int player;
	private int targetNum;
	private Vector3 pivot;
	
	public override void Start()
	{
		damageValue = 3;
		healValue = 2;
	}
	
	public void Attack(Vector2 direction, float power, int player_copy, Vector3 pivot_copy)
	{
		player = player_copy;
		pivot = pivot_copy;

		GameObject waterball;
		waterball = (GameObject)Instantiate(gameObject, pivot, Quaternion.identity);
		if (direction.x<0)waterball.transform.Rotate(0, 180, 0);
		waterball.rigidbody2D.AddForce(direction * power);
		waterball.layer = (player == 1) ? LayerMask.NameToLayer("Blue") : LayerMask.NameToLayer("Red");
	}
	public void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.tag == "Mage")
			AttackMage(other.gameObject);
		if (other.gameObject.tag == "Minion")
			AttackMinion(other.gameObject);
	}
	public override void AttackMage(GameObject hitedMage)
	{
		hitedMage.GetComponent<Mage>().Hurt(1f);
		hitedMage.GetComponent<Mage>().setFreezed();
		Destroy(this.gameObject);
	}
	public override void AttackMinion(GameObject gameobject)
	{
		minion[0].GetComponent<Minion>().moveMinion(damageValue*0.1f);
		minion[1].GetComponent<Minion>().moveMinion(damageValue*0.1f);
		Destroy(this.gameObject);
	}
	
	
	public void BuffOrDebuff(int player, GameObject target)
	{		
		if (target.tag == "Mage")
			targetNum = target.GetComponent<Mage> ().player;
		else if (target.tag == "Minion")
			targetNum = target.GetComponent<Minion>().player;
		
		if (player==targetNum)//False is Red
		{
			if (target.tag == "Mage")//MageRed Buff himself
				BuffMage(target);
			if (target.tag == "Minion")//MageRed Buff his Minion
				BuffMinion(target);
		} else {
			if (target.tag == "Mage")//MageRed Buff himself
				DeBuffMage(target);
			if (target.tag == "Minion")//MageRed Buff his Minion
				DeBuffMinion(target);
		}
	}
	public void BuffMage(GameObject gameobject)
	{
		gameobject.GetComponent<Mage>().Health += healValue;
	}
	public void BuffMinion(GameObject gameobject)
	{
		gameobject.GetComponent<Minion>().Defence += buffDefence;
	}
	public void DeBuffMage(GameObject gameobject)
	{
		gameobject.GetComponent<Mage>().Shield -= buffDefence; //TODO: Cooldown
	}
	public void DeBuffMinion(GameObject gameobject)
	{
		gameobject.GetComponent<Minion>().Defence -= buffDefence;
	}
}