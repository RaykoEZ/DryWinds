using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Skill
{
    public static class SkillUtil
    {
        public static IEnumerator IFrame(GameObject obj, float duration) 
        {
            int iframeLayer = LayerMask.NameToLayer("IFrame");
            Physics.IgnoreLayerCollision(0, iframeLayer, true);
            obj.layer = iframeLayer;
            yield return new WaitForSeconds(duration);
            obj.layer = 0;
        }
    }
}
