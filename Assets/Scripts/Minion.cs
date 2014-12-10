using UnityEngine;
using System.Collections;

public class Minion : MonoBehaviour
{
	// Mage stats	
    public float Damage;
    public float Health;
	public float Defence;
	public int player;
	
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void moveMinion(float deltaX)
    {
		this.transform.position += new Vector3 (deltaX, 0, 0);
    }
}
