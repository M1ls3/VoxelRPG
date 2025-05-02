using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDetector : Detector
{
    [SerializeField]
    private float targetDetectionRange = 5;

    [SerializeField]
    private LayerMask obstaclesLayerMask, playerLayerMask;

    [SerializeField]
    private bool showGizmos = false;

    //gizmo parameters
    private List<Transform> colliders;

    public override void Detect(AIData aiData)
    {
        // Find out if player is near using sphere cast
        Collider[] playerColliders =
            Physics.OverlapSphere(transform.position, targetDetectionRange, playerLayerMask);

        if (playerColliders.Length > 0)
        {
            // Check if we see the player
            Vector3 direction = (playerColliders[0].transform.position - transform.position).normalized;
            RaycastHit hit;
            bool hasHit = Physics.Raycast(
                transform.position,
                direction,
                out hit,
                targetDetectionRange,
                obstaclesLayerMask | playerLayerMask);

            // Verify that hit object is player
            if (hasHit && (playerLayerMask & (1 << hit.collider.gameObject.layer)) != 0)
            {
                Debug.DrawRay(transform.position, direction * targetDetectionRange, Color.magenta);
                colliders = new List<Transform>() { playerColliders[0].transform };
            }
            else
            {
                colliders = null;
            }
        }
        else
        {
            colliders = null;
        }
        aiData.targets = colliders;
    }

    private void OnDrawGizmosSelected()
    {
        if (showGizmos)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, targetDetectionRange);
        }
    }
}
