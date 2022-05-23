using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Curry.Game;

namespace Curry.Ai
{
    public class BaseAiStateMachine : MonoBehaviour 
    {
        [SerializeField] protected BaseNpc m_npc = default;
        [SerializeField] protected NpcController m_controller = default;
        [SerializeField] protected AiAction<IActionInput> m_defaultAction = default;
        [SerializeField] protected List<AiAction<IActionInput>> m_optionalActions = default;
        protected float m_timer = 0f;
        protected AiAction<IActionInput> m_current;
        AiWorldState m_worldStateSnapshot = new AiWorldState();
        protected virtual AiWorldState WorldStateSnapshot
        {
            get
            {
                return m_worldStateSnapshot;
            }
        }

        protected virtual List<AiAction<IActionInput>> ValidStates
        {
            get
            {
                List<AiAction<IActionInput>> validActions = new List<AiAction<IActionInput>>();
                foreach (AiAction<IActionInput> action in m_optionalActions)
                {
                    if (action.PreCondition(WorldStateSnapshot))
                    {
                        validActions.Add(action);
                    }
                }
                return validActions;
            }
        }

        protected virtual AiAction<IActionInput> BestAction()
        {            
            List<AiAction<IActionInput>> newStates = ValidStates;
            AiAction<IActionInput> best;
            if (newStates.Count > 0) 
            {
                best = newStates[0];
                for (int i = 0; i < newStates.Count; ++i)
                {
                    if (newStates[i].Priority(WorldStateSnapshot) > best.Priority(WorldStateSnapshot))
                    {
                        best = newStates[i];
                    }
                }
            }
            else 
            {
                best = m_defaultAction;
            }
            return best;
            
        }

        protected void OnEnable()
        {
            m_npc.OnEvaluate += OnInteraction;
        }
        protected void OnDisable()
        {
            m_npc.OnEvaluate -= OnInteraction;
        }

        protected virtual void Start() 
        {
            UpdateWorldState();
            ExecuteAction(m_defaultAction);
        }

        // Determine state changes or additional behaviour
        protected virtual void Evaluate() 
        {
            UpdateWorldState();
            AiAction<IActionInput> newState = BestAction();
            if (newState != m_current) 
            {
                ExecuteAction(newState);
            }
        }

        // Determine what we do with current interaction event
        protected virtual void OnInteraction(InteractionContext c) 
        {
            Evaluate();
        }

        protected virtual void ExecuteAction(AiAction<IActionInput> action)
        {
            m_current = action;
            AiActionInput input = new AiActionInput(m_controller, WorldStateSnapshot);
            m_current.OnExecute(input);
        }

        void UpdateWorldState()
        {
            m_worldStateSnapshot.CurrentStats = m_npc.CurrentStats;
            m_worldStateSnapshot.Enemies = m_npc.Enemies;
            m_worldStateSnapshot.Allies = m_npc.Allies;
            m_worldStateSnapshot.BasicSkills = m_npc.BasicSkills.Skills;
            m_worldStateSnapshot.DrawSkills = m_npc.DrawSkills.Skills;
        }
    }
}
