using UnityEngine;
using System.Collections;

public class BaseEnemy : AliveEntity {

	public GameObject player;
	public LayerMask rayMask;
	
	public float maxSpeed;
	
	bool sawPlayer = false;
	bool facingRight = true;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
		//Debug.LogWarning(Vector2.Distance (player.transform.position, transform.position));
		
		RaycastHit2D rayToPlayer = Physics2D.Raycast(transform.position, 
		                                             Vector3.Normalize(player.transform.position - transform.position), 100f, rayMask);
		
		if (rayToPlayer.collider != null && rayToPlayer.collider.tag == "Player") {
			if(transform.position.x > player.transform.position.x && !facingRight || 
			   transform.position.x < player.transform.position.x && facingRight)
			{
				Debug.LogWarning("Saw you !");
				GetComponent<Rigidbody2D>().velocity = new Vector3(maxSpeed, 0f);
			}
		}
		
	}
}
