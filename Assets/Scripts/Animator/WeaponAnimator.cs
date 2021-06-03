using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimator : MonoBehaviour {
    
    [SerializeField] private int weaponId = 0;

    private Animator animator;
    private Weapon weapon;

    private void Start() {
        animator = transform.GetComponent<Animator>();
        weapon = transform.GetComponent<Weapon>();
        SetAnimationId();
    }

    public void SetAnimationId() {
        if (animator != null) {
            animator.SetFloat("weaponId", weaponId);
        }
    }

    public void PlayEquipAnimation() {
        if (animator == null) {
            return;
        }
        animator.SetTrigger("Equip");
    }

    public void PlayFireAnimation() {
        if (animator == null) {
            return;
        }
        animator.SetTrigger("Fire");
    }

    public void PlayAltFireAnimation() {
        if (animator == null) {
            return;
        }
        animator.SetTrigger("AltFire");
    }

    public void PlayReloadAnimation() {
        if (animator == null) {
            return;
        }
        animator.SetTrigger("Reload");
    }

}
