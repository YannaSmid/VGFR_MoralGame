using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] private AudioClip coinSound; // Drag the coin sound effect here in the Inspector
    private AudioSource audioSource;

    private void Start()
    {
        // Ensure the AudioSource component exists on the CoinPickupManager
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.playOnAwake = false; // Ensure it doesn't play automatically
    }

    public void HandleCoinPickup(GameObject coin)
    {
        // Play the coin sound
        if (coinSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(coinSound);
        }

        // Disable the coin GameObject
        coin.SetActive(false);

        Debug.Log("Coin collected and disabled!");
    }
}
