using UnityEngine;
using System.Collections;

public class AliveEntity : MonoBehaviour {

	protected int CurrentHP = 1;

	public void GetHit()
	{
		CurrentHP--;
		if (CurrentHP <= 0)
			Die ();
	}

	public void Die()
	{
		Destroy (gameObject);
	}

	// Update is called once per frame
	void Update () {
	
	}
}
