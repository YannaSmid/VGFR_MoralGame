using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImmortalItem : MonoBehaviour
{

    public bool pickedUp = false;
    float m_Hue = 0f;
    float m_Saturation = 1f;
    float m_Value = 1f;

    public float changeSpeed = 0.1f;
    [SerializeField] public TextAsset inkJSON;
    [SerializeField] private AudioClip itemSound;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!pickedUp){
            if (m_Hue > 1f){
                m_Hue = 0f;
            }
            m_Hue += changeSpeed * Time.deltaTime;
            this.GetComponent<SpriteRenderer>().color = Color.HSVToRGB(m_Hue, m_Saturation, m_Value);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !pickedUp)
        {
            pickedUp = true;
            GainImmortality(collision.gameObject);
            
        }
    }

    private void GainImmortality(GameObject player){

        player.GetComponent<Health>().invulnerable = true;
        DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
        SoundManager.instance.PlaySound(itemSound);
        this.gameObject.SetActive(false);

    }
}
