using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

//Developer : SangonomiyaSakunovi
//Discription: Just a test.

public class TestABBundleLoad : MonoBehaviour
{
    void Start()
    {
        //TestLoadAB();
        AssetBundleManager.Instance.LoadAssetBundleConfig();
    }

    void TestLoadAB()
    {
        AssetBundle assetBundleConfig = AssetBundle.LoadFromFile("Assets/StreamingAssets/assetbundleconfig");
        TextAsset textAsset = assetBundleConfig.LoadAsset<TextAsset>("AssetBundleConfig");
        MemoryStream stream = new MemoryStream(textAsset.bytes);
        BinaryFormatter bf = new BinaryFormatter();
        AssetBundleConfig testSerilize = (AssetBundleConfig)bf.Deserialize(stream);
        stream.Close();
        string path = "Assets/_Resources/Scenes/Prefabs/Shitou/Shitou_hill_01.prefab";
        uint crc = CRC32.GetCRC32(path);
        ABBase abBase = null;
        for (int i = 0; i < testSerilize.ABList.Count; i++)
        {
            if (testSerilize.ABList[i].Crc == crc)
            {
                abBase = testSerilize.ABList[i];
            }
        }
        for (int i = 0; i < abBase.ABDependList.Count; i++)
        {
            AssetBundle.LoadFromFile("Assets/StreamingAssets/" + abBase.ABDependList[i]);
        }
        AssetBundle assetBundle = AssetBundle.LoadFromFile("Assets/StreamingAssets/" + abBase.ABName);
        GameObject obj = GameObject.Instantiate(assetBundle.LoadAsset<GameObject>(abBase.AssetName));
    }
}
