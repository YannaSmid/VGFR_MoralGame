using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] public float startingHealth;
    public float currentHealth { get; private set; }
    private Animator anim;
    private bool dead;

    [Header("iFrames")]
    [SerializeField] private float iFramesDuration;
    [SerializeField] private int numberOfFlashes;
    private SpriteRenderer spriteRend;

    [Header("Components")]
    [SerializeField] private Behaviour[] components;
    public bool invulnerable;

    [Header("Death Sound")]
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip hurtSound;

    [Header("Respawn")]
    [SerializeField] private Transform startingPosition; // Assign starting position in Inspector

    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(float _damage)
    {
        if (invulnerable) return;
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);

        if (currentHealth > 0)
        {
            anim.SetTrigger("hurt");
            StartCoroutine(Invunerability());
            SoundManager.instance.PlaySound(hurtSound);
        }
        else
        {
            if (!dead)
            {
                dead = true;
                SoundManager.instance.PlaySound(deathSound);
                StartCoroutine(HandleRespawn());
            }
        }
    }

    public void AddHealth(float _value)
    {
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);
    }

    private IEnumerator Invunerability()
    {
        invulnerable = true;
        Physics2D.IgnoreLayerCollision(10, 11, true);
        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRend.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
            spriteRend.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
        }
        Physics2D.IgnoreLayerCollision(10, 11, false);
        invulnerable = false;
    }

    private IEnumerator HandleRespawn()
    {
        // Delay to allow death animation or sound to finish
        yield return new WaitForSeconds(1.2f);

        Vector3 respawnPosition;

        // Check if last door position exists
        if (Door.lastDoorPosition != null)
        {
            // Respawn near last door
            respawnPosition = Door.lastDoorPosition.position;
            respawnPosition.x -= 1.5f; // Respawn 1.5 units to the left of the door
        }
        else if (startingPosition != null)
        {
            // Respawn at starting position
            respawnPosition = startingPosition.position;
        }
        else
        {
            Debug.LogError("No respawn position set. Cannot respawn player.");
            yield break;
        }

        // Move the player to respawn position
        transform.position = respawnPosition;

        // Move camera
        Transform lastRoom = Door.GetLastDoorRoom();
        if (lastRoom != null)
        {
            lastRoom.GetComponent<Room>().ActivateRoom(true);

            CameraController cameraController = Camera.main.GetComponent<CameraController>();
            if (cameraController != null)
            {
                cameraController.MoveToNewRoom(lastRoom);
            }
            else
            {
                Debug.LogError("CameraController not found!");
            }
        }

        // Respawn player
        Respawn();
    }

    public void Respawn()
    {
        AddHealth(startingHealth);
        anim.ResetTrigger("die");
        anim.Play("Idle");
        StartCoroutine(Invunerability());
        dead = false;

        // Reactivate all attached components
        foreach (Behaviour component in components)
            component.enabled = true;
    }
}
