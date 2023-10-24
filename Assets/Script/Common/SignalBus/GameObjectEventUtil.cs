using System.Collections.Generic;
using UnityEngine;

namespace ZOne
{
    public static class GameObjectEventUtil
    {
        private static Dictionary<GameObject, EventUtil> m_EventBinders = new Dictionary<GameObject, EventUtil>(); 
        public static void Register(this GameObject go, object component)
        {
            m_EventBinders.TryAdd(go, new EventUtil());
            m_EventBinders[go].Register(component);
        }

        public static void Unregister(this GameObject go)
        {
            m_EventBinders[go] = null;
        }

        public static void Send<T>(this GameObject go, T evt)
        {
            m_EventBinders[go].Send(evt);
        }
    }
}