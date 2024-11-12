using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MysteryBox : MonoBehaviour
{
    public bool opened = false;
    float m_Hue = 0f;
    float m_Saturation = 1f;
    float m_Value = 1f;

    public float changeSpeed = 0.1f;
    public GameObject MysteryItem;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!opened){
            if (m_Hue > 1f){
                m_Hue = 0f;
            }
            m_Hue += changeSpeed * Time.deltaTime;
            this.GetComponent<SpriteRenderer>().color = Color.HSVToRGB(m_Hue, m_Saturation, m_Value);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !opened)
        {
            opened = true;
            //this.gameObject.SetActive(false);
            this.GetComponent<SpriteRenderer>().color = Color.HSVToRGB(m_Hue, m_Saturation, 0.5f);
            MysteryItem.SetActive(true);
        }
    }
}
