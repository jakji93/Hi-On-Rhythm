using UnityEditor;
using UnityEngine;

namespace Kamgam.SettingsGenerator
{
    [CustomEditor(typeof(SettingsProvider))]
    public class SettingProviderEditor : Editor
    {
        public SettingsProvider provider;

        public void OnEnable()
        {
            provider = target as SettingsProvider;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button(new GUIContent("Delete","NOTICE: If you are using a custom delete method then this will only work during PLAY MODE.")))
            {
                provider.Delete();
            }
            GUI.enabled = EditorApplication.isPlaying;
            if (GUILayout.Button("Save"))
            {
                provider.Save();
            }
            if (GUILayout.Button("Load"))
            {
                provider.Load();
            }
            GUI.enabled = true;
            GUILayout.EndHorizontal();
        }
    }
}
