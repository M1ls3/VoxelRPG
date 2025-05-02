using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInput : MonoBehaviour
{
    public UnityEvent<Vector3> OnMovementInput, OnPointerInput;
    public UnityEvent OnAttack;

    //[SerializeField]
    //private InputActionReference movement, attack, pointerPosition;

    private void Update()
    {
        OnMovementInput?.Invoke(new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")));
        OnPointerInput?.Invoke(GetPointerInput());
        if (Input.GetMouseButtonDown(0))
            OnAttack?.Invoke();
    }

    private Vector3 GetPointerInput()
    {
        //Vector3 mousePos = pointerPosition.action.ReadValue<Vector3>();
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }
}
