using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;

namespace Curry.Util
{
    public class SkillAnimGenerator : EditorWindow
    {
        string fileName = "AC_Skill";
        void OnGUI()
        {
            GUILayout.Label("Skill Animator Controller Generator", EditorStyles.boldLabel);
            fileName = EditorGUILayout.TextField("File Name", fileName);
            if (GUILayout.Button("Generate Anim Controller"))
            {
                string path = EditorUtility.SaveFilePanel("Save Animator Controller to folder", "Asset/", fileName, "");
                string prefix = Application.dataPath;
                if (path.Length != 0 && path.StartsWith(prefix))
                {
                    path = path.Substring(prefix.Length).TrimStart(Path.DirectorySeparatorChar);
                    Debug.Log(path);

                    CreateSkillAnimController($"Assets{path}");
                }
            }
        }

        // Add menu item named "My Window" to the Window menu
        [MenuItem("Curry/Skill/Anim Controller Generator")]
        public static void ShowWindow()
        {
            //Show existing window instance. If one doesn't exist, make one.
            GetWindow(typeof(SkillAnimGenerator));
        }

        void CreateSkillAnimController(string saveAddress)
        {
            // Creates the controller
            var controller = AnimatorController.CreateAnimatorControllerAtPath($"{saveAddress}.controller");

            AnimationClip idle = new AnimationClip();
            AssetDatabase.CreateAsset(idle, $"{saveAddress}_idle.anim");
            AnimationClip start = new AnimationClip();
            AssetDatabase.CreateAsset(start, $"{saveAddress}_start.anim");
            AnimationClip end = new AnimationClip();
            AssetDatabase.CreateAsset(end, $"{saveAddress}_end.anim");

            // Add StateMachines
            var rootStateMachine = controller.layers[0].stateMachine;

            // Add States
            var onIdle = rootStateMachine.AddState("OnIdle");
            onIdle.motion = idle;
            var onStart = rootStateMachine.AddState("OnStart");
            onStart.motion = start;
            var onEnd = rootStateMachine.AddState("OnExit");

            onEnd.motion = end;
            // Add parameters
            controller.AddParameter("Start", AnimatorControllerParameterType.Trigger);
            controller.AddParameter("End", AnimatorControllerParameterType.Trigger);

            // Add Transitions
            var idleToStart = onIdle.AddTransition(onStart);
            idleToStart.AddCondition(AnimatorConditionMode.If, 1f, "Start");
            idleToStart.duration = 0f;

            var idleToEnd = onIdle.AddTransition(onEnd);
            idleToEnd.AddCondition(AnimatorConditionMode.If, 1f, "End");
            idleToEnd.duration = 0f;

            var startToIdle = onStart.AddTransition(onIdle);
            startToIdle.hasExitTime = true;
            startToIdle.duration = 0f;

            var endToIdle = onEnd.AddTransition(onIdle);
            endToIdle.hasExitTime = true;
            endToIdle.duration = 0f;
        }



    }
}
