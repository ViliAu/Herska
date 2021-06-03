using UnityEngine;

public static class LevelEditorItemDatabase {
    private const string prefix = "Prefabs/LevelEditor";

    public static GameObject[] LoadWalls() {
        return Resources.LoadAll<GameObject>(prefix+"/walls");
    }

    public static GameObject[] LoadFloors() {
        return Resources.LoadAll<GameObject>(prefix+"/floors");
    }

}
