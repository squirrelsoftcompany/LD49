using System.Collections.Generic;
using UnityEngine;

namespace GameEventSystem {
    [CreateAssetMenu(fileName = "GameEvent", menuName = "GameEvent")]
    public class GameEvent : ScriptableObject {
        public string sentString;
        public int sentInt;
        public float sentFloat;
        public bool sentBool;
        public MonoBehaviour sentMonoBehaviour;

        private readonly List<GameEventListener> _listeners = new List<GameEventListener>();

        [ContextMenu("Raise Event")]
        public void Raise() {
            for (var i = _listeners.Count - 1; i >= 0; i--) {
                _listeners[i].OnEventRaised(this);
            }
        }

        public void RegisterListener(GameEventListener listener) {
            if (!_listeners.Contains(listener)) {
                _listeners.Add(listener);
            }
        }

        public void UnregisterListener(GameEventListener listener) {
            if (_listeners.Contains(listener)) {
                _listeners.Remove(listener);
            }
        }
    }
}