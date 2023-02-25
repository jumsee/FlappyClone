using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRigidBody;
    private Vector3 beginPos;
    private float delay;
    public float pushForce;
    public bool isPaused = true;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        beginPos = transform.position;
        playerRigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Limit y to 12 (stops player from exiting screen)
        if (transform.position.y >= 12)
        {
            Transform playerTransform = transform;
            Vector3 newPosition = playerTransform.position;
            newPosition = new Vector3(newPosition.x, 12, newPosition.z);
            playerTransform.position = newPosition;
            playerRigidBody.velocity = Vector3.zero;
        }

        // Decay input delay
        if (delay > 0f)
        {
            delay -= Time.deltaTime;
        }

        // Manage input (jumping)
        if (Input.GetKey(KeyCode.UpArrow) && delay <= 0)
        {
            if (isPaused)
            {
                ChangePause();
            }
            audioSource.Play();
            // Apply jump force
            // Gravity managed by RigidBody
            playerRigidBody.AddForce(transform.up * pushForce, ForceMode.Impulse);
            delay = 0.25F;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Collision with floor or tube means Game Over
        if (collision.gameObject.CompareTag("Floor") || collision.gameObject.CompareTag("Tube"))
        {
            transform.position = beginPos;
            ChangePause();
        }
    }

    private void ChangePause()
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            GameManager.instance.ResetGame();
        }
        else
        {
            GameManager.instance.StartGame();
        }

        // Allow y movement (falling) only if game isn't paused
        playerRigidBody.constraints = isPaused
            ? RigidbodyConstraints.FreezeAll
            : (RigidbodyConstraints) 122;
    }
}