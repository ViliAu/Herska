using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildTool : Weapon {
    [SerializeField] private GameObject[] buildables = null;
    private int equippedBuildableIndex = 0;

    public GameObject CurrentBuildable {
        get {
            return equippedBuildableIndex < 0 || equippedBuildableIndex > buildables.Length-1 ? null : buildables[equippedBuildableIndex];
        }
    }

    private void Update() {
        CastRay();
    }

    private void CastRay() {
        RaycastHit hit;
        if (Physics.Raycast(EntityManager.LocalPlayer.Player_Camera.head.position, EntityManager.LocalPlayer.Player_Camera.head.forward, out hit, 200, 0, QueryTriggerInteraction.Ignore)) {
            //ghost.transform.position = hit.point;
        }
    }

}
