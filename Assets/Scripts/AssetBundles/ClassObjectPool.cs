using System.Collections.Generic;

//Developer : SangonomiyaSakunovi
//Discription: A special objectPool.

public class ClassObjectPool<T> where T : class, new()
{
    protected Stack<T> m_Pool = new Stack<T>();
    protected int m_MaxCount = 0;
    protected int m_NoRecycleCount = 0;

    public ClassObjectPool(int maxCount)
    {
        m_MaxCount = maxCount;
        for (int i = 0; i < m_MaxCount; i++)
        {
            m_Pool.Push(new T());
        }
    }

    public T Spawn(bool creatIfPoolEmpty)
    {
        if (m_Pool.Count > 0)
        {
            T rtn = m_Pool.Pop();
            if (rtn == null)
            {
                if (creatIfPoolEmpty)
                {
                    rtn = new T();
                }
            }
            m_NoRecycleCount++;
            return rtn;
        }
        else
        {
            if (creatIfPoolEmpty)
            {
                T rtn = new T();
                m_NoRecycleCount++;
                return rtn;
            }
        }
        return null;
    }

    public bool Recycle(T obj)
    {
        if (obj == null)
        {
            return false;
        }
        m_NoRecycleCount--;
        if (m_Pool.Count >= m_MaxCount && m_MaxCount > 0)
        {
            obj = null;
            return false;
        }
        m_Pool.Push(obj);
        return true;
    }
}
