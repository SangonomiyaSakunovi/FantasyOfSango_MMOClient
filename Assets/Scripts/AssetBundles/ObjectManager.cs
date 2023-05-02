using System;
using System.Collections.Generic;

//Developer : SangonomiyaSakunovi
//Discription: The ABBundle objectPool manager.
public class ObjectManager : Singleton<ObjectManager>
{
    protected Dictionary<Type,object> m_ClassPoolDict = new Dictionary<Type, object>();
    public ClassObjectPool<T> GetOrCreateClassPool<T>(int maxCount) where T : class, new()
    {
        Type type = typeof(T);
        object outObj = null;
        if (!m_ClassPoolDict.TryGetValue(type, out outObj) || outObj == null)
        {
            ClassObjectPool<T> newPool = new ClassObjectPool<T>(maxCount);
            m_ClassPoolDict.Add(type, newPool);
            return newPool;
        }
        return (ClassObjectPool<T>)outObj;
    }
}
