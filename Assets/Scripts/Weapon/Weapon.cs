using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Weapon : NetworkBehaviour {
    [Header("Spawn options")]
    [SerializeField] public Vector3 positionOffset = Vector3.zero;
    [SerializeField] public Vector3 rotationOffset = Vector3.zero;

    // The weapon can have 1-2 firemodes (LMB and RMB).
    public FireMode[] fireModes = null;

    protected WeaponAnimator animator = null;

    // When the weapon is able to fire next
    protected float nextPossibleFire = 0;

    private void Start() {
        animator = GetComponent<WeaponAnimator>();
    }

    public virtual void Equip() {

    }

    public virtual void Unequip() {

    }

    /// <summary>
    /// Wrapper method for the shooting (Handles all the FX etc)
    /// </summary>
    /// <param name="fireMode">Firemode to be used</param>
    public virtual void Shoot(FireMode fireMode) {}

    /// <summary>
    /// Shooting logic, override this
    /// </summary>
    protected virtual void Fire(FireMode fireMode) {}

    // Client shoot FX
    protected virtual void PlayShootFX(FireMode fireMode) {
        //animator.PlayFireAnimation();
        SoundSystem.PlaySound2D(fireMode.fireSound2D.name);
    }

    // Server Shoot FX
    [Command]
    protected void CmdPlayShootFX(NetworkIdentity id, string soundName) {
        RpcPlayShootFX(id, soundName);
    }

    // FX That'll be played on other clients
    [ClientRpc]
    protected virtual void RpcPlayShootFX(NetworkIdentity id, string soundName) {
        if (id.netId == EntityManager.LocalPlayer.Network_Identity.netId) {
            return;
        }
        SoundSystem.PlaySound(soundName, id.transform.position);
    }


    [System.Serializable]
    public struct FireMode {
        // Customizable data
        [Tooltip("Should the weapon be autofired?")]
        public bool autoFire;
        [Tooltip("Firerate in RPM")]
        public float firerate;
        [Tooltip("How much damage the weapon does")]
        public float damage;
        [Tooltip("How many shots the weapon shoots per shot")]
        public int shotCount;
        [Tooltip("Hitscan weapon if projectiledata is null")]
        public ProjectileData projectileData;
        [Tooltip("Spread in degrees")]
        public float spread;
        [Tooltip("Recoil vertical and horizontal values")]
        public Vector2 recoilBounds;
        [Tooltip("Sound that is played when fired")]
        public AudioClip fireSound2D;
        public AudioClip fireSound3D;
        [Tooltip("The weapon's hitmask")]
        public LayerMask hitMask;
        [Tooltip("The weapon's bullet trail")]
        public BulletTrail bulletTrail;

        // Inner data
        [HideInInspector]
        public float lastFired;
    }

    /// <summary>
    /// Used only if the weapon shoots projectiles
    /// </summary>
    [System.Serializable]
    public struct ProjectileData {
        [Tooltip("Projectile to shoot")]
        public Projectile projectile;
        [Tooltip("Projectile speed")]
        public float speed;
        [Tooltip("How long the projectile exists")]
        public float lifeSpan;
    }
}
