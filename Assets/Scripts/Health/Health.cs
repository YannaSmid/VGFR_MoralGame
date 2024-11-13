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

    [Header("Respawn")]
    [SerializeField] private Transform startingPosition; // Assign starting position
    private Transform currentCheckpoint; // Tracks most recent checkpoint

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
        }
        else
        {
            if (!dead)
            {
                dead = true;
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
        // Delay to allow death animation or sound to finish if needed
        yield return new WaitForSeconds(0.2f);

        Vector3 respawnPosition;

        // Check if a checkpoint exists
        if (currentCheckpoint != null)
        {
            // Respawn at most recent checkpoint
            respawnPosition = currentCheckpoint.position;
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

        // Move player to respawn position
        transform.position = respawnPosition;

        // Move camera to appropriate room
        CameraController cameraController = Camera.main.GetComponent<CameraController>();
        if (cameraController != null)
        {
            // Check if current checkpoint's parent exists
            Transform room = currentCheckpoint != null ? currentCheckpoint.parent : null;
            if (room != null)
            {
                room.GetComponent<Room>().ActivateRoom(true);
                cameraController.MoveToNewRoom(room);
            }
            else
            {
                Debug.LogError("Room reference for current checkpoint not found!");
            }
        }
        else
        {
            Debug.LogError("CameraController not found!");
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if player interacts with a checkpoint
        if (collision.CompareTag("Checkpoint"))
        {
            currentCheckpoint = collision.transform; // Update current checkpoint
            Debug.Log("Checkpoint updated: " + currentCheckpoint.name);
        }
    }
}
