using UnityEngine;
using UnityEngine.InputSystem;
using Context = UnityEngine.InputSystem.InputAction.CallbackContext;

public class CameraMovement : BaseBehavior {

    #region Properties
    /// <summary>
    /// Gets the XY coordinates for the active mouse's current position.
    /// </summary>
    public static Vector2 CurrentMousePosition {
        get {
            return Mouse.current.position.ReadValue();
        }
    }

    public static Vector2 TranslatedMousePosition {
        get {
            var cameraObject = GameObject.Find("Main Camera");
            var camera = cameraObject.GetComponent<Camera>();
            var screenPos = CurrentMousePosition;
            var cameraPos = camera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, camera.transform.position.z));
            return new Vector3(-cameraPos.x, cameraPos.y, cameraPos.z);
        }
    }
    private Vector2 lastCameraStartPosition;
    private bool isCameraMoving = false;

    [Tooltip("The input mapping to use for controlling the camera.")]
    [SerializeField]
    private InputActionReference MovementInput;

    [SerializeField]
    private float MoveRate = 1f;
    #endregion

    protected override void OnStart() {
        // Add event handlers for when the mouse button is pushed and released.
        MovementInput.action.performed += CameraMoved;
        MovementInput.action.canceled += CameraStopped;
    }

    protected override void OnUpdate(float delta) {
        if (isCameraMoving) {
            var diff = (CurrentMousePosition - lastCameraStartPosition) * MoveRate;
            var cellSize = PixelPerfectCamera.assetsPPU;
            // These if statements cause the camera to move stepwise (1 tile at a time).
            if (cellSize < Mathf.Abs(diff.x)) {
                lastCameraStartPosition = CurrentMousePosition;
                Position += new Vector3(diff.x < 0 ? 1 : -1, 0);
            }
            if (cellSize < Mathf.Abs(diff.y)) {
                lastCameraStartPosition = CurrentMousePosition;
                Position += new Vector3(0, diff.y < 0 ? 1 : -1);
            }
        }
    }

    /// <summary>
    /// Handler for when the mouse button is pressed.
    /// </summary>
    private void CameraMoved(Context context) {
        print("poop");
        lastCameraStartPosition = CurrentMousePosition;
        isCameraMoving = true;
    }

    /// <summary>
    /// Handler for when the mouse button is released.
    /// </summary>
    private void CameraStopped(Context context) {
        print(CurrentMousePosition);
        isCameraMoving = false;
    }

}
