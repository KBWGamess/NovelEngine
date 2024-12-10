using DIALOGUE;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace History
{
    [RequireComponent(typeof(HistoryLogManager))]
    [RequireComponent(typeof(HistoryNavigator))]
    public class HistoryManager : MonoBehaviour
    {
        public const int HISTORY_CACHE_LIMIT = 15;
        public static HistoryManager instance { get; private set; }
        public List<HistoryState> history = new List<HistoryState>();

        private HistoryNavigator navigation;
        public HistoryLogManager logManager { get; private set; }

        private void Awake()
        {
            instance = this;
            navigation = GetComponent<HistoryNavigator>();
            logManager = GetComponent<HistoryLogManager>();
        }

        void Start()
        {
            DialogueSystem.instance.onClear += LogCurrentState;
        }

        public void LogCurrentState()
        {
            HistoryState state = HistoryState.Capture();
            history.Add(state);
            logManager.AddLog(state);

            if (history.Count > HISTORY_CACHE_LIMIT)
                history.RemoveAt(0);
        }

        public void LoadState(HistoryState state)
        {
            state.Load();
        }

        public void GoForward() => navigation.GoForward();
        public void GoBack() => navigation.GoBack();
    }
}