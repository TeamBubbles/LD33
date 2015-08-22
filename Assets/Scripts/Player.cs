using UnityEngine;
using System.Collections;

public class Player : AliveEntity {

    public float maxJumpHeight = 4;
    public float minJumpHeight = 1;
    public float timeToJumpApex = .4f;
    float accelerationTimeAirborne = .2f;
    float accelerationTimeGrounded = .1f;
    float moveSpeed = 6;

    public Vector2 wallJumpClimb;
    public Vector2 wallJumpOff;
    public Vector2 wallLeap;

    public float wallSlideSpeedMax = 3;
    public float wallStickTime = .25f;
    float timeToWallUnstick;

    float gravity;
    float maxJumpVelocity;
    float minJumpVelocity;
    public Vector3 velocity;
    float velocityXSmoothing;

	bool isAttacking = false;
	Animator animator;

	bool facingRight = true;

    Controller2D controller;

    void Start()
    {
        controller = GetComponent<Controller2D>();
		animator = gameObject.GetComponent<Animator> ();

        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
        print("Gravity: " + gravity + "  Jump Velocity: " + maxJumpVelocity);
    }

    void Update()
    {
//		if (isAttacking) {
//			Transform arm = transform.FindChild("player_arm");
//			arm.RotateAround(armJoint.transform.position, Vector3.forward, Time.deltaTime * armRotationTime);
//			//arm.Rotate(new Vector3(0f, 0f, Time.deltaTime * armRotationTime));
//			if(Mathf.Abs(Mathf.DeltaAngle(arm.rotation.eulerAngles.z, 0f)) < 2f)
//			{
//				isAttacking = false;
//				reverseAttack = true;
//			}
//		}
//		else if (reverseAttack)
//		{
//			Transform arm = transform.FindChild("player_arm");
//			arm.RotateAround(armJoint.transform.position, Vector3.forward, Time.deltaTime * (-armRotationTime));
//			if(Mathf.Abs(Mathf.DeltaAngle(arm.rotation.eulerAngles.z, 325f)) < 2f)
//			{
//				reverseAttack = false;
//			}
//		}

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        int wallDirX = (controller.collisions.left) ? -1 : 1;

        float targetVelocityX = input.x * moveSpeed;

		if (targetVelocityX < 0f && facingRight) {
			facingRight = false;
			transform.Rotate(new Vector3(0f, 180f));
			Debug.Log ("facing left");
		} else if (targetVelocityX > 0f && !facingRight) {
			facingRight = true;
			transform.Rotate(new Vector3(0f, 180f));
			Debug.Log("facing right");
		}

		if(!facingRight)
			targetVelocityX *= -1;

        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
		animator.SetFloat ("xVelocity", velocity.x);


        bool wallSliding = false;
        if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && velocity.y < 0)
        {
            wallSliding = true;

            if (velocity.y < -wallSlideSpeedMax)
            {
                velocity.y = -wallSlideSpeedMax;
            }

            if (timeToWallUnstick > 0)
            {
                velocityXSmoothing = 0;
                velocity.x = 0;

                if (input.x != wallDirX && input.x != 0)
                {
                    timeToWallUnstick -= Time.deltaTime;
                }
                else
                {
                    timeToWallUnstick = wallStickTime;
                }
            }
            else
            {
                timeToWallUnstick = wallStickTime;
            }

        }

        if (Input.GetButtonDown("Jump"))
        {
            if (wallSliding)
            {
                if (wallDirX == input.x)
                {
                    velocity.x = -wallDirX * wallJumpClimb.x;
                    velocity.y = wallJumpClimb.y;
                }
                else if (input.x == 0)
                {
                    velocity.x = -wallDirX * wallJumpOff.x;
                    velocity.y = wallJumpOff.y;
                }
                else
                {
                    velocity.x = -wallDirX * wallLeap.x;
                    velocity.y = wallLeap.y;
                }
            }
            if (controller.collisions.below)
            {
                velocity.y = maxJumpVelocity;
            }
        }
        if (Input.GetButtonUp("Jump"))
        {
            if (velocity.y > minJumpVelocity)
            {
                velocity.y = minJumpVelocity;
            }
        }

		if (Input.GetButton ("Fire1") && !wallSliding) 
		{
			isAttacking = true;
		}


        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime, input);

        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
        }
    }
}
