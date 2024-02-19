using UnityEngine;

public class Utils : MonoBehaviour {
    public static Material[] GetAllMaterials(GameObject go) {
        Renderer[] rends = go.GetComponentsInChildren<Renderer>();
        Material[] mats = new Material[rends.Length];
        for(var i = 0; i < rends.Length; i++) {
            mats[i] = rends[i].material;
        }
        return mats;
    }
}
