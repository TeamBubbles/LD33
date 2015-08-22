using UnityEngine;
using System.Collections;

public class AliveEntity : MonoBehaviour {

	protected int CurrentHP = 1;

	public void GetHit()
	{
		CurrentHP--;
	}

	// Update is called once per frame
	void Update () {
	
	}
}
