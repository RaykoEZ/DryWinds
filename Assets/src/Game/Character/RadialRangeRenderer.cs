using System;
using System.Collections.Generic;
using UnityEngine;
using Curry.Util;

namespace Curry.Game
{ 
    public class RadialRangeRenderer : MonoBehaviour
    {
        [Range(1, 360)]
        [SerializeField] int m_rayCount = default;
        [Range(0, 360f)]
        [SerializeField] float m_fov = default;
        [SerializeField] float m_viewRadius = default;
        [SerializeField] MeshFilter m_meshFilter = default;
        [SerializeField] PolygonCollider2D m_collider = default;
        [SerializeField] LayerMask m_raycastMask = default;
        List<Vector3> m_verts;
        List<int> m_triangles;
        Mesh m_renderMesh;
        float m_angleInterval;
        float m_faceAngle = 0f;
        public float Radius { get { return m_viewRadius; } set { m_viewRadius = value; } }
        public float FieldOfViewDegree { get { return m_fov; } set { m_fov = value; } }

        public void FaceTowards(Vector2 dir) 
        {
            m_faceAngle = VectorExtension.DegreeFromDirection(dir);
        }

        void Awake() 
        {
            //Setup mesh and array sizes
            m_renderMesh = new Mesh();
            m_renderMesh.name = "RangeMesh";
            m_meshFilter.mesh = m_renderMesh;
            m_angleInterval = m_fov / m_rayCount;
            m_verts = new List<Vector3>(new Vector3[m_rayCount + 2]);
            m_verts[0] = Vector3.zero;
            m_triangles = new List<int>(new int[m_rayCount * 3]);
        }

        void LateUpdate()
        {
            RenderMesh();
        }

        void RenderMesh() 
        {
            if(Mathf.Approximately(m_viewRadius, 0f)) 
            {
                return;
            }

            SetVisibleExtent();
            SetTriangleIndex();

            m_renderMesh.Clear();
            m_renderMesh.vertices = m_verts.ToArray();
            m_renderMesh.triangles = m_triangles.ToArray();
            m_renderMesh.RecalculateNormals();

            Vector2[] points = VectorExtension.ToVector2Array(m_verts.ToArray());
            m_collider.points = points;
        }

        void SetVisibleExtent() 
        {
            float angle = m_faceAngle;
            // Set visible extent for mesh with raycasts
            for (int i = 0; i < m_verts.Count - 1; ++i)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, VectorExtension.VectorFromDegree(angle), m_viewRadius, m_raycastMask);                   
                // Check if there are obstacles
                if (hit.collider != null)
                {
                    m_verts[i + 1] = transform.InverseTransformPoint(hit.point);
                }
                else
                {
                    Vector3 localPoint = VectorExtension.VectorFromDegree(angle) * m_viewRadius;
                    m_verts[i + 1] = localPoint;

                }
                angle -= m_angleInterval;
            }
        }

        void SetTriangleIndex() 
        {
            // assign triangle order
            int v = 0;
            for (int i = 0; i < m_triangles.Count; i += 3)
            {
                m_triangles[i] = 0;
                m_triangles[i + 1] = v + 1;
                m_triangles[i + 2] = v + 2;
                ++v;
            }
        }
    }
}
