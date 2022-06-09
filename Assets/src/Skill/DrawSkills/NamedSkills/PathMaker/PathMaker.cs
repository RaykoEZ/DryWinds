using System;
using System.Collections.Generic;
using Curry.Game;
using UnityEngine;
using Curry.Events;
using Curry.Util;

namespace Curry.Skill
{
    public class PathMaker : BaseDrawSkill
    {
        [SerializeField] protected CurryGameEventSource m_onMakePath = default;
        [SerializeField] protected PrefabLoader m_pathAsset = default;
        public virtual ISkillObject<LineInput> Path { get; protected set; }
        protected PlayerPath m_latestPathRef;
        public override void Init(BaseCharacter user)
        {
            base.Init(user);
            m_pathAsset.OnLoadSuccess += (obj) =>
            {
                Path = obj.GetComponent<PlayerPath>();
            };
            m_pathAsset.LoadAsset();
        }
        protected virtual void MakePath(LineInput param)
        {
            if (param.Payload == null)
            {
                param.Payload = new Dictionary<string, object>();
            }
            m_latestPathRef = Instantiate(Path.go).GetComponent<PlayerPath>();
            m_latestPathRef?.Begin(param);
        }
        protected override void OnExecute(LineInput input)
        {
            MakePath(input);
        }
    }
}