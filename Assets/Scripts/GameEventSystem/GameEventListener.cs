using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GameEventSystem {
    [AddComponentMenu("GameEventListener")]
    public class GameEventListener : MonoBehaviour {
        //[Reorderable]
        public List<EventAndResponse> eventAndResponses = new List<EventAndResponse>();

        private void OnEnable() {
            if (eventAndResponses.Count == 0) return;
            foreach (var eAndR in eventAndResponses) {
                eAndR.gameEvent.RegisterListener(this);
            }
        }

        private void OnDisable() {
            if (eventAndResponses.Count == 0) return;
            foreach (var eAndR in eventAndResponses) {
                eAndR.gameEvent.UnregisterListener(this);
            }
        }

        [ContextMenu("Raise Events")]
        public void OnEventRaised(GameEvent passedEvent) {
            for (var i = eventAndResponses.Count - 1; i >= 0; i--) {
                // Check if the passed event is the correct one
                if (passedEvent == eventAndResponses[i].gameEvent) {
                    // Uncomment the line below for debugging the event listens and other details
//                    Debug.Log("Called Event named: " + eventAndResponses[i].name + " on GameObject: " +
//                              gameObject.name);
                    eventAndResponses[i].EventRaised();
                }
            }
        }

        [Serializable]
        public class EventAndResponse {
            public string name;
            public GameEvent gameEvent;
            public UnityEvent response;
            public ResponseWithString responseForSentString;
            public ResponseWithInt responseForSentInt;
            public ResponseWithFloat responseForSentFloat;
            public ResponseWithBool responseForSentBool;
            public ResponseWithMonoBehaviour responseForSentMonoBehaviour;

            public void EventRaised() {
                // default/generic
                if (response.GetPersistentEventCount() >= 1) {
                    // always check if at least 1 object is listening for the event
                    response.Invoke();
                }

                // string
                if (responseForSentString.GetPersistentEventCount() >= 1) {
                    responseForSentString.Invoke(gameEvent.sentString);
                }

                // int
                if (responseForSentInt.GetPersistentEventCount() >= 1) {
                    responseForSentInt.Invoke(gameEvent.sentInt);
                }

                // float
                if (responseForSentFloat.GetPersistentEventCount() >= 1) {
                    responseForSentFloat.Invoke(gameEvent.sentFloat);
                }

                // bool
                if (responseForSentBool.GetPersistentEventCount() >= 1) {
                    responseForSentBool.Invoke(gameEvent.sentBool);
                }

                // Flammable
                if (responseForSentMonoBehaviour.GetPersistentEventCount() >= 1) {
                    responseForSentMonoBehaviour.Invoke(gameEvent.sentMonoBehaviour);
                }
            }
        }
    }

    [Serializable]
    public class ResponseWithString : UnityEvent<string> { }

    [Serializable]
    public class ResponseWithInt : UnityEvent<int> { }

    [Serializable]
    public class ResponseWithFloat : UnityEvent<float> { }

    [Serializable]
    public class ResponseWithBool : UnityEvent<bool> { }

    [Serializable]
    public class ResponseWithMonoBehaviour : UnityEvent<MonoBehaviour> { }
}