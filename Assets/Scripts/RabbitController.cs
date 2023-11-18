using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RabbitController : MonoBehaviour
{
    [Header("Horizontal Movement")]
    public float moveSpeed = 10f;
    public Vector2 direction;
    private bool facingRight = true;
    private int facingDirection = 1;
    private float knockBackForce = 2.5f;

    [Header("Vertical Movement")]
    public float jumpSpeed = 15f;
    public float jumpDelay = 0.25f;
    private float jumpTimer;
    private bool canDoubleJump;

    [Header("Components")]
    public Rigidbody2D rb;
    public Animator animator;
    public LayerMask groundLayer;
    public GameObject characterHolder;

    [Header("Physics")]
    public float maxSpeed = 7f;
    public float linearDrag = 4f;
    public float gravity = 1f;
    public float fallMultiplier = 5f;

    [Header("Collision")]
    public bool onGround = false;
    public float groundLength = 0.6f;
    public Vector3 colliderOffset;

    [Header("Health")]
    public int health = 1;

    [Header("Buffs")]
    public int redBuff =0;
    public int blueBuff = 0;

    void Update()
    { 
        //Raycast checking if player is on ground (needed for jumps)
        bool wasOnGround = onGround;
        onGround = Physics2D.Raycast(transform.position + colliderOffset, Vector2.down, groundLength, groundLayer) || Physics2D.Raycast(transform.position - colliderOffset, Vector2.down, groundLength, groundLayer);

        if (!wasOnGround && onGround)
        {
            StartCoroutine(JumpSqueeze(1.15f, 0.8f, 0.05f)); //squeezing jump animation
            animator.SetBool("Jump", false);
        }

        if (Input.GetButtonDown("Jump")) //main jumping animation
        {
            jumpTimer = Time.time + jumpDelay;
            animator.SetBool("Jump", true);
        }
        

        direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (onGround)
        {
            canDoubleJump = true;
        }
    }
    void FixedUpdate()
    {
        moveCharacter(direction.x);
        if (jumpTimer > Time.time) //falling animation
        {
            if (onGround)
            {
                Jump();
            }
            else
            {
                if (canDoubleJump)
                {
                    Jump();
                }
                canDoubleJump = false;
            }
        }
        

        ModifyPhysics();

    }
    void moveCharacter(float horizontal) //movement method
    {
        rb.AddForce(Vector2.right * horizontal * moveSpeed); 

        if ((horizontal > 0 && !facingRight) || (horizontal < 0 && facingRight)) //codtition to flip character 
        {
            Flip();
            
        }
        if (Mathf.Abs(rb.velocity.x) > maxSpeed) // speed limitation
        {
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y);
        }
        animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
        
    }
    void Jump() //jumping method
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
        jumpTimer = 0;
        StartCoroutine(JumpSqueeze(0.5f, 1.2f, 0.1f));
        animator.SetBool("Jump", true);
    }
    void Hurt() // damage income
    {
        health--;
        animator.SetTrigger("hit");
        rb.velocity = new Vector2(-facingDirection * knockBackForce, knockBackForce);
        if (health <= 0)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        
    }
    public void OnTriggerEnter2D(Collider2D collision) //checking if player collides whith the enemy and getting damage
    {
        if (collision.gameObject.name == "DeathZone")
        {
            Hurt();
        }
        if (collision.gameObject.name == "blueBuff")
        {
            blueBuff++;
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.name == "redBuff")
        {
            redBuff++;
            
            Destroy(collision.gameObject);

        }

    }
 

    void ModifyPhysics()
    {
        bool changingDirections = (direction.x > 0 && rb.velocity.x < 0) || (direction.x < 0 && rb.velocity.x > 0);

        if (onGround) // gravity drag while changing direction or stopping
        {
            if (Mathf.Abs(direction.x) < 0.4f || changingDirections)
            {
                rb.drag = linearDrag;
            }
            else
            {
                rb.drag = 0f;
            }
            rb.gravityScale = 0;
            
        }
        else
        {
            rb.gravityScale = gravity;
            rb.drag = linearDrag * 0.15f;
            if (rb.velocity.y < 0)
            {
                rb.gravityScale = gravity * fallMultiplier;
            }
            
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        transform.rotation = Quaternion.Euler(0, facingRight ? 0 : 180, 0);
        facingDirection = -facingDirection;
    }
    IEnumerator JumpSqueeze(float xSqueeze, float ySqueeze, float seconds) //jumping sqeeze
    {
        Vector3 originalSize = Vector3.one;
        Vector3 newSize = new Vector3(xSqueeze, ySqueeze, originalSize.z);
        float t = 0f;
        while (t <= 1.0)
        {
            t += Time.deltaTime / seconds;
            characterHolder.transform.localScale = Vector3.Lerp(originalSize, newSize, t);
            yield return null;
        }
        t = 0f;
        while (t <= 1.0)
        {
            t += Time.deltaTime / seconds;
            characterHolder.transform.localScale = Vector3.Lerp(newSize, originalSize, t);
            yield return null;
        }

    }
    private void OnDrawGizmos() //red lines that checks different collisions
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + colliderOffset, transform.position + colliderOffset + Vector3.down * groundLength);
        Gizmos.DrawLine(transform.position - colliderOffset, transform.position - colliderOffset + Vector3.down * groundLength);
    }
    
}