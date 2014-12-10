using UnityEngine;
using System.Collections;

public class inputtest : MonoBehaviour {

	public int player;
	public bool X, Y, A, B;
	public float AxisX, AxisY, Trigger;

	// Use this for initialization
	void Start () {
		player = 2;
	}
	
	// Update is called once per frame
	void Update () {
		A = Input.GetButton("A"+player);
		B = Input.GetButton("B"+player);
		X = Input.GetButton("X"+player);
		Y = Input.GetButton("Y"+player);
		AxisX = Input.GetAxisRaw("AxisX"+player);
		AxisY = Input.GetAxisRaw("AxisY"+player);
		Trigger = Input.GetAxisRaw("Trigger"+player);

		if (B) this.transform.position += new Vector3 (0.1f, 0, 0);
		if (X) this.transform.position += new Vector3 (-0.1f, 0, 0);
		if (Y) this.transform.position += new Vector3 (0, 0.1f, 0);
		if (A) this.transform.position += new Vector3 (0, -0.1f, 0);
	}
}
