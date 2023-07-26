using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Trash.Spawning
{
    public class DayNightCycle : MonoBehaviour
    {
        [SerializeField]
        private float m_dayTimeInSeconds;

        [SerializeField]
        private float m_nightTimeInSeconds;

        [SerializeField]
        private UnityEvent OnDayStart;

        [SerializeField]
        private UnityEvent OnDayEnd;

        [SerializeField]
        private UnityEvent OnNightStart;

        [SerializeField]
        private UnityEvent OnNightEnd;

        [SerializeField]
        private DailyEvent[] m_dailyEvents;

        private bool m_isDay;
        private int m_dayNumber;
        private int m_nightNumber;

        private IEnumerator Start()
        {
            m_isDay = true;
            m_dayNumber = 1;
            m_nightNumber = 0;
            StartDay(m_dayNumber);

            yield return new WaitForSeconds(m_dayTimeInSeconds);
            StartCoroutine(SwapDayNight());
        }
        
        private IEnumerator SwapDayNight()
        {
            if (!m_isDay)
            {
                m_dayNumber++;
                EndNight(m_nightNumber);
                yield return new WaitForSeconds(0.1f);
                StartDay(m_dayNumber);
                yield return new WaitForSeconds(m_dayTimeInSeconds);
            }
            else
            {
                m_nightNumber++;
                EndDay(m_dayNumber);
                yield return new WaitForSeconds(0.1f);
                StartNight(m_nightNumber);
                yield return new WaitForSeconds(m_nightTimeInSeconds);
            }


            m_isDay = !m_isDay;

            yield return SwapDayNight();
        }

        private void StartDay(int dayNumber)
        {
            for (int i = 0; i < OnDayStart.GetPersistentEventCount(); i++)
            {
                ((MonoBehaviour) OnDayStart.GetPersistentTarget(i)).SendMessage(OnDayStart.GetPersistentMethodName(i), dayNumber,SendMessageOptions.RequireReceiver);
            }

            foreach (DailyEvent dailyEvent in m_dailyEvents)
            {
                if (dailyEvent.Day == dayNumber)
                {
                    dailyEvent.OnDayStarted?.Invoke();
                }
            }
        }

        private void EndDay(int dayNumber)
        {
            for (int i = 0; i < OnDayEnd.GetPersistentEventCount(); i++)
            {
                ((MonoBehaviour) OnDayEnd.GetPersistentTarget(i)).SendMessage(OnDayEnd.GetPersistentMethodName(i), dayNumber);
            }
            foreach (DailyEvent dailyEvent in m_dailyEvents)
            {
                if (dailyEvent.Day == dayNumber)
                {
                    dailyEvent.OnDayEnded?.Invoke();
                }
            }
        }

        private void StartNight(int nightNumber)
        {
            for (int i = 0; i < OnNightStart.GetPersistentEventCount(); i++)
            {
                ((MonoBehaviour) OnNightStart.GetPersistentTarget(i)).SendMessage(OnNightStart.GetPersistentMethodName(i), nightNumber);
            }

            foreach (DailyEvent dailyEvent in m_dailyEvents)
            {
                if (dailyEvent.Night == nightNumber)
                {
                    dailyEvent.OnNightStarted?.Invoke();
                }
            }
        }

        private void EndNight(int nightNumber)
        {
            for (int i = 0; i < OnNightEnd.GetPersistentEventCount(); i++)
            {
                ((MonoBehaviour) OnNightEnd.GetPersistentTarget(i)).SendMessage(OnNightEnd.GetPersistentMethodName(i), nightNumber);
            }
         
            foreach (DailyEvent dailyEvent in m_dailyEvents)
            {
                if (dailyEvent.Night == nightNumber)
                {
                    dailyEvent.OnNightEnded?.Invoke();
                }
            }
        }

        [Serializable]
        public class IntEvent : UnityEvent<int>
        {
        }
    }
}