using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public static class Extensions {
    public const float TAU = Mathf.PI * 2;
    
    public static void Fade(this SpriteRenderer renderer, float alpha) {
        var color = renderer.color;
        color.a = alpha;
        renderer.color = color;
    }
	
    public static T Rand<T>(this IList<T> list) {
        return list[Random.Range(0, list.Count)];
    }
	
    public static void DestroyChildren(this Transform t) {
        foreach (Transform child in t) Object.Destroy(child.gameObject);
    }
	
    public static void SetLayersRecursively(this GameObject gameObject, int layer) {
        gameObject.layer = layer;
        foreach (Transform t in gameObject.transform) t.gameObject.SetLayersRecursively(layer);
    }

    public static IEnumerator WaitFor<T>(float time, Action<T> fun, T i) {
        yield return new WaitForSeconds(time);
        fun.Invoke(i);
    }
    
    public static float CircularClamp(float value) {
        var flatValue = value >= 0 ? value : TAU - Mathf.Abs(value % TAU); 
        return flatValue % TAU;
    }
    
    public static float ShortestArc(float target, float current) {
        var diff = (target - current + Mathf.PI) % TAU - Mathf.PI;
        return diff < -Mathf.PI ? diff + TAU : diff;
    }
    
    public static Vector2 ToV2(this Vector3 vec) => new Vector2(vec.x, vec.y);
	
    public static Vector3 Flat(this Vector3 vec) => new Vector3(vec.x, 0, vec.z);
	
    public static Vector3 ToV3Int(this Vector3 vec) => new Vector3((int)vec.x, (int)vec.y, (int)vec.z);
}