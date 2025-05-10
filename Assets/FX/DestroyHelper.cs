using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyHelper : MonoBehaviour
{
    [SerializeField]
    private GameObject referenceObject;
    [SerializeField] private float secounds = 1f;

    private void Start()
    {
        StartCoroutine(DestroyWithDelay());
    }

    private IEnumerator DestroyWithDelay()
    {
        yield return new WaitForSeconds(secounds);
        DestoryObject();
    }

    public void DestoryObject()
    {
        Destroy(referenceObject);
    }
}
