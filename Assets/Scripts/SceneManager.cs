using UnityEngine;
using System.Collections;

public class SceneManager : MonoBehaviour
{
	public GameObject[] minion;
	public float moveMinion;
	
    // Use this for initialization
    public void Start()
    {
		minion = new GameObject[2] {
				GameObject.Find ("MinionsRed"),
				GameObject.Find ("MinionsBlue")
			};
		StartCoroutine("moveMinionsCor");
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator moveMinionCor()
    {
		for(;;)
		{
			moveMinion = minion[0].GetComponent<Minion>().Defence - minion[0].GetComponent<Minion>().Damage;
			minion[0].GetComponent<Minion>().moveMinion(moveMinion);
			minion[1].GetComponent<Minion>().moveMinion(moveMinion);
			
			yield return new WaitForSeconds(1.0f);	
		}
    }
}
