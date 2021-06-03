using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class WeaponHitScan : Weapon {

    [Header("FX")]
    [SerializeField] public ParticleSystem ps = null;

    public override void Shoot(FireMode fireMode) {
        // Firerate check
        if (nextPossibleFire > Time.time) {
            return;
        }
        // Retard next fire by firerate
        nextPossibleFire = Time.time + (60 / fireMode.firerate);
        
        // Fire
        Fire(fireMode);
    }

    protected override void Fire(FireMode fireMode) {
        for (int i = 0; i < fireMode.shotCount; i++) {
            CastRay(fireMode);

            // FX Stuff
            PlayTrailFX();
            CmdPlayTrailFX(EntityManager.LocalPlayer.Network_Identity);
        }
        // Sounds, anim
        PlayShootFX(fireMode);
        CmdPlayShootFX(EntityManager.LocalPlayer.Network_Identity, fireMode.fireSound3D.name);
    }

    private void CastRay(FireMode fireMode) {
        RaycastHit hit;
        if (Physics.Raycast(EntityManager.LocalPlayer.Player_Camera.head.position, EntityManager.LocalPlayer.Player_Camera.head.forward, out hit, 200, fireMode.hitMask, QueryTriggerInteraction.Ignore)) {
            if (hit.transform.GetComponent<Health>() != null) {
                CmdDamageEnemy(fireMode.damage, hit.transform.GetComponent<NetworkIdentity>());
            }
        }
    }

    [Command]
    private void CmdDamageEnemy(float damage, NetworkIdentity netID) {
        netID.GetComponent<Health>().TakeDamage(damage);
    }

    private void PlayTrailFX() {
        ps.Play();
    }

    [Command]
    private void CmdPlayTrailFX(NetworkIdentity id) {
        RpcPlayTrailFX(id);
    }

    // Trail FX That'll be played on other clients
    [ClientRpc]
    protected virtual void RpcPlayTrailFX(NetworkIdentity id) {
        if (id.netId == EntityManager.LocalPlayer.Network_Identity.netId) {
            return;
        }
        Weapon w = id.GetComponent<PlayerWeapon>().EquippedWeapon;
        WeaponHitScan ws = w as WeaponHitScan;
        if (ws != null) {
            ws.ps.Play();
            print("asd");
        }
        print("tou");
    }
}
