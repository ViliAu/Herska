using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEditor : MonoBehaviour {

    private static LevelEditor ed;
    public static LevelEditor Instance {
        get {
            if (ed == null) {
                ed = FindObjectOfType<LevelEditor>();
                if (ed == null) {
                    Debug.LogError("Couldn't find editor!");
                }
            }
            return ed;
        }
    }

    private LevelEditorInput edinp;
    public LevelEditorInput Input {
        get {
            if (edinp == null) {
                edinp = LevelEditor.Instance.GetComponent<LevelEditorInput>();
            }
            return edinp;
        }
    }

    private LevelEditorCamera edcam;
    public LevelEditorCamera Camera {
        get {
            if (edcam == null) {
                edcam = LevelEditor.Instance.GetComponent<LevelEditorCamera>();
            }
            return edcam;
        }
    }

}
