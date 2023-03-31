using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    // player movement
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float rotationSpeed = 5f;

    // jump variables
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float jumpHeight = 6.0f;
    private bool jumpPressed = false;
    private float gravityValue = -9.81f;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform barrelTransform;
    [SerializeField] private Transform bulletParent;
    [SerializeField] private float bulletHitMissDistance = 25f;
    private CharacterController controller;
    private PlayerInput playerInput;
    [SerializeField] private Transform playerObjTransform;

    private Transform cameraTransform;

    // player actions
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction shootAction;
    private InputAction grappleAction;
    private InputAction jetpackAction;
    private InputAction shopAction;

    // Ammo var
    private int maxAmmo = 10;
    public int currentAmmo;

    // health var
     private int maxHealth = 100;
     public int currentHealth;
    public TextMeshProUGUI HealthText;

    // jetpack var
    public float currentFuel;
    private float maxFuel = 5f;
     Rigidbody rigid;

    // failed grapple var
    public bool freeze;
    public bool activeGrapple;

    //shop vars
    public Canvas shopCanvas;
    bool shopOpen = false;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        rigid = GetComponent<Rigidbody>();
        cameraTransform = Camera.main.transform;
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        shootAction = playerInput.actions["Shoot"];
        grappleAction = playerInput.actions["Grapple"];
        jetpackAction = playerInput.actions["Jetpack"];
        shopAction = playerInput.actions["Shop"];

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        currentAmmo = maxAmmo;
        currentHealth = maxHealth;
        HealthText.text = "Current health; " + currentHealth;
        currentFuel = maxFuel;

        shopCanvas.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        shootAction.performed +=_ =>ShootGun();
    }

    private void OnDisable()
    {
        shootAction.performed -=_ =>ShootGun();
    }

    private void ShootGun()
    {
        
        if(currentAmmo > 0 && shopOpen == false)
        {
        currentAmmo --; 
        RaycastHit hit;
        GameObject bullet = GameObject.Instantiate(bulletPrefab, barrelTransform.position, Quaternion.identity, bulletParent);
        BulletController bulletController = bullet.GetComponent<BulletController>();
        if(Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, Mathf.Infinity))
        {
            bulletController.target = hit.point;
            bulletController.hit = true;
        }
        else
        {
            bulletController.target = cameraTransform.position + cameraTransform.forward *bulletHitMissDistance;
            bulletController.hit = false;
        }
        }
        else
        {
            return;
        }
    }

    void Update()
    {
        // ground movement
        Vector2 input = moveAction.ReadValue<Vector2>();
        Vector3 move = new Vector3(input.x, 0, input.y);
        move = move.x * cameraTransform.right.normalized + move.z * cameraTransform.forward.normalized;

        controller.Move(move * Time.deltaTime * playerSpeed);
        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        MovementJump();
        
        if(Input.GetKey(KeyCode.Q) && currentFuel > 1)
        {
            playerVelocity.y = 1.1f;
            currentFuel -= Time.deltaTime;
            Debug.Log("Fly function triggered");
        }

        // Rotating the player object
    Quaternion targetRotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);
    transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

         // killing the player
    if(currentHealth <= 0)
    {
        Destroy(gameObject);
    }

    // part of failed grappling code
    if(freeze == true)
    {
        rigid.velocity = Vector3.zero;
    }

    // opening the shop ui
    if(shopAction.triggered)
    {
       OpenShop();
       shopOpen = true;
       Cursor.lockState = CursorLockMode.None;
       Cursor.visible = true;
    }

    HealthText.text = "Current health; " + currentHealth;

    }

    public void PlayerTakeDamage(int damage)
    {
        currentHealth -= damage;
    }

    private void MovementJump()
    {
        groundedPlayer = controller.isGrounded;

        // if on ground stop vertical movement
        if(groundedPlayer)
        {
            playerVelocity.y = 0.0f;
        }

        // if on ground and jump pressed jump player
        if(jumpPressed && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -1.0f * gravityValue);
            jumpPressed = false;
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    private void OnJump()
    {
        Debug.Log("Jump pressed");

        // check if no vertical movement
        if(controller.velocity.y == 0)
        {
            jumpPressed = true;
        }
    }

    // shop functions
    public void OpenShop()
    {
        shopCanvas.gameObject.SetActive(true);
        shopOpen = true;
    }

    public void CloseShop()
    {
        shopCanvas.gameObject.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        shopOpen = false;
    }

    // Failed Grappling code WIP
    public Vector3 CalculateJumpVelocity(Vector3 startPoint, Vector3 endPoint, float trajectoryHeight)
    {
        float gravity = Physics.gravity.y;
        float displacementY = endPoint.y - startPoint.y;
        Vector3 displacementXZ = new Vector3(endPoint.x - startPoint.x, 0f, endPoint.z - startPoint.z);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * trajectoryHeight);
        Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * trajectoryHeight / gravity) + Mathf.Sqrt(2 * (displacementY - trajectoryHeight) / gravity));

        return velocityXZ + velocityY;
    }

    public void JumpToPosition(Vector3 targetPosition, float trajectoryHeight)
    {
        activeGrapple = true;

        velocityToSet = CalculateJumpVelocity(transform.position, targetPosition, trajectoryHeight);
        Invoke(nameof(SetVelocity), 0.1f);
    }

    private Vector3 velocityToSet;
    private void SetVelocity()
    {
        rigid.velocity = velocityToSet;
    }

    public void AddAmmo(int amount)
    {
        currentAmmo += amount;
    }

        public void AddHealth(int amount)
    {
        currentHealth += amount;
    }

    public void AddJetpackFuel(int amount)
    {
        currentFuel += amount;
    }
}
