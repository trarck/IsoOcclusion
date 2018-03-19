using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using iso;

namespace iso.Renderer
{
    public class IsoTile : MonoBehaviour
    {
        [SerializeField]
        MeshFilter m_MeshFilter = null;

        Mesh m_Mesh = null;

        public Vector2 origin = Vector2.zero;

        public Vector2 size = Vector2.one;

        // Use this for initialization
        void Start()
        {
            if (m_MeshFilter == null)
            {
                m_MeshFilter = gameObject.AddComponent<MeshFilter>();
            }

            CreateMesh();
        }


        /// <summary>
        /// 
        ///                               2
        ///                     
        ///                 1                           3
        ///                               0
        /// 
        /// </summary>
        void CreateMesh()
        {
            m_Mesh = new Mesh();


            Vector3[] vertices = new Vector3[4];
            Vector2[] uv = new Vector2[4] { new Vector2(0, 1), new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1) };

            Vector2 start = iso.IsoStaticCoordinateFormulae.gameToView2F(origin.x, origin.y);
            Vector2 end = iso.IsoStaticCoordinateFormulae.gameToViewPoint(origin + size);

            Vector2 pos = iso.IsoStaticCoordinateFormulae.gameToView2F(origin.x, origin.y);

            vertices[0] = new Vector3(pos.x, pos.y,0);
            pos = iso.IsoStaticCoordinateFormulae.gameToView2F(origin.x, origin.y + size.y);
           
            vertices[1] = new Vector3(pos.x, pos.y,0);
            pos = iso.IsoStaticCoordinateFormulae.gameToView2F(origin.x + size.x, origin.y+size.y);
            vertices[2] = new Vector3(pos.x, pos.y,0);
            pos = iso.IsoStaticCoordinateFormulae.gameToView2F(origin.x + size.x, origin.y);
            vertices[3] = new Vector3(pos.x, pos.y,0);

            int[] triangles = {0,1, 2,0, 2, 3 };

            m_Mesh.vertices = vertices;
            m_Mesh.triangles = triangles;
            m_Mesh.uv = uv;

            m_MeshFilter.mesh = m_Mesh;
        }
    }
}

