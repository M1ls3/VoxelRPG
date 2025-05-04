using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponParent : MonoBehaviour
{
    public MeshRenderer characterRenderer, weaponRenderer;
    public Vector3 PointerPosition { get; set; }

    public Animator animator;
    public float delay = 0.3f;
    private bool attackBlocked;

    public bool IsAttacking { get; private set; }

    public Transform sphereOrigin;
    public float radius;

    public void ResetIsAttacking()
    {
        IsAttacking = false;
    }

    private void Update()
    {
        if (IsAttacking)
            return;
        //Vector3 direction = (PointerPosition - (Vector3)transform.position).normalized;
        //transform.right = direction;

        //var rotation = transform.localRotation;
        //Vector3 scale = transform.localScale;
        //if (direction.x < 0)
        //{
        //    scale.z = -1;
        //}
        //else if (direction.x > 0)
        //{
        //    scale.z = 1;
        //}
        //transform.localScale = scale;

        if (transform.eulerAngles.y > 0 && transform.eulerAngles.y < 180)
        {
            weaponRenderer.sortingOrder = characterRenderer.sortingOrder - 1;
        }
        else
        {
            weaponRenderer.sortingOrder = characterRenderer.sortingOrder + 1;
        }
    }

    public void Attack()
    {
        if (attackBlocked)
            return;
        animator.SetTrigger("Attack");
        IsAttacking = true;
        attackBlocked = true;
        StartCoroutine(DelayAttack());
    }

    private IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(delay);
        attackBlocked = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Vector3 position = sphereOrigin == null ? Vector3.zero : sphereOrigin.position;
        Gizmos.DrawWireSphere(position, radius);
    }

    public void DetectColliders()
    {
        foreach (Collider collider in Physics.OverlapSphere(sphereOrigin.position, radius))
        {
            if (collider.isTrigger == false)
                continue;
            Debug.Log(collider.name);
            Health health;
            if (health = collider.GetComponent<Health>())
            {
                health.GetHit(1, transform.parent.gameObject);
            }
        }
    }
}
