using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement3D : MonoBehaviour
{
    [SerializeField] private float speed = 2f;          
    [SerializeField] private float rotationSpeed = 360f; 
    [SerializeField] private Transform cameraTransform;  // Reference to the camera's transform
    [SerializeField] private GameObject winScreenPanel;  // Reference to the "You Won" screen panel
    [SerializeField] private GameObject startScreenPanel; // Reference to the start screen panel

    private PlayerControls controls;   
    private Vector2 moveInput;         
    private Animator anim;             
    private bool gameStarted = false;  // Flag to track if the game has started

    private void Awake()
    {
        try
        {
            controls = new PlayerControls();

            controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
            controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error initializing PlayerControls or binding actions: " + ex.Message);
        }
    }

    private void OnEnable()
    {
        try
        {
            controls?.Enable();
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error enabling controls: " + ex.Message);
        }
    }

    private void OnDisable()
    {
        try
        {
            controls?.Disable();
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error disabling controls: " + ex.Message);
        }
    }

    private void Start()
    {
        try
        {
            anim = GetComponent<Animator>();
            if (anim == null)
            {
                Debug.LogWarning("Animator component not found on the player.");
            }

            // Ensure the win screen and start screen are set correctly
            if (winScreenPanel != null)
            {
                winScreenPanel.SetActive(false);
            }

            if (startScreenPanel != null)
            {
                startScreenPanel.SetActive(true); // Show the start screen initially
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error in Start method: " + ex.Message);
        }    
    }

    private void Update()
    {
        if (!gameStarted) return; // Do nothing if the game hasn't started yet

        try
        {
            // Get the camera's forward and right directions
            Vector3 cameraForward = cameraTransform.forward;
            Vector3 cameraRight = cameraTransform.right;

            // Flatten the camera directions on the Y-axis
            cameraForward.y = 0;
            cameraRight.y = 0;
            cameraForward.Normalize();
            cameraRight.Normalize();

            // Calculate the movement direction relative to the camera
            Vector3 moveDirection = cameraForward * moveInput.y + cameraRight * moveInput.x;

            // Move the player
            transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);

            // Rotate the player to face the direction of movement
            if (moveDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }

            // Update the animator's IsMoving parameter
            bool isMoving = moveInput != Vector2.zero;
            if (anim != null)
            {
                anim.SetBool("IsMoving", isMoving);
            }

            // Attack animation
            if (Input.GetKeyDown(KeyCode.X) && anim != null)
            {
                anim.SetTrigger("Attack");
            }

            // Roll animation
            if (Input.GetKeyDown(KeyCode.Q) && anim != null)
            {
                anim.SetTrigger("Rolling");
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error in Update method: " + ex.Message);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        try
        {
            // Check if the collided object has the tag "Sphere" and is on the appropriate layer
            if (collision.gameObject.CompareTag("Sphere") && collision.gameObject.layer == LayerMask.NameToLayer("Sphere"))
            {
                ShowWinScreen();
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error in OnCollisionEnter method: " + ex.Message);
        }
    }

    private void ShowWinScreen()
    {
        // Enable the win screen panel
        if (winScreenPanel != null)
        {
            winScreenPanel.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Win Screen Panel is not assigned in the Inspector.");
        }
    }

    public void StartGame()
    {
        // Called when the Start Game button is clicked
        gameStarted = true;

        // Hide the start screen
        if (startScreenPanel != null)
        {
            startScreenPanel.SetActive(false);
        }
    }
}
