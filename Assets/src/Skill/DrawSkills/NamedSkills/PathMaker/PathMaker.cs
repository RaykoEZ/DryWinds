using System;
using System.Collections.Generic;
using Curry.Game;
using UnityEngine;
using UnityEngine.InputSystem;
using Curry.Events;
using Curry.Util;
using Curry.Explore;

namespace Curry.Skill
{
    public class PathMaker : BaseDrawSkill
    {
        [SerializeField] protected CurryGameEventSource m_onMakePath = default;
        [SerializeField] protected PrefabLoader m_pathAsset = default;
        public virtual ISkillObject<LineInput> PathObject { get; protected set; }
        public ExplorePath CurrentPath { get; protected set; }
        protected PlayerPath m_latestPathRef;

        public override void Init(BaseCharacter user)
        {
            base.Init(user);
            m_pathAsset.OnLoadSuccess += (obj) =>
            {
                PathObject = obj.GetComponent<PlayerPath>();
            };
            m_pathAsset.LoadAsset();
        }
        protected virtual void MakePath(LineInput param)
        {
            if (param.Payload == null)
            {
                param.Payload = new Dictionary<string, object>();
            }
            m_latestPathRef = Instantiate(PathObject.go, transform).GetComponent<PlayerPath>();
            m_latestPathRef.transform.position = Vector3.zero;
            m_latestPathRef?.Begin(param);
        }
        protected override void OnExecute(LineInput input)
        {
            MakePath(input);
            Queue<Vector2> path = new Queue<Vector2>(input.Vertices);
            CurrentPath = new ExplorePath(path);
        }
    }
}