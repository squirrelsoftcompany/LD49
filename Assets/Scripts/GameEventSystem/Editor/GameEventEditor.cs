using UnityEditor;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

namespace GameEventSystem.Editor {
    [CustomEditor(typeof(GameEvent))]
    public class GameEventEditor : UnityEditor.Editor {
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            GUI.enabled = Application.isPlaying;
            var gameEvent = target as GameEvent;
            if (GUILayout.Button("Raise")) {
                Debug.Assert(gameEvent != null, nameof(gameEvent) + " != null");
                gameEvent.Raise();
            }
        }
    }
}