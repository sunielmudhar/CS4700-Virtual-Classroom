using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draw : MonoBehaviour
{

    public void Drawing(bool state)
    {
        if (state)
        {
            Mesh line = new Mesh();

            Vector3[] verticies = new Vector3[4];
            Vector2[] uv = new Vector2[4];
            int[] triangles = new int[4];

            verticies[0] = new Vector3(-1, +1);
            verticies[1] = new Vector3(-1, -1);
            verticies[2] = new Vector3(+1, -1);
            verticies[3] = new Vector3(+1, +1);

            uv[0] = Vector2.zero;
            uv[1] = Vector2.zero;
            uv[2] = Vector2.zero;
            uv[3] = Vector2.zero;

            triangles[0] = 0;
            triangles[1] = 3;
            triangles[2] = 1;
            triangles[3] = 1;
            triangles[4] = 3;
            triangles[5] = 2;

            line.vertices = verticies;
            line.uv = uv;
            line.triangles = triangles;
            line.MarkDynamic();

            GetComponent<MeshFilter>().mesh = line;
        }
    }

    private void Update()
    {
        transform.position = MouseController.GetMousePosition();
    }
}
