using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSlash : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] public float dealingDamage = 1;

    public bool slashing = false;
    private bool hit;
    public float lifetime;
    
    private CapsuleCollider2D boxCollider;

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = this.GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hit){
            slashing = false;
            gameObject.SetActive(false);
            return;
        }
        lifetime += Time.deltaTime;
        if (lifetime > 2){
            slashing = false;
            gameObject.SetActive(false);
        }
    }

    public void SlashAttack(){
        lifetime = 0;
        slashing = true;
        gameObject.SetActive(true);
        hit = false;
        //boxCollider.enabled = true;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.tag == "Enemy" && slashing){

            collision.GetComponent<Health>()?.TakeDamage(1);
            Debug.Log("Hit someone");
            slashing = false;
            hit = true;
        }

        //boxCollider.enabled = false;
        //Debug.Log("Sword attack");
    }
}
