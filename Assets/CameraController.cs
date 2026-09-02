using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class CameraController : MonoBehaviour
{
    [Header("監視カメラ設定")]
    public Camera[] securityCameras;       // 3つのカメラを割り当てる
    public TextMeshProUGUI cameraNameText; // UIテキスト（例: CAM 01）

    [Header("360度視点操作設定")]
    public float mouseSensitivity = 2.0f;
    public float verticalClamp = 80.0f;

    [Header("シーン遷移設定")]
    public string officeSceneName = "taiki Scene"; // 戻り先のシーン名

    private int currentCameraIndex = 0;
    private float xRotation = 0f;
    private float yRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // 常に Security camera 1 (インデックス0) からスタート
        currentCameraIndex = 0;
        UpdateCameraView();
    }

    void Update()
    {
        // 1. マウス操作による360度視点回転
        HandleCameraLook();

        // 2. 左クリックでカメラ切替（1 -> 2 -> 3 -> 1）
        if (Input.GetMouseButtonDown(0))
        {
            SwitchToNextCamera();
        }

        // 3. Gキーで即座にオフィス（taiki Scene）へ戻る
        if (Input.GetKeyDown(KeyCode.G))
        {
            SceneManager.LoadScene(officeSceneName);
        }
    }

    void HandleCameraLook()
    {
        if (securityCameras.Length == 0 || securityCameras[currentCameraIndex] == null) return;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -verticalClamp, verticalClamp);

        yRotation += mouseX;

        securityCameras[currentCameraIndex].transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }

    void SwitchToNextCamera()
    {
        currentCameraIndex = (currentCameraIndex + 1) % securityCameras.Length;

        xRotation = 0f;
        yRotation = 0f;

        UpdateCameraView();
    }

    void UpdateCameraView()
    {
        for (int i = 0; i < securityCameras.Length; i++)
        {
            if (securityCameras[i] != null)
            {
                bool isActive = (i == currentCameraIndex);
                securityCameras[i].gameObject.SetActive(isActive);

                if (isActive)
                {
                    securityCameras[i].transform.localRotation = Quaternion.identity;
                }
            }
        }

        if (cameraNameText != null)
        {
            cameraNameText.text = $"CAM 0{currentCameraIndex + 1}";
        }
    }
}