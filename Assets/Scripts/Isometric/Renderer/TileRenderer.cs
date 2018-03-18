using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using iso;

namespace iso.Renderer
{
    public class TileRenderer : MonoBehaviour
    {
        [SerializeField]
        MeshFilter m_MeshFilter = null;

        Mesh m_Mesh = null;

        public Vector2 grid=Vector2.zero;

        // Use this for initialization
        void Start()
        {
            if (m_MeshFilter == null)
            {
                m_MeshFilter = gameObject.AddComponent<MeshFilter>();
            }

            CreateMesh();
        }
        
        void CreateMesh()
        {
            m_Mesh = new Mesh();


            Vector3[] vertices = new Vector3[4];
            Vector2[] uv = new Vector2[4] { new Vector2( 0,0), new Vector2( 1,0), new Vector2(1, 1), new Vector2( 0, 1 ) };

            Vector2 p = iso.IsoStaticCoordinateFormulae.gameToView2F(grid.x, grid.y);
            Vector3 pos = new Vector3(p.x, p.y, 0);

            vertices[0] = pos + new Vector3(-0.32f,0, 0);
            vertices[1] = pos + new Vector3(0, 0.16f, 0);
            vertices[2] = pos + new Vector3(0.32f, 0,0);
            vertices[3] = pos + new Vector3(0,-0.16f,0);

            int[] triangles = {0,1,3,1,2,3 };

            m_Mesh.vertices = vertices;
            m_Mesh.triangles = triangles;
            m_Mesh.uv = uv;

            m_MeshFilter.mesh = m_Mesh;
        }
    }
}

