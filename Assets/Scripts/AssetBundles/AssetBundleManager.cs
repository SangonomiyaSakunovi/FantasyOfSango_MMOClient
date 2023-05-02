using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

//Developer : SangonomiyaSakunovi
//Discription: The ABManager, define the bundle method, not done.

public class AssetBundleManager : Singleton<AssetBundleManager>
{
    string CONFIGPATH = "Assets/StreamingAssets/assetbundleconfig";

    //This Dict can let you use CRC to find the ResourceItem
    protected Dictionary<uint, ResourceItem> m_ResourceItemDict = new Dictionary<uint, ResourceItem>();
    //This Dict can let you use CRC to find the Loaded AssetBundleItem
    protected Dictionary<uint, AssetBundleItem> m_AssetBundleItemDict = new Dictionary<uint, AssetBundleItem>();
    protected ClassObjectPool<AssetBundleItem> m_AssetBundleItemPool = ObjectManager.Instance.GetOrCreateClassPool<AssetBundleItem>(1000);

    public bool LoadAssetBundleConfig()
    {
        m_ResourceItemDict.Clear();
        AssetBundle configABBundle = AssetBundle.LoadFromFile(CONFIGPATH);
        TextAsset textAsset = configABBundle.LoadAsset<TextAsset>("assetbundleconfig");
        if (textAsset != null)
        {
            Debug.LogError("There is no AssetBundleConfig to load?");
            return false;
        }

        MemoryStream stream = new MemoryStream(textAsset.bytes);
        BinaryFormatter bf = new BinaryFormatter();
        AssetBundleConfig config = (AssetBundleConfig)bf.Deserialize(stream);
        stream.Close();

        for (int i = 0; i < config.ABList.Count; i++)
        {
            ABBase abBase = config.ABList[i];
            ResourceItem item = new ResourceItem
            {
                m_Crc = abBase.Crc,
                m_AssetName = abBase.AssetName,
                m_ABName = abBase.ABName,
                m_DependAssetBundleList = abBase.ABDependList
            };
            if (m_ResourceItemDict.ContainsKey(item.m_Crc))
            {
                Debug.LogError("There are conflict RCR in two ResourceItem: " + item.m_AssetName + " and the ABName is: " + item.m_ABName);
            }
            else
            {
                m_ResourceItemDict.Add(item.m_Crc, item);
            }
        }
        return true;
    }

    public ResourceItem LoadResourceAssetBundle(uint crc)
    {
        ResourceItem item = null;
        if (!m_ResourceItemDict.TryGetValue(crc, out item) || item == null)
        {
            Debug.LogError("LoadResourceAssetBundle Error: there is no crc " + crc.ToString() + " in AssetBundleConfig");
            return item;
        }
        if (item.m_AssetBundle != null)
        {
            return item;
        }
        item.m_AssetBundle = LoadAssetBundle(item.m_ABName);

        if (item.m_DependAssetBundleList != null)
        {
            for (int i = 0; i < item.m_DependAssetBundleList.Count; i++)
            {
                LoadAssetBundle(item.m_DependAssetBundleList[i]);
            }
        }
        return item;
    }

    private AssetBundle LoadAssetBundle(string name)
    {
        AssetBundleItem item = null;
        uint crc = CRC32.GetCRC32(name);
        if (!m_AssetBundleItemDict.TryGetValue(crc, out item))
        {
            AssetBundle assetBundle = null;
            string fullPath = Application.streamingAssetsPath + "/" + name;
            if (File.Exists(fullPath))
            {
                assetBundle = AssetBundle.LoadFromFile(fullPath);
            }
            if (assetBundle == null)
            {
                Debug.Log("LoadAssetBundle Error: " + fullPath);
            }
            item = m_AssetBundleItemPool.Spawn(true);
            item.assetBundle = assetBundle;
            item.RefCount++;
            m_AssetBundleItemDict.Add(crc, item);
        }
        else
        {
            item.RefCount++;
        }
        return item.assetBundle;
    }

    public void ReleaseAsset(ResourceItem item)
    {
        if (item == null)
        {
            return;
        }
        if (item.m_DependAssetBundleList != null && item.m_DependAssetBundleList.Count > 0)
        {
            for (int i = 0; i < item.m_DependAssetBundleList.Count; i++)
            {
                UnLoadAssetBundle(item.m_DependAssetBundleList[i]);
            }
        }
        UnLoadAssetBundle(item.m_AssetName);
    }

    private void UnLoadAssetBundle(string name)
    {
        AssetBundleItem item = null;
        uint crc = CRC32.GetCRC32(name);
        if (m_AssetBundleItemDict.TryGetValue(crc, out item) && item!= null)
        {
            item.RefCount--;
            if (item.RefCount <= 0 && item.assetBundle != null)
            {
                item.assetBundle.Unload(true);
                item.Rest();
                m_AssetBundleItemPool.Recycle(item);
                m_AssetBundleItemDict.Remove(crc);
            }
        }
    }

    public ResourceItem FindeResourceItem(uint crc)
    {
        return m_ResourceItemDict[crc];
    }
}

public class AssetBundleItem
{
    public AssetBundle assetBundle = null;
    public int RefCount;

    public void Rest()
    {
        assetBundle = null;
        RefCount = 0;
    }
}

public class ResourceItem
{
    public uint m_Crc = 0;    //This CRC is the ResourcePathCRC
    public string m_AssetName = string.Empty;
    public string m_ABName = string.Empty;
    public List<string> m_DependAssetBundleList = null;
    public AssetBundle m_AssetBundle = null;
}
