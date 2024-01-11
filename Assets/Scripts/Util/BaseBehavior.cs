using UnityEngine;
using UnityEngine.U2D;

public abstract class BaseBehavior : MonoBehaviour {

    #region Unity lazy-loaded property definitions
    /// <summary>
    /// This SpriteRenderer component for this GameObject, if one exists.
    /// </summary>
    protected SpriteRenderer Renderer {
        get {
            return GetComponentOrNull(ref rendererRef);
        }
    }
    private SpriteRenderer rendererRef = null;

    /// <summary>
    /// This Animator component for this GameObject, if one exists.
    /// </summary>
    protected Animator Animator {
        get {
            return GetComponentOrNull(ref animatorRef);
        }
    }
    private Animator animatorRef = null;

    /// <summary>
    /// The Camera component for this GameObject, if one exists.
    /// </summary>
    protected Camera Camera {
        get {
            return GetComponentOrNull(ref cameraRef);
        }
    }
    private Camera cameraRef = null;

    /// <summary>
    /// The PixelPerfectCamera component for this GameObject, if one exists.
    /// </summary>
    protected PixelPerfectCamera PixelPerfectCamera {
        get {
            return GetComponentOrNull(ref pixelPerfectCameraRef);
        }
    }
    private PixelPerfectCamera pixelPerfectCameraRef = null;
    #endregion

    #region Helpful wrapper properties
    protected Vector3 Position {
        get {
            return transform.position;
        }
        set {
            transform.position = value;
        }
    }
    #endregion

    protected T GetComponentOrNull<T>(ref T comp) where T : Component {
        if (comp == null && !TryGetComponent(out comp)) {
            print($"An reference attempt was made on null component of type {typeof(T).Name}.");
        }
        return comp;
    }

    #region Unity function overrides
    // Unity start function.
    private void Start() {
        OnStart();
    }

    /// <summary>
    /// Override this function to define actions to take when this GameObject is loaded.
    /// </summary>
    protected abstract void OnStart();

    // Unity update function.
    private void Update() {
        OnUpdate(Time.deltaTime);
    }

    /// <summary>
    /// Override this function to define actions to take when Unity make a call to Update().
    /// </summary>
    /// <param name="delta">The time in seconds since the last call to this function.</param>
    protected virtual void OnUpdate(float delta) { }

    // Unity fixed update function.
    private void FixedUpdate() {
        OnFixedUpdate(Time.deltaTime);
    }

    /// <summary>
    /// Override this function to define actions to take when Unity make a call to FixedUpdate().
    /// This function is called every frame, if one is set.
    /// </summary>
    /// <param name="delta">The time in seconds since the last call to this function.</param>
    protected virtual void OnFixedUpdate(float delta) { }

    protected virtual void OnCollisionEnter2D(Collision2D collision) { }
    #endregion
}