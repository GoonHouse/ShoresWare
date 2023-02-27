#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ShoresWare.Core {
    [InitializeOnLoad]
    public class SaveOnPlay: ScriptableObject
    {
        static SaveOnPlay() {
            EditorApplication.playModeStateChanged += DoSave;
        }

        private static void DoSave(PlayModeStateChange playModeStateChange) {
            if (!EditorApplication.isPlayingOrWillChangePlaymode || EditorApplication.isPlaying) return;
            Debug.Log($"Auto-Saving scene before entering Play mode: {SceneManager.GetActiveScene().name}");

            EditorSceneManager.SaveOpenScenes();
            AssetDatabase.SaveAssets();
        }
    }
}
#endif