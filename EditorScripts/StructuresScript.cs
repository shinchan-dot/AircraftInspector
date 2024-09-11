#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class StructuresScript : MonoBehaviour
{
    public GameObject Prefab;
    public Transform Parent;
    public AnimationCurve AnimationCurveY;

    [ContextMenu("PUT_CUBES")]
    public void PutCubes()
    {
        for (int i = 0; i < 600; i++)
        {
            GameObject obj = PrefabUtility.InstantiatePrefab(Prefab) as GameObject;
            obj.name = "Cube (" + i + ")";

            Transform tran = obj.GetComponent<Transform>();
            tran.SetParent(Parent);
            tran.localPosition = new Vector3((Random.value * 36000) - 18000, (AnimationCurveY.Evaluate(Random.value) * 1000) + 50, (Random.value * 36000) - 18000);
            tran.localRotation = Quaternion.Euler(0, Random.value * 360, 0);
        }
    }
}
#endif
