using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Parameters")]
    [SerializeField] public float speed;
    [SerializeField] public float jumpPower;

    [Header("Coyote Time")]
    [SerializeField] private float coyoteTime; //How much time the player can hang in the air before jumping
    private float coyoteCounter; //How much time passed since the player ran off the edge


    [Header("Wall Jumping")]
    [SerializeField] private float wallJumpX; //Horizontal wall jump force
    [SerializeField] private float wallJumpY; //Vertical wall jump force


    [Header("Layers")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;

    [Header("Sounds")]
    [SerializeField] private AudioClip jumpSound;

    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D boxCollider;
    private float wallJumpCooldown;
    private float horizontalInput;

    public bool canJump = true; // Tracks if player can jump


    private void Awake()
    {
        //Grab references for rigidbody and animator from object
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (DialogueManager.GetInstance().dialogueIsPlaying)
        {
            return;
        }
        
        horizontalInput = Input.GetAxis("Horizontal");

        //Flip player when moving left-right
        if (horizontalInput > 0.01f)
            transform.localScale = Vector3.one;
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1);

        //Set animator parameters
        anim.SetBool("run", horizontalInput != 0);
        anim.SetBool("grounded", isGrounded());

        //Jump
        if (Input.GetKeyDown(KeyCode.Space))
            Jump();

        //Adjustable jump height
        if (Input.GetKeyUp(KeyCode.Space) && body.velocity.y > 0)
            body.velocity = new Vector2(body.velocity.x, body.velocity.y / 2);

        if (onWall())
        {
            // Allow sliding
            body.velocity = new Vector2(0, Mathf.Max(body.velocity.y, -5)); // Limit slide speed to -5
        }
        else
        {
            body.gravityScale = 7; // Ensure gravity when not on wall
            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

            if (isGrounded())
            {
                coyoteCounter = coyoteTime; // Reset coyote counter when on ground
                canJump = true; // Allow jumping again when grounded
            }
            else if (coyoteCounter > 0)
            {
                coyoteCounter -= Time.deltaTime; // Decrease coyote counter in air
            }

        }
    }


    private void Jump()
    {
        // Allow jump only if grounded, within coyote time, or on a wall, and ensure the player hasn't jumped already
        if ((!isGrounded() && coyoteCounter <= 0 && !onWall()) || !canJump) return;

        // Play jump sound
        SoundManager.instance.PlaySound(jumpSound);

        if (onWall())
        {
            WallJump(); // Perform wall jump
        }
        else
        {
            // Perform normal jump
            body.velocity = new Vector2(body.velocity.x, jumpPower);
        }

        // Disable jumping until the player is grounded again
        canJump = false;

        // Reset coyote time to prevent additional air jumps
        coyoteCounter = 0;
    }


    private void WallJump()
    {
        body.AddForce(new Vector2(-Mathf.Sign(transform.localScale.x) * wallJumpX, wallJumpY));
        wallJumpCooldown = 0;
    }


    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }
    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }
    public bool canAttack()
    {
        return horizontalInput == 0 && isGrounded() && !onWall();
    }
}