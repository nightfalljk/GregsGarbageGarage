using System;
using UnityEngine.Events;

namespace Trash.Spawning
{
    [Serializable]
    public class DailyEvent
    {
        public int Day = -1;
        public int Night = -1;
        public UnityEvent OnDayStarted;
        public UnityEvent OnDayEnded;
        public UnityEvent OnNightStarted;
        public UnityEvent OnNightEnded;
    }
}