using UnityEngine;
using System.Collections;

public class BladeHit : MonoBehaviour {

	GameObject player;

	void Start()
	{
		player = transform.parent.gameObject;
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if(player.GetComponent<MyPlayer>().isAttacking
			&& other.gameObject.GetComponent<Guard>() != null)
				other.gameObject.GetComponent<Guard>().GetHit();
	}
}
