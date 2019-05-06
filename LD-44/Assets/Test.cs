using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using helper;

public class Test : MonoBehaviour
{
    public float angle = 30;

    [Range(3, 15)] public int rayCount = 10;

    private void OnDrawGizmos()
    {
        Vector2[] arkVectors = HelperMath.GetArkVectors2D(transform.up, angle, rayCount);
        foreach (Vector2 vec in arkVectors)
        {
            Gizmos.DrawRay(transform.position, vec);
        }
    }
}
