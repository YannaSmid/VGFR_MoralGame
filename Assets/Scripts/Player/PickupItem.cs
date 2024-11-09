using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour
{
    //private GameObject choiceObject;
    public GiveFood foodGiven;
    // Start is called before the first frame update
    void Start()
    {
        //choiceObject = GameObject.Find("CookieChoice");
        //foodGiven = choiceObject.GetComponent<GiveFood>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            foodGiven.pickedUp = true;
            foodGiven.GiveAway(2); // activate choice dialogue
            this.gameObject.SetActive(false);
        }
    }
}
