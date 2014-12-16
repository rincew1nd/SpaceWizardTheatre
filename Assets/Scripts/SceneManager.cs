using UnityEngine;
using System.Collections;

public class SceneManager : MonoBehaviour
{
	public GameObject[] minion;
	public float deltaMoveMinion;
	
    // Use this for initialization
    public void Start()
    {
		minion = new GameObject[2] {
				GameObject.Find ("RedMinion"),
				GameObject.Find ("BlueMinion")
			};
		StartCoroutine("moveMinionCor");
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator moveMinionCor()
	{
		yield return new WaitForSeconds(0.55f);
		for(;;)
		{
			yield return new WaitForSeconds(0.65f);
			
			deltaMoveMinion = (minion[0].GetComponent<Minion>().Defence - minion[1].GetComponent<Minion>().Damage)/-50;
			minion[0].GetComponent<Minion>().moveMinion(deltaMoveMinion);
			minion[1].GetComponent<Minion>().moveMinion(deltaMoveMinion);
		}
    }
}
