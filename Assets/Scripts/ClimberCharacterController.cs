using UnityEngine;

public class ClimberCharacterController : MonoBehaviour 
{
	bool facingRight = true;							// For determining which way the player is currently facing.

	[SerializeField] float maxSpeed = 10f;				// The fastest the player can travel in the x axis.	

	
	[SerializeField] LayerMask whatIsGround;			// A mask determining what is ground to the character
    [SerializeField] LayerMask whatIsClimbable;			// A mask determining what is ground to the character
	
	Transform groundCheck;								// A position marking where to check if the player is grounded.
	float groundedRadius = .2f;							// Radius of the overlap circle to determine if grounded
	bool grounded = false;								// Whether or not the player is grounded.						// Radius of the overlap circle to determine if the player can stand up
	Animator anim;										// Reference to the player's animator component.
    bool walled = false;
    CharacterController controller;

    void Awake()
	{
		// Setting up references.
		groundCheck = transform.Find("GroundCheckClimber");
		anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
	}


	void FixedUpdate()
	{
        float charRadius = collider2D.bounds.size.x / 2 + 2f;
        Vector3 charCenter = new Vector3(collider2D.bounds.center.x, collider2D.bounds.center.y + 1.5f, 0);
		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		grounded = Physics2D.OverlapCircle(groundCheck.position, groundedRadius, whatIsGround);
        walled = Physics2D.OverlapCircle(charCenter, charRadius, whatIsClimbable);
		anim.SetBool("Ground", grounded);

		// Set the vertical animation
		anim.SetFloat("vSpeed", rigidbody2D.velocity.y);
	}


	public void Move(float move, float climbing)
	{

		//only control the player if grounded or airControl is turned on


			// The Speed animator parameter is set to the absolute value of the horizontal input.
			anim.SetFloat("Speed", Mathf.Abs(move));

			// Move the character
			rigidbody2D.velocity = new Vector2(move * maxSpeed, rigidbody2D.velocity.y);
			
			// If the input is moving the player right and the player is facing left...
			if(move > 0 && !facingRight)
				// ... flip the player.
				Flip();
			// Otherwise if the input is moving the player left and the player is facing right...
			else if(move < 0 && facingRight)
				// ... flip the player.
				Flip();
		
        if (walled)
        {
            //if (climbing > 0)
            //{
            //    Vector3 newPosition = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
            //    transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * 10f);
            //}
            //else if (climbing < 0)
            //{
            //    Vector3 newPosition = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
            //    transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * 1f);
            //}
            //transform.position = new Vector3(transform.position.x, transform.position.y + climbing, transform.position.z);
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, climbing * 13f);


        }


	}

	
	void Flip ()
	{
		// Switch the way the player is labelled as facing.
		facingRight = !facingRight;
		
		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}
