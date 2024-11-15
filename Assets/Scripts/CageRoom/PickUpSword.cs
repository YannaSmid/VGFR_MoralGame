using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class PickUpSword : MonoBehaviour
{
    public bool swordAvailable = false;
    private Animator anim;
    public AnimatorController swordanim;
    
    // Start is called before the first frame update
    void Start()
    {
        //choiceObject = GameObject.Find("CookieChoice");
        //foodGiven = choiceObject.GetComponent<GiveFood>();
        anim = GameObject.Find("Player").GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && swordAvailable)
        {
            // foodGiven.pickedUp = true;
            // foodGiven.GiveAway(2); // activate choice dialogue
            collision.GetComponent<PlayerAttack>().hasSword = true;
            collision.GetComponent<Animator>().runtimeAnimatorController = swordanim;
            this.gameObject.SetActive(false);
        }
    }
}
