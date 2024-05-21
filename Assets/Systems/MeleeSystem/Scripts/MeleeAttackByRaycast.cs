using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackByRaycast : MonoBehaviour
{
    [Header("Behaviour")]
    [SerializeField] float distanceBetweenRays = 0.05f;
    [SerializeField] LayerMask layerMask = Physics.DefaultRaycastLayers;

    [Header("Setup")]
    [SerializeField] Transform forwardPoint;
    [SerializeField] Transform backPoint;

    Vector3 oldForwardPoint;
    Vector3 oldBackPoint;

    bool isAttacking = false;

    private void Awake()
    {
        oldForwardPoint = forwardPoint.position;
        oldBackPoint = backPoint.position;
    }

    private void FixedUpdate()
    {
        FixedUpdateAttack();
        FixedUpdateRememberOldPositions();
    }

    private void FixedUpdateAttack()
    {
        if (isAttacking)
        {
            float forwardToOldForwardDistance = (forwardPoint.position - oldForwardPoint).magnitude;
            float backToOldBackDistance = (forwardPoint.position - oldBackPoint).magnitude;

            int numRays = Mathf.CeilToInt(Mathf.Max(forwardToOldForwardDistance, backToOldBackDistance) / distanceBetweenRays);

            for (int i = 0; i < numRays; i++)
            {
                float t = (float)i / (float)numRays;
                Vector3 startPosition = Vector3.Lerp(oldForwardPoint, forwardPoint.position, t);
                Vector3 endPosition = Vector3.Lerp(oldBackPoint, backPoint.position, t);

                if (Physics.Linecast(startPosition, endPosition, out RaycastHit hit, layerMask))
                {
                    hit.collider.GetComponent<HurtCollider>()?.NotifyHit(this);
                }
            }
        }
    }

    private void FixedUpdateRememberOldPositions()
    {
        oldForwardPoint = forwardPoint.position;
        oldBackPoint = backPoint.position;
    }

    internal void StartAttack() { isAttacking = true; }
    internal void StopAttack() { isAttacking = false; }
}
