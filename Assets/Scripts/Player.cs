using UnityEngine;

public class Player : MonoBehaviour
{
    public float jumpForce = 10f;
    public Rigidbody2D rb;
    public string currentColor;
    public SpriteRenderer sr;
    public Color colorGreen;
    public Color colorYellow;
    public Color colorBlue;
    public Color colorRed;

    void Start()
    {
        SetRandomColor();
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump") || Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            rb.velocity = Vector2.up * jumpForce;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {

        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        ProcessCollision(collision.gameObject);
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        ProcessCollision(collision.gameObject);
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

    void ProcessCollision(GameObject collision)
    {
        if (collision.gameObject.tag == "ColorChanger")
        {
            SetRandomColor();
            //Figure out how to enable and disable Color Changer instead of deleting
            Destroy(collision.gameObject);
            return;
        }
        if (collision.gameObject.tag == currentColor)
        {
            //turn off collision
            gameObject.GetComponent<Collider2D>().isTrigger = true;
            Debug.Log("Same color");

        }
        else if (collision.gameObject.tag != currentColor)
        {
            //turn on collision
            gameObject.GetComponent<Collider2D>().isTrigger = false;
            Debug.Log("not same color :(");
        }
    }
}
