using Unity.VisualScripting;
using UnityEngine;

public class ToggleCollision : MonoBehaviour
{
    public Player playerScript;
    public string thisColor;

    private void Start()
    {
       // thisColor = playerScript.currentColor;
    }

    void Update()
    {
        thisColor = playerScript.currentColor;

        if (gameObject.tag == thisColor)
        {
            //turn off collision
            gameObject.GetComponent<Collider2D>().isTrigger = true;
            //Debug.Log("Same color");

        }
        else if (gameObject.tag != thisColor)
        {
            //turn on collision
            gameObject.GetComponent<Collider2D>().isTrigger = false;
           // Debug.Log("not same color :(");
        }
    }
}
