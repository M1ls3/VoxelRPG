using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using Unity.VisualScripting;

public class Item : MonoBehaviour
{
    [SerializeField]
    private GameObject itemRenderer; // Changed from SpriteRenderer to MeshRenderer
    //[SerializeField]
    //private SpriteRenderer spriteRenderer;
    [SerializeField]
    private BoxCollider itemCollider;

    //[SerializeField]
    //int health = 3;
    //[SerializeField]
    //bool nonDestructible;

    //[SerializeField]
    //private GameObject hitFeedback, destoyFeedback;

    private GameObject itemModel; // Reference to instantiated 3D model


    public UnityEvent OnGetHit { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public void Initialize(ItemData itemData)
    {
        // Instantiate 3D model
        itemModel = Instantiate(itemData.gameObject, transform);
        itemModel.transform.localPosition = new Vector3(0.5f * itemData.size.x, 0.5f* itemData.size.y, 0.5f * itemData.size.z);

        //set sprite
        itemRenderer = itemData.gameObject;

        //set sprite offset
        itemRenderer.transform.localPosition = new Vector3(0.5f * itemData.size.x, 0.5f * itemData.size.y, 0.5f * itemData.size.z);
        //itemCollider.size = itemData.size;
        //itemCollider.center = spriteRenderer.transform.localPosition;

        // Set collider properties
        itemCollider.size = itemData.size;
        itemCollider.center = new Vector3(
            0.5f * itemData.size.x,
            0.5f * itemData.size.y,
            0.5f * itemData.size.z
        );

        //if (itemData.nonDestructible)
        //    nonDestructible = true;

        //this.health = itemData.health;

    }

    //public void GetHit(int damage, GameObject damageDealer)
    //{
    //    if (nonDestructible)
    //        return;
    //    if (health > 1)
    //        Instantiate(hitFeedback, itemRenderer.transform.position, Quaternion.identity);
    //    else
    //        Instantiate(destoyFeedback, itemRenderer.transform.position, Quaternion.identity);
    //    itemRenderer.transform.DOShakePosition(0.2f, 0.3f, 75, 1, false, true).OnComplete(ReduceHealth);
    //}

    //private void ReduceHealth()
    //{
    //    health--;
    //    if (health <= 0)
    //    {
    //        itemRenderer.transform.DOComplete();
    //        Destroy(gameObject);
    //    }

    //}
}
