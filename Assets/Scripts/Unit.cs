using System.Collections;
using UnityEngine;
using Context = UnityEngine.InputSystem.InputAction.CallbackContext;

public partial class Unit : BaseBehavior {

    private Vector2Int lastCoordinates;
    private Vector2Int nextCoordinates;

    protected override void OnStart() {
        InitializeComponents();
        MovementInput.action.performed += OnDestinationSelected;
    }

    protected override void OnUpdate(float delta) {
        if (lastCoordinates != nextCoordinates) {
            var newPos = GridManager.Instance.FindWorldPosition(nextCoordinates);
            transform.position = new Vector3(newPos.x + 1, -newPos.y, -1);
            lastCoordinates = nextCoordinates;
        }
    }

    private void OnDestinationSelected(Context context) {
        var gridManager = GridManager.Instance;
        var startCoords = GetStartCoordiantes();
        var newPos = CameraMovement.TranslatedMousePosition;
        var destination = gridManager.FindCoordinates(newPos);
        var destCoords = new Vector2Int(1 + destination.x, destination.y);
        var path = GridManager.Instance.FindPath(startCoords, destCoords);
        var str = "";
        foreach (var step in path) {
            str += step.ToString() + ", ";
        }
        print(str);
        StartCoroutine(MovementCoroutine(path));
    }

    private Vector2Int GetStartCoordiantes() {
        var gridManager = GridManager.Instance;
        var coords = gridManager.FindCoordinates(new Vector3(1 + transform.position.x, -transform.position.y));
        return coords;
    }

    private IEnumerator MovementCoroutine(Vector2Int[] travelPath) {
        for (var i = 0; i < travelPath.Length; i++) {
            nextCoordinates = travelPath[i];
            yield return new WaitForSeconds(0.2f);
        }
    }
}
