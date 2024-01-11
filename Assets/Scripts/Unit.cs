using UnityEngine;
using Context = UnityEngine.InputSystem.InputAction.CallbackContext;

public partial class Unit : BaseBehavior {

    private Vector2Int lastCoordinates;

    protected override void OnStart() {
        InitializeComponents();
        MovementInput.action.performed += OnDestinationSelected;
    }

    protected override void OnUpdate(float delta) {
        if (lastCoordinates != CurrentCoordinates) {
            lastCoordinates = CurrentCoordinates;
        }
    }

    private void OnDestinationSelected(Context obj) {
        var gridManager = GridManager.Instance;
        var startCoords = poop();
        var newPos = CameraMovement.TranslatedMousePosition;
        var destination = gridManager.FindCoordinates(newPos);
        var destCoords = new Vector2Int(1 + destination.x, destination.y);
        var path = GridManager.Instance.FindPath(startCoords, destCoords);
        print(path);
    }

    private Vector2Int poop() {
        var gridManager = GridManager.Instance;
        var coords = gridManager.FindCoordinates(new Vector3(1 + transform.position.x, -transform.position.y));
        return coords;
    }
}
