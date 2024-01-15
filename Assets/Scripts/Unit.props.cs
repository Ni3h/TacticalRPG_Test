using UnityEngine;
using UnityEngine.InputSystem;

public partial class Unit {

    [Header("Dynamic Properties")]

    [Tooltip("The current direction this unit is walking.")]
    [SerializeField]
    private int Direction;

    [SerializeField]
    private InputActionReference MovementInput;

    [SerializeField]
    private Vector2Int CurrentCoordinates;


    [Header("Static Properties")]

    [SerializeField]
    private InputActionReference SelectInput;

    [Tooltip("The animator to use for rendering this unit.")]
    [SerializeField]
    private RuntimeAnimatorController AnimatonController;

    [Tooltip("How many spaces this unit can move per turn.")]
    [SerializeField]
    private int Speed;

    private void InitializeComponents() {
        Animator.runtimeAnimatorController = AnimatonController;
    }
}
