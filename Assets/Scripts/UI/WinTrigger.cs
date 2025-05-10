using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WinTrigger : MonoBehaviour
{
    public static UnityEvent OnPlayerWin = new UnityEvent();

    [SerializeField] private string playerTag = "Player";
    [SerializeField] private BoxCollider collider;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            OnPlayerWin?.Invoke();
            //Destroy(gameObject);
        }
    }
}
