using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] Transform weaponsParent;
    [SerializeField] int startingWeaponIndex = -1;

    [Header("Animation")]
    [SerializeField] RuntimeAnimatorController defaultAnimatorController;

    [Header("Inputs")]
    [SerializeField] InputActionAsset inputActionAsset;
    [Space(10)]
    [SerializeField] InputActionReference attack;
    [SerializeField] InputActionReference shot;
    [SerializeField] InputActionReference cycleWeapon;
    [SerializeField] InputActionReference[] selectWeaponActions;

    [Header("Debug")]
    [SerializeField] bool debugNextWeapon;
    [SerializeField] bool debugPrevWeapon;
    [SerializeField] bool debugAttack;


    WeaponBase[] weapons;
    Animator animator;

    #region UnityMessages
    private void OnValidate()
    {
        if (debugNextWeapon)
        {
            debugNextWeapon = false;
            SelectNextWeapon();
        }
        if (debugPrevWeapon)
        {
            debugPrevWeapon = false;
            SelectPreviousWeapon();
        }
        if (debugAttack)
        {
            debugAttack = false;
            PerformAttack();
        }
    }

    private void Awake()
    {
        weapons = weaponsParent.GetComponentsInChildren<WeaponBase>();
        animator = GetComponentInChildren<Animator>();
    }
    private void OnEnable()
    {
        inputActionAsset.Enable();

        attack.action.performed += OnAttack;
        cycleWeapon.action.performed += OnCycleWeapon;

        for (int i = 0; i < selectWeaponActions.Length; i++)
        {
            selectWeaponActions[i].action.performed += (ctx) => SelectWeapon(i);
        }
    }

    private void Start()
    {
        foreach (WeaponBase wb in weapons)
        {
            wb.gameObject.SetActive(false);
        }
        SelectWeapon(startingWeaponIndex);
        if(startingWeaponIndex == -1)
        {
            ResetAnimatorForUnarmedCase();
        }
    }
    private void OnDisable()
    {
        inputActionAsset.Disable();
    }
    #endregion
    const string animatorAttackParameter = "Attack";
    private void OnAttack(InputAction.CallbackContext ctx)
    {
        PerformAttack();
    }

    private void PerformAttack()
    {
        weapons[currentWeaponIndex].NotifyAttack();
        animator.SetTrigger(animatorAttackParameter);
    }

    private void OnCycleWeapon(InputAction.CallbackContext ctx)
    {
        Vector2 value = ctx.ReadValue<Vector2>();
        if (value.y > 0f)
        {
            SelectNextWeapon();
        }
        if (value.y < 0f)
        {
            SelectPreviousWeapon();
        }
    }
    private void SelectNextWeapon()
    {
        SelectWeapon(currentWeaponIndex + 1);
    }

    private void SelectPreviousWeapon()
    {
        SelectWeapon(currentWeaponIndex - 1);
    }

    int currentWeaponIndex = -1;

    private void SelectWeapon(int weaponIndex)
    {
        if(currentWeaponIndex != -1)
        {
            weapons[currentWeaponIndex].NotifyUnselected();
            weapons[currentWeaponIndex].gameObject.SetActive(false);

            ResetAnimatorForUnarmedCase();
        }
        if (weaponIndex < -1)
        {
            weaponIndex = weapons.Length - 1;
        }
        if (weaponIndex >= weapons.Length)
        {
            weaponIndex = - 1;
        }

        currentWeaponIndex = weaponIndex;

        if (currentWeaponIndex != -1)
        {
            weapons[currentWeaponIndex].NotifySelected();
            weapons[currentWeaponIndex].gameObject.SetActive(true);

            animator.runtimeAnimatorController = weapons[currentWeaponIndex].animatorOverrideController;
            animator.SetInteger("CurrentComboLength", weapons[currentWeaponIndex].comboLength);
        }
    }

    private void ResetAnimatorForUnarmedCase()
    {
        animator.runtimeAnimatorController = defaultAnimatorController;
        animator.SetInteger("CurrentComboLength", 0);
    }
}
