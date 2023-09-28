using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEditor.PackageManager;
using System;
using System.Collections;

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

    public TextMeshProUGUI countText;
    public GetChildren childScript;
    public float currentCount;
    public static event Action OnPlayerDeath;

    [SerializeField] AudioClip _burstSound = null;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        //SetRandomColor();
        currentColor = "Blue";
        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(2).gameObject.SetActive(false);
        transform.GetChild(3).gameObject.SetActive(true);
        transform.GetChild(4).gameObject.SetActive(false);
        sr.color = colorBlue;

        SetCountText();
        EnablePlayerMovement();
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

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            Camera.main.cullingMask = Camera.main.cullingMask ^ (1 << 7);
        }


        SetCountText();

        if (currentCount == 0)
        {
            OnPlayerDeath?.Invoke();
            DisablePlayerMovement();
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
        int index = UnityEngine.Random.Range (0, 4);

        //Add in some way of ensuring that the next random will not be the current case
        switch (index)
        {
            case 0:
                currentColor = "Green";
                sr.color = colorGreen;
                transform.GetChild(1).gameObject.SetActive(true);
                transform.GetChild(2).gameObject.SetActive(false);
                transform.GetChild(3).gameObject.SetActive(false);
                transform.GetChild(4).gameObject.SetActive(false);
                break;
            case 1:
                currentColor = "Yellow";
                transform.GetChild(1).gameObject.SetActive(false);
                transform.GetChild(2).gameObject.SetActive(true);
                transform.GetChild(3).gameObject.SetActive(false);
                transform.GetChild(4).gameObject.SetActive(false);
                sr.color = colorYellow;
                break;
            case 2:
                currentColor = "Blue";
                transform.GetChild(1).gameObject.SetActive(false);
                transform.GetChild(2).gameObject.SetActive(false);
                transform.GetChild(3).gameObject.SetActive(true);
                transform.GetChild(4).gameObject.SetActive(false);
                sr.color = colorBlue;
                break;
            case 3:
                currentColor = "Red";
                transform.GetChild(1).gameObject.SetActive(false);
                transform.GetChild(2).gameObject.SetActive(false);
                transform.GetChild(3).gameObject.SetActive(false);
                transform.GetChild(4).gameObject.SetActive(true);
                sr.color = colorRed;
                break;
        }
    }

    void ProcessCollision(GameObject collision)
    {
        if (collision.gameObject.tag == "ColorChangerRandom")
        {
            SetRandomColor();
            StartCoroutine(BreakCC());
            return;
        }
        else if (collision.gameObject.tag == "ColorChangerGreen"){
            currentColor = "Green";
            sr.color = colorGreen;
            transform.GetChild(1).gameObject.SetActive(true);
            transform.GetChild(2).gameObject.SetActive(false);
            transform.GetChild(3).gameObject.SetActive(false);
            transform.GetChild(4).gameObject.SetActive(false);
            StartCoroutine(BreakCC());
            return;
        }
        else if (collision.gameObject.tag == "ColorChangerYellow")
        {
            currentColor = "Yellow";
            sr.color = colorYellow;
            transform.GetChild(1).gameObject.SetActive(false);
            transform.GetChild(2).gameObject.SetActive(true);
            transform.GetChild(3).gameObject.SetActive(false);
            transform.GetChild(4).gameObject.SetActive(false);
            StartCoroutine(BreakCC());
            return;
        }
        else if (collision.gameObject.tag == "ColorChangerRed")
        {
            currentColor = "Red";
            sr.color = colorRed;
            transform.GetChild(1).gameObject.SetActive(false);
            transform.GetChild(2).gameObject.SetActive(false);
            transform.GetChild(3).gameObject.SetActive(false);
            transform.GetChild(4).gameObject.SetActive(true);
            StartCoroutine(BreakCC());
            return;
        }
        else if (collision.gameObject.tag == "ColorChangerBlue")
        {
            currentColor = "Blue";
            sr.color = colorBlue;
            transform.GetChild(1).gameObject.SetActive(false);
            transform.GetChild(2).gameObject.SetActive(false);
            transform.GetChild(3).gameObject.SetActive(true);
            transform.GetChild(4).gameObject.SetActive(false);
            StartCoroutine(BreakCC());
            return;
        }

        if (collision.gameObject.tag == currentColor)
        {
            if (collision.gameObject.GetComponent<BoxCollider2D>() != null)
            {
                StartCoroutine(BreakPlatform());
            }
            else
            {
                StartCoroutine(BreakCircle());
            }
            //StartCoroutine(BreakPlatform());

        }
        /*else if (collision.gameObject.tag != currentColor)
        {
            
        }*/
        //Debug.Log(currentCount);
        //SetCountText();

        IEnumerator BreakPlatform()
        {
            SpriteRenderer sr = collision.gameObject.GetComponent<SpriteRenderer>();
            BoxCollider2D bc = collision.gameObject.GetComponent<BoxCollider2D>();
            AudioHelper.PlayClip2d(_burstSound, 1);
            ParticleSystem platformParticle = collision.gameObject.GetComponentInChildren<ParticleSystem>();
            platformParticle.Play();
            sr.enabled = false;
            bc.enabled = false;
            yield return new WaitForSeconds(.5f);
            Destroy(collision.gameObject);
        }
        IEnumerator BreakCC()
        {
            SpriteRenderer sr = collision.gameObject.GetComponent<SpriteRenderer>();
            CircleCollider2D bc = collision.gameObject.GetComponent<CircleCollider2D>();
            AudioHelper.PlayClip2d(_burstSound, 1);
            ParticleSystem platformParticle = collision.gameObject.GetComponentInChildren<ParticleSystem>();
            platformParticle.Play();
            sr.enabled = false;
            bc.enabled = false;
            yield return new WaitForSeconds(.5f);
            Destroy(collision.gameObject);
        }
        IEnumerator BreakCircle()
        {
            SpriteRenderer sr = collision.gameObject.GetComponent<SpriteRenderer>();
            PolygonCollider2D bc = collision.gameObject.GetComponent<PolygonCollider2D>();
            AudioHelper.PlayClip2d(_burstSound, 1);
            ParticleSystem platformParticle = collision.gameObject.GetComponentInChildren<ParticleSystem>();
            platformParticle.Play();
            sr.enabled = false;
            bc.enabled = false;
            yield return new WaitForSeconds(.5f);
            Destroy(collision.gameObject);
        }
    }
    
    void SetCountText()
    {
        currentCount = childScript.counter;
        countText.text = "Items Left: " + currentCount.ToString();
    }

    void DisablePlayerMovement()
    {
        rb.bodyType = RigidbodyType2D.Static;
    }

    void EnablePlayerMovement()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
    }


}
