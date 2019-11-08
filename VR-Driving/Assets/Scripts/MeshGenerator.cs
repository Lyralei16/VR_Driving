using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{
    Mesh mesh;
    Vector3[] verticles; // to generate a grid: verticles are used as points to set off tghe grid

    MeshRenderer myMeshRenderer;

    Material myMat;


    int[] triangles; 
    public int xSize = 20; // xSize and zSize set the position and size of the generated mesh
    public int zSize = 20;
    
    void Start()
    {
        mesh = new Mesh(); 
        GetComponent<MeshFilter>().mesh = mesh;

        myMeshRenderer = GetComponent<MeshRenderer>();
        myMat = myMeshRenderer.materials[0];

        CreateShape(); // this function generates the grid
        UpdateMesh(); // this function 
        
    }
    void CreateShape(){
        verticles = new Vector3[(xSize + 1) * (zSize +1)];
         for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float y = Mathf.PerlinNoise(x * .3f, z * .3f) * 2f;
                verticles[i] = new Vector3(x, y, z);
                i++;
            }
            
        }

        triangles = new int[xSize * zSize * 6];
        int vert = 0;
        int tris = 0;
        for (int z = 0; z <zSize; z++)
        {

        for (int x = 0; x < xSize; x++)
        {
            
            triangles[tris + 0] = vert + 0;
            triangles[tris + 1] = vert + xSize + 1;
            triangles[tris + 2] = vert + 1;
            triangles[tris + 3] = vert + 1;
            triangles[tris + 4] = vert + xSize + 1;
            triangles[tris + 5] = vert + xSize + 2;

            vert++;
            tris += 6;
           
        }
        vert++;
        }


        
        
    }

    void UpdateMesh(){
        mesh.Clear();

        mesh.vertices = verticles;
        mesh.triangles = triangles;

        Vector2[] uvs = new Vector2[mesh.vertices.Length];

        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2(mesh.vertices[i].x, mesh.vertices[i].z);
        }
        mesh.uv = uvs;

        mesh.RecalculateNormals();

        

    }
    /* private void OnDrawGizmos(){
        if (verticles == null)
        return;
        for (int i = 0; i < verticles.Length; i++)
        {
            Gizmos.DrawSphere(verticles[i], 0.1f);
        }
    } */
}
