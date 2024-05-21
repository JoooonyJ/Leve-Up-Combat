using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    [SerializeField] public AnimatorOverrideController animatorOverrideController;
    [SerializeField] public int comboLength = 0;

    internal abstract void NotifyAttack();
    internal abstract void NotifySelected();
    internal abstract void NotifyUnselected();
}
