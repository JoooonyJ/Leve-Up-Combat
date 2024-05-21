using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HurtCollider : MonoBehaviour
{
    [SerializeField] UnityEvent onHit;
    internal void NotifyHit(HitCollider hitCollider)
    {
        onHit.Invoke();
    }

    internal void NotifyHit(MeleeAttackByRaycast meleeAttackByRaycast)
    {
        onHit.Invoke();
    }
}
