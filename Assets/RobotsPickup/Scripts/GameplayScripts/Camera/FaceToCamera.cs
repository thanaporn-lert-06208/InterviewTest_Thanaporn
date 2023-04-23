using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceToCamera : MonoBehaviour
{
    public static List<Transform> faceToCameraTfs = new List<Transform>();
    private void Start()
    {
        faceToCameraTfs.Add(transform);
    }

    private void OnDestroy()
    {
        faceToCameraTfs.Remove(transform);
    }
}
