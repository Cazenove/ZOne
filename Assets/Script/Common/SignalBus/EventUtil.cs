﻿using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace ZOne
{
    public class EventUtil
    {
        private class FuncGroup
        {
            private FuncGroup(){}
            public static FuncGroup Create<T>()
            {
                var group = new FuncGroup();
                group.call = obj =>
                {
                    foreach (var action in group.actions)
                    {
                        ((Action<T>)action)((T)obj);
                    }
                };
                return group;
            }

            public Action<object> call;
            public List<object> actions = new List<object>();
        }
        
        private static Dictionary<Type, FuncGroup> m_BindEvents = new Dictionary<Type, FuncGroup>();
        
        public static void Send<T>(T evt)
        {
            var type = evt.GetType();
            if (!m_BindEvents.ContainsKey(type))
            {
                Debug.LogError($"未注册过 {type.FullName} 类型的事件");
                return;
            }
            m_BindEvents[type].call.Invoke(evt);
        }

        private static object[] s_OC = new object[1];
        public static Action Register(object component)
        {
            var sType = component.GetType();
            List<object> list = new List<object>();
            foreach (MethodInfo method in sType.GetMethods(BindingFlags.Public | BindingFlags.NonPublic |
                                                           BindingFlags.Instance))
            {
                var attribute = method.GetCustomAttribute<EventBusAttribute>();
                if (attribute == null)
                    continue;
                var pType = method.GetParameters()[0].ParameterType;

                var fType = typeof(EventUtil);
                var mi1 = fType.GetMethod("RegisterAction");
                var mi2 = mi1.MakeGenericMethod(pType);
                
                s_OC[0] = method.CreateDelegate(mi2.GetParameters()[0].ParameterType, component);
                list.Add(mi2.Invoke(null, s_OC));
            }

            return () =>
            {
                foreach (var a in list)
                {
                    ((Action)a).Invoke();
                }
            };
        }
        
        public static Action RegisterAction<T>(Action<T> action)
        {
            var type = typeof(T);
            if (!m_BindEvents.ContainsKey(type))
            {
                var fgType = typeof(FuncGroup);
                MethodInfo m1 = fgType.GetMethod("Create");
                MethodInfo m2 = m1.MakeGenericMethod(type);
                m_BindEvents[type] = (FuncGroup)m2.Invoke(fgType, null);
            }
            m_BindEvents[type].actions.Add(action);
            return () =>
            {
                UnregisterAction(action);
            };
        }

        public static void UnregisterAction<T>(Action<T> action)
        {
            var type = typeof(T);
            if (!m_BindEvents.ContainsKey(type))
            {
                var fgType = typeof(FuncGroup);
                MethodInfo m1 = fgType.GetMethod("Create");
                MethodInfo m2 = m1.MakeGenericMethod(type);
                m_BindEvents[type] = (FuncGroup)m2.Invoke(fgType, null);
            }
            m_BindEvents[type].actions.Remove(action);
        }
    }
}