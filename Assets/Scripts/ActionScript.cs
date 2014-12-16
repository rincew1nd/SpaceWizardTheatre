using UnityEngine;
using System.Collections;

public class ActionScript : MonoBehaviour {

	public float damageValue, healValue, buffDamage, buffDefence;
	public GameObject[] minion;
	
    public virtual void Start()
	{
		minion = new GameObject[2] {
				GameObject.Find ("RedMinion"),
				GameObject.Find ("BlueMinion")
			};
		damageValue = 3f;
		healValue = 2f;
		buffDamage = 0.5f;
		buffDefence = 0.3f;
	}
	public virtual void AttackMage(GameObject gameobject)
	{
		Destroy(this.gameObject);
	}
	public virtual void AttackMinion(GameObject gameobject, bool self)
	{
		Destroy(this.gameObject);
	}
}
