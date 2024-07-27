using UnityEngine;

public class CameraController : MonoBehaviour
{
    private GameInputs gameInputs;
    private Transform cameraTransform;
    private Vector2 smoothedInputVelocity;
    private Vector2 smoothedInput;
    private float smoothedMouseDeltaInputX;
    private float smoothedMouseDelta;

    [Header("MapSettings")]
    [SerializeField] private float mapScale = 1;
    [Header("MovementSettings")]
    [SerializeField] private float reguralSpeed = 10;
    [Header("RotationSettings")]
    [Range(0.01f,0.5f)][SerializeField] private float cameraSensivity = 0.05f;

    private void Awake()
    {
        gameInputs = new();
        cameraTransform = this.GetComponentInChildren<Camera>().transform;
    }
    void Update()
    {
        CameraMovement();
        RotateCamera();
    }
    void CameraMovement()
    {

        var cameraMovementInput = new Vector2(gameInputs.Camera.Move.ReadValue<Vector2>().x, gameInputs.Camera.Move.ReadValue<Vector2>().y);

        smoothedInput = Vector2.SmoothDamp(smoothedInput, cameraMovementInput, ref smoothedInputVelocity, 0.2f);
        var cameraMovementInputToVector3 = new Vector3(smoothedInput.x, 0, smoothedInput.y);

        transform.position += cameraMovementInputToVector3 * reguralSpeed * mapScale * Time.deltaTime;
    }

    private void RotateCamera()
    {
        var mouseDeltaInput = gameInputs.Camera.RotateCamera.ReadValue<Vector2>();
        smoothedMouseDeltaInputX = Mathf.SmoothDamp(smoothedMouseDeltaInputX, mouseDeltaInput.x, ref smoothedMouseDelta, 0.2f);


        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, smoothedMouseDeltaInputX * cameraSensivity + transform.rotation.eulerAngles.y, 0f);
    }



    private void OnEnable()
    {
        gameInputs.Enable();
    }
    private void OnDisable()
    {
        gameInputs.Disable();
    }
}
