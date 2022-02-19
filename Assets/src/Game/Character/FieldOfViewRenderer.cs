using System;
using System.Collections.Generic;
using UnityEngine;
using Curry.Util;

namespace Curry.Game
{ 
    public class FieldOfViewRenderer : MonoBehaviour
    {
        [SerializeField] int m_rayCount = default;
        [SerializeField] float m_fov = default;
        [SerializeField] float m_viewDistance = default;
        [SerializeField] MeshFilter m_meshFilter = default;
        Vector3[] m_verts;
        Vector2[] m_uv;
        int[] m_triangles;
        Mesh m_renderMesh;

        void Awake() 
        {
            m_renderMesh = new Mesh();
            m_meshFilter.mesh = m_renderMesh;
        }

        void Start()
        {
            CreateRenderMesh();
        }

        void CreateRenderMesh() 
        {
            Vector3 origin = Vector3.zero;
            float angleIncrease = m_fov / m_rayCount;
            float angle = 0f;
            m_verts = new Vector3[m_rayCount + 2];
            m_verts[0] = origin;
            m_uv = new Vector2[m_verts.Length];
            m_triangles = new int[m_rayCount * 3];
            // Calc verts for mesh
            for (int i = 0; i < m_verts.Length - 1; ++i) 
            {
                Vector3 newV = origin + VectorExtension.VectorFromDegree(angle) * m_viewDistance;
                m_verts[i + 1] = newV;
                angle -= angleIncrease;
            }
            // assign triangle order
            int v = 0;
            for(int i = 0; i < m_triangles.Length; i += 3) 
            {
                m_triangles[i] = 0;
                m_triangles[i + 1] = v + 1;
                m_triangles[i + 2] = v + 2;
                ++v;             
            }

            m_renderMesh.vertices = m_verts;
            m_renderMesh.uv = m_uv;
            m_renderMesh.triangles = m_triangles;
        }


    }
}
