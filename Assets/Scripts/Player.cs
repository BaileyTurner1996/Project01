using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float jumpForce = 13f;
    private float speed = 8f;
    private float horizontal;
    private bool doubleJump;

    public Transform groundCheck;
    public LayerMask groundLayer;
 
    public Rigidbody2D rb;
    public string currentColor;
    public SpriteRenderer sr;
    public Color colorGreen;
    public Color colorYellow;
    public Color colorBlue;
    public Color colorRed;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        //SetRandomColor();
        currentColor = "Blue";
        sr.color = colorBlue;
    }

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        if (IsGrounded() && (!Input.GetButton("Jump") && !Input.GetMouseButton(0) && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.UpArrow)))
        {
            doubleJump = false;
        }

        if (Input.GetButtonDown("Jump") || Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (IsGrounded() || doubleJump)
            {
                rb.velocity = Vector2.up * jumpForce;
                doubleJump = !doubleJump;
            }
        }

        if ((Input.GetButtonUp("Jump") || Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow)) && rb.velocity.y > 0f)
         {
             rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
         }
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        ProcessCollision(collision.gameObject);
    } 
    void OnCollisionEnter2D(Collision2D collision)
    {
        ProcessCollision(collision.gameObject);
    }

    bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    void SetRandomColor()
    {
        int index = Random.Range (0, 4);

        //Add in some way of ensuring that the next random will not be the current case
        switch (index)
        {
            case 0:
                currentColor = "Green";
                sr.color = colorGreen;
                break;
            case 1:
                currentColor = "Yellow";
                sr.color = colorYellow;
                break;
            case 2:
                currentColor = "Blue";
                sr.color = colorBlue;
                break;
            case 3:
                currentColor = "Red";
                sr.color = colorRed;
                break;
        }
    }

    void toggleSymbols()
    {
        
    }
    void ProcessCollision(GameObject collision)
    {
        if (collision.gameObject.tag == "ColorChangerRandom")
        {
            SetRandomColor();
            //Figure out how to enable and disable Color Changer instead of deleting
            Destroy(collision.gameObject);
            return;
        }
        else if (collision.gameObject.tag == "ColorChangerGreen"){
            currentColor = "Green";
            sr.color = colorGreen;
            Destroy(collision.gameObject);
            return;
        }
        else if (collision.gameObject.tag == "ColorChangerYellow")
        {
            currentColor = "Yellow";
            sr.color = colorYellow;
            Destroy(collision.gameObject);
            return;
        }
        else if (collision.gameObject.tag == "ColorChangerRed")
        {
            currentColor = "Red";
            sr.color = colorRed;
            Destroy(collision.gameObject);
            return;
        }
        else if (collision.gameObject.tag == "ColorChangerBlue")
        {
            currentColor = "Blue";
            sr.color = colorBlue;
            Destroy(collision.gameObject);
            return;
        }

        if (collision.gameObject.tag == currentColor)
        {
            Destroy(collision.gameObject);

        }
        /*else if (collision.gameObject.tag != currentColor)
        {
            
        }*/
    }
}
