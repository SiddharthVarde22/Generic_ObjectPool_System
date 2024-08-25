using System.Collections.Generic;
using System;

namespace siddharthvarde22
{
    public class GenericObjectPool<T>
    {
        Stack<T> m_objectPool;

        Func<T> m_OnFactoryMethodToCall;
        Action<T> m_OnReleaseMethodToCall;
        Action<T> m_OnDestroyMethodToCall;
        Action<T> m_OnGetMethodToCall;

        public GenericObjectPool(int a_minimumLength, Func<T> a_onFactoryMethodToCall, Action<T> a_onGetMethodToCall, Action<T> a_onRealeseMethodToCall, Action<T> a_onDestroyMethodToCall)
        {
            m_objectPool = new Stack<T>(a_minimumLength);
            m_OnFactoryMethodToCall = a_onFactoryMethodToCall;
            m_OnGetMethodToCall = a_onGetMethodToCall;
            m_OnReleaseMethodToCall = a_onRealeseMethodToCall;
            m_OnDestroyMethodToCall = a_onDestroyMethodToCall;
        }

        public T Get()
        {
            if(m_objectPool.TryPop(out T l_object))
            {
                m_OnGetMethodToCall.Invoke(l_object);
                return l_object;
            }
            else
            {
                return m_OnFactoryMethodToCall.Invoke();
            }
        }

        public void Release(T a_object)
        {
            m_OnReleaseMethodToCall.Invoke(a_object);
            m_objectPool.Push(a_object);
        }

        public void DestroyPool()
        {
            T l_object;
            for(int i = 0, l_length = m_objectPool.Count; i < l_length; i++)
            {
                l_object = m_objectPool.Pop();
                m_OnReleaseMethodToCall.Invoke(l_object);
                m_OnDestroyMethodToCall.Invoke(l_object);
            }

            m_objectPool.Clear();
        }

        ~GenericObjectPool()
        {
            m_objectPool = null;
            m_OnFactoryMethodToCall = null;
            m_OnReleaseMethodToCall = null;
            m_OnDestroyMethodToCall = null;
        }
    }
}
