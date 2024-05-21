using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventForwarder : MonoBehaviour
{
    PlayerCombat playerCombat;

    private void Awake()
    {
        playerCombat = GetComponentInParent<PlayerCombat>();
    }

    public void Attack(string hitColliderObjectName)
    {
        playerCombat.OnAnimationAttack(hitColliderObjectName);
    }
}
