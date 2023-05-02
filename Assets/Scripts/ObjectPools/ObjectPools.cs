using System.Collections.Generic;

//Developer : SangonomiyaSakunovi
//Discription: The Object Pools.

public static class GameObjectPools<T> where T : new()
{
    private static int maxCount = 10;
    private static Stack<T> objectPool;
    private static int createdCount;

    static GameObjectPools()
    {
        objectPool = new Stack<T>();
    }

    public static int MaxCount
    {
        get
        {
            return maxCount;
        }
        set
        {
            maxCount = value;
        }
    }

    public static int CreatedCount
    {
        get
        {
            return createdCount;
        }
    }

    public static int ValidCount()
    {
        return objectPool.Count;
    }

    public static T GetObject()
    {
        T result;
        if (objectPool.Count > 0)
        {
            result = objectPool.Pop();
        }
        else
        {
            result = new T();
            createdCount++;
        }
        return result;
    }

    public static void RecyclePool(T ob)
    {
        if (objectPool.Count < maxCount)
        {
            objectPool.Push(ob);
            createdCount++;
        }

    }

    public static void CleanPool()
    {
        objectPool.Clear();
        createdCount = 0;
    }
}
