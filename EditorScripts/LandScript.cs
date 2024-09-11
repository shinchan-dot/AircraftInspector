#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LandScript : MonoBehaviour
{
    public GameObject Prefab;
    public Transform Parent;

    [ContextMenu("PUT_PLANES")]
    public void PutPlanes()
    {
        int count = 1;
        for (int z = -19000; z <= 19000; z += 2000)
        {
            for (int x = -19000; x <= 19000; x += 2000)
            {
                GameObject obj = PrefabUtility.InstantiatePrefab(Prefab) as GameObject;

                obj.name = "Plane (" + count + ")";
                count++;

                Transform tran = obj.GetComponent<Transform>();
                tran.SetParent(Parent);
                tran.localPosition = new Vector3(x, 0, z);
            }
        }
    }
}
#endif
