using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Game
{
    // Renders 2d sonar effect on one renderer
    // Code learnt and adapted from SimpleSonarShader written by Drew Okenfuss.
    public class SonarRenderer : MonoBehaviour
    {
        [SerializeField] Renderer m_render = default;
        // The number of rings that can be rendered at once.
        // Must be the same value as the array size in the shader.
        static int maxWaveCount = 20;
        Queue<Vector4> positionsQueue = new Queue<Vector4>(maxWaveCount);
        // Queue of intensity values for each ring.
        // These are kept in the same order as the positionsQueue.
        Queue<float> intensityQueue = new Queue<float>(maxWaveCount);
        public delegate void OnSonarFinish();
        public event OnSonarFinish OnFinish;
        void Start()
        {
            StartSonar(transform.position, 1f, 20, 3f);
        }

        // Start the sonar effect to search for objects
        public void StartSonar(Vector3 worldPos, float intensity, int numRepeat = 0, float repeatPerSecond = 0)
        {
            Vector4 pos = worldPos;
            StartCoroutine(SonarWave(pos, intensity, numRepeat, repeatPerSecond));
        }

        IEnumerator SonarWave(Vector3 worldPos, float intensity, int numRepeat, float repeatPerSecond)
        {
            for (int i = 0; i <= numRepeat; i++)
            {
                StartSonarRing(worldPos, intensity);
                yield return new WaitForSeconds(repeatPerSecond);
            }
            OnFinish?.Invoke();
        }

        void StartSonarRing(Vector4 position, float intensity)
        {
            if (positionsQueue.Count > 0)
            {
                positionsQueue.Dequeue();
            }
            if (intensityQueue.Count > 0)
            {
                intensityQueue.Dequeue();
            }
            // Put values into the queue
            position.w = Time.timeSinceLevelLoad;
            positionsQueue.Enqueue(position);
            intensityQueue.Enqueue(intensity);
            // Send updated queues to the shaders
            m_render.material.SetVectorArray("_hitPts", positionsQueue.ToArray());
            m_render.material.SetFloatArray("_Intensity", intensityQueue.ToArray());
        }
    }
}
