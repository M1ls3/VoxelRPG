using Cinemachine;
using DG.Tweening.Core.Easing;
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
        if (PauseScript.IsPaused) return;

        OnMovementInput?.Invoke(new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")));
        OnPointerInput?.Invoke(GetPointerInput());
        RotateTowardsPointer(GetLocalPointerInput());
        if (Input.GetMouseButtonDown(0))
            OnAttack?.Invoke();
    }

    private Vector3 GetPointerInput() //Cursor position GetPointerInput
    {
        //Vector3 mousePos = pointerPosition.action.ReadValue<Vector3>();
        //string log;
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }

    private Vector3 GetLocalPointerInput()
    {
        if (PauseScript.IsPaused) return transform.position + transform.forward;

        Vector3 playerPos = transform.position;
        Camera mainCamera = Camera.main;
        Vector3 cameraPos = mainCamera.transform.position;

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        Plane playerPlane = new Plane(Vector3.up, new Vector3(0, playerPos.y, 0));

        if (playerPlane.Raycast(ray, out float enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter);

            hitPoint.y = playerPos.y;

            return hitPoint;
        }

        return playerPos + mainCamera.transform.forward * 10f;
    }

    private void RotateTowardsPointer(Vector3 targetPosition)
    {
        if (PauseScript.IsPaused) return;

        Vector3 direction = targetPosition - transform.position;
        direction.y = 0f;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            Time.deltaTime * 10f // �������� ��������
        );
        }
    }
}
