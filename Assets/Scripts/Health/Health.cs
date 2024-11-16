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
                anim.SetTrigger("die");
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
        // Delay for death animation
        yield return new WaitForSeconds(0.7f);

        Vector3 respawnPosition;

        // Access PlayerRespawn component
        PlayerRespawn playerRespawn = GetComponent<PlayerRespawn>();
        if (playerRespawn != null && playerRespawn.currentCheckpoint != null)
        {
            respawnPosition = playerRespawn.currentCheckpoint.position;
            Debug.Log("Respawning at checkpoint: " + playerRespawn.currentCheckpoint.name);
        }
        else if (startingPosition != null)
        {
            respawnPosition = startingPosition.position;
            Debug.LogWarning("No checkpoint found! Respawning at starting position.");
        }
        else
        {
            Debug.LogError("No respawn position set. Cannot respawn player.");
            yield break;
        }

        // Move player to respawn position
        transform.position = respawnPosition;

        // Lock camera position to current room
        CameraController cameraController = Camera.main.GetComponent<CameraController>();
        if (cameraController != null)
        {
            cameraController.LockCameraPosition();
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

        // Unlock camera if necessary
        CameraController cameraController = Camera.main.GetComponent<CameraController>();
        if (cameraController != null)
        {
            cameraController.UnlockCameraPosition();
        }
    }

}
