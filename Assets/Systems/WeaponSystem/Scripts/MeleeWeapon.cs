using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : WeaponBase
{
    [SerializeField] float attackDuration = 0.25f;

    MeleeAttackByRaycast meleeAttackByRaycast;

    public void StartAttack()
    {
        meleeAttackByRaycast.StartAttack();
        DOVirtual.DelayedCall(attackDuration,() => meleeAttackByRaycast.StopAttack(), false);
    }

    internal override void NotifyAttack()
    {
    }

    internal override void NotifySelected()
    {
    }

    internal override void NotifyUnselected()
    {
    }
}
