using Curry.Game;
using System;
using System.Collections;
using UnityEngine;

namespace Curry.Skill
{
    public class MinorShield : EdgeSkill
    {
        protected override IEnumerator SkillEffect(IActionInput target)
        {
            yield return new WaitForSeconds(1f);
            Debug.Log("Shield");
        }
    }
}
