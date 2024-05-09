using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        CheckColider(other);
    }

    private void OnCollisionEnter(Collision collision)
    {
        CheckColider(collision.collider);
    }

    private void CheckColider(Collider other)
    {
        other.GetComponent<HurtCollider>()?.NotifyHit(this);
    }
}
