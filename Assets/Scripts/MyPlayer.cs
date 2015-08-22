using UnityEngine;
using System.Collections;

public class MyPlayer : AliveEntity {

#region Public Variables

	public float maxSpeed = 10f;
	public float jumpForce;

	public bool isJumping = false;
	public bool isAttacking = false;
	public float currentSpeed = 0.0f;

	public Transform groundCheck;
	public LayerMask whatIsGround;

	public Transform[] leftChecks;
	public Transform[] rightChecks;
	
	public Transform playerBlade;
#endregion

#region Private Members
	bool facingRight = true;
	bool grounded = false;
	float groundRadius = 0.1f;
	float sideRadius = 0.2f;
	Rigidbody2D rb2d;
#endregion

	Animator animator;

	// Use this for initialization
	void Start () {
		playerBlade = transform.FindChild ("PlayerBlade");
		animator = gameObject.GetComponent<Animator> ();
		rb2d = gameObject.GetComponent<Rigidbody2D> ();
	}
	
	void Update() {

		if (isAttacking) {
			return;
		}

		if (Input.GetButtonDown ("Fire1") && grounded) {
			isAttacking = true;
			animator.SetBool("IsAttacking", true);
			playerBlade.gameObject.SetActive(true);
			return;
		}

		if (grounded && Input.GetButtonDown ("Jump")) {
			isJumping = true;
			animator.SetBool("IsOnGround", false);
			rb2d.AddForce(new Vector2(0f, jumpForce));
		}

		if (grounded && isJumping)
			isJumping = false;
	}

	void FixedUpdate () {
		grounded = Physics2D.OverlapCircle (groundCheck.position, groundRadius, whatIsGround);
		animator.SetBool ("IsOnGround", grounded);
		animator.SetFloat ("yVelocity", rb2d.velocity.y);
		
		animator.SetFloat ("xVelocity", Mathf.Abs(rb2d.velocity.x));

		if (isAttacking)
			return;

		float xInput = Input.GetAxis ("Horizontal");

		Transform[] actualLeft = facingRight ? leftChecks : rightChecks;
		Transform[] actualRight = facingRight ? rightChecks : leftChecks;

		bool sticky = false;

		for (int i = 0; i < actualLeft.Length; i++) {
			if((xInput < 0 && Physics2D.OverlapCircle(actualLeft[i].position, sideRadius, whatIsGround)) ||
			   (xInput > 0 && Physics2D.OverlapCircle(actualRight[i].position, sideRadius, whatIsGround)))
				sticky = true;
		}

		if(sticky && !grounded)
		{

			Debug.Log("Spiderman");

			return;
		}
		rb2d.velocity = new Vector2 (xInput * maxSpeed, rb2d.velocity.y);

		animator.SetFloat ("xVelocity", Mathf.Abs(rb2d.velocity.x));
		
		if (xInput > 0 && !facingRight)
			Flip ();
		else if (xInput < 0 && facingRight)
			Flip ();
	}

	void Flip()
	{
		facingRight = !facingRight;
		Vector3 scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale;
	}
	
}
