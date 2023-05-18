using Curry.Explore;
using System.Collections.Generic;
using System.Linq;

namespace Curry.Util
{
    public static class SamplingUtil 
    {
        public static T SampleWithWeights<T>(List<T> samplePool) where T : IWeightedItem
        {
            T ret = default;
            if (samplePool.Count == 0)
            {
                return ret;
            }

            // get total weight
            int weightSum = 0;
            foreach (var item in samplePool)
            {
                weightSum += item.Weight;
            }
            // get random sample within sum range
            int rand = UnityEngine.Random.Range(0, weightSum);
            int step = 0;
            // iterate through all weights and compare sample with weight step sum
            foreach (var item in samplePool)
            {
                step += item.Weight;
                if (rand < step)
                {
                    return item;
                }
            }
            return samplePool.First();
        }
        public static List<T> SampleFromList<T>(List<T> pool, int numToSample, bool uniqueResults = true)
        {
            // Redirect to random index selection
            if (!uniqueResults) return RandomSelectFromList(pool, numToSample);

            List<T> ret = new List<T>();
            if (numToSample < 1) { return ret; }
            //Sample probability variables
            float p;
            float sample;
            int numToGet = numToSample;
            int numLeftInRange = pool.Count;
            foreach (T v in pool)
            {
                p = numToGet / numLeftInRange;
                sample = UnityEngine.Random.Range(0f, 1f);
                // Sample probability to pick this position for spawning
                if (sample < p)
                {
                    ret.Add(v);
                    --numToGet;
                }
                --numLeftInRange;
                // finish early if we got enough this iteration
                if (numToGet == 0)
                {
                    return ret;
                }
            }
            return ret;
        }
        static List<T> RandomSelectFromList<T>(List<T> pool, int numToSample)
        {
            List<T> ret = new List<T>();
            int n = pool.Count;
            if (numToSample < 1 || n < 1)
            {
                return ret;
            }
            List<int> getAt = new List<int>(numToSample);
            for (int i = 0; i < numToSample; ++i)
            {
                getAt[i] = UnityEngine.Random.Range(0, n);
            }
            foreach (int i in getAt)
            {
                ret.Add(pool[i]);
            }
            return ret;
        }

    }
}
