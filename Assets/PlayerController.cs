using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("移動・視点設定")]
    public float moveSpeed = 4.0f;
    public float gravity = -9.81f;
    public float mouseSensitivity = 2.0f;
    public float verticalClamp = 80.0f;

    [Header("着席設定")]
    public GameObject interactUI;
    public Transform sitPosition;
    public float standingEyeHeight = 1.6f;

    private CharacterController controller;
    private Transform cameraTransform;
    private float xRotation = 0f;
    private float yRotation = 0f;
    private Vector3 velocity;

    private bool isNearChair = false;
    private bool isSeated = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        cameraTransform = GetComponentInChildren<Camera>().transform;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (interactUI != null) interactUI.SetActive(false);
    }

    void Update()
    {
        HandleLook();

        if (!isSeated)
        {
            HandleMove();
        }

        // Fキー：着席 / 離席
        if (isNearChair && Input.GetKeyDown(KeyCode.F))
        {
            ToggleSit();
        }

        // Gキー：座っている時に完全にランダムでマップ遷移
        if (isSeated && Input.GetKeyDown(KeyCode.G))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.GoToRandomMap();
            }
            else
            {
                Debug.LogError("GameManagerが見つかりません！taiki SceneにGameManagerを配置してください。");
            }
        }
    }

    void HandleLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -verticalClamp, verticalClamp);

        if (isSeated)
        {
            yRotation += mouseX;
            cameraTransform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
        }
        else
        {
            cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            transform.Rotate(Vector3.up * mouseX);
        }
    }

    void HandleMove()
    {
        if (controller.isGrounded && velocity.y < 0) velocity.y = -2f;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * moveSpeed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void ToggleSit()
    {
        isSeated = !isSeated;

        if (isSeated)
        {
            if (sitPosition != null)
            {
                cameraTransform.position = sitPosition.position;
                cameraTransform.rotation = sitPosition.rotation;
            }
            xRotation = 0f;
            yRotation = 0f;
            if (interactUI != null) interactUI.SetActive(false);
        }
        else
        {
            transform.Rotate(Vector3.up * yRotation);
            cameraTransform.localPosition = new Vector3(0, standingEyeHeight, 0);
            cameraTransform.localRotation = Quaternion.identity;
            xRotation = 0f;
            yRotation = 0f;
            if (isNearChair && interactUI != null) interactUI.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Chair"))
        {
            isNearChair = true;
            if (!isSeated && interactUI != null) interactUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Chair"))
        {
            isNearChair = false;
            if (interactUI != null) interactUI.SetActive(false);
        }
    }
}