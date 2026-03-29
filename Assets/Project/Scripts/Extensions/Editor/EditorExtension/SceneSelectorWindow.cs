using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.IO;

namespace Core.Extensions
{
    public class SceneSelectorWindow : EditorWindow
    {
        private Vector2 _scrollPos;

        [MenuItem("Window/Scene Selector")]
        public static void ShowWindow()
        {
            GetWindow<SceneSelectorWindow>("Scene Selector");
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("Scenes in Build Settings", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);

            var scenes = EditorBuildSettings.scenes;

            if (scenes.Length == 0)
            {
                EditorGUILayout.HelpBox("No scenes found in Build Settings!", MessageType.Info);
            }

            foreach (var scene in scenes)
            {
                if (scene == null) continue;

                string scenePath = scene.path;
                string sceneName = Path.GetFileNameWithoutExtension(scenePath);

                GUI.enabled = scene.enabled;
                if (GUILayout.Button(sceneName, GUILayout.Height(30)))
                {
                    if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                    {
                        EditorSceneManager.OpenScene(scenePath);
                    }
                }
                GUI.enabled = true;
            }

            EditorGUILayout.EndScrollView();

            EditorGUILayout.Space();
            if (GUILayout.Button("Refresh List"))
            {
                Repaint();
            }
        }
    }
}