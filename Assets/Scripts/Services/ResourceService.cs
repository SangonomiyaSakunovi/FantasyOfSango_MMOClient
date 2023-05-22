using SangoCommon.Tools;
using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.SceneManagement;

//Developer : SangonomiyaSakunovi
//Discription: The Resource Service.

public class ResourceService : MonoBehaviour
{
    public static ResourceService Instance = null;

    private Dictionary<string, AudioClip> audioClipDict = new Dictionary<string, AudioClip>();
    private Dictionary<string, Sprite> spriteDict = new Dictionary<string, Sprite>();

    private Dictionary<string, MissionConfig> missionConfigDict = new Dictionary<string, MissionConfig>();
    private Dictionary<string, WeaponBreakConfig> weaponBreakConfigDict = new Dictionary<string, WeaponBreakConfig>();
    private Dictionary<string, WeaponInfoConfig> weaponInfoConfigDict = new Dictionary<string, WeaponInfoConfig>();
    private Dictionary<string, WeaponValueConfig> weaponValueConfigDict = new Dictionary<string, WeaponValueConfig>();

    public void InitService()
    {
        Instance = this;
        InitMissionConfig(ConfigConstant.MissionConfigPath_01);
    }

    private Action loadingProgressCallBack = null;

    public void AsyncLoadScene(string sceneName, Action loadedActionCallBack)
    {
        SangoRoot.Instance.loadingWindow.SetWindowState();

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        loadingProgressCallBack = () =>
            {
                float loadingProgress = asyncOperation.progress;
                SangoRoot.Instance.loadingWindow.SetLoadingProgress(loadingProgress);
                if (loadingProgress == 1)
                {
                    if (loadedActionCallBack != null)
                    {
                        loadedActionCallBack();
                    }
                    loadingProgressCallBack = null;
                    asyncOperation = null;
                    SangoRoot.Instance.loadingWindow.SetWindowState(false);
                }
            };
    }

    private void Update()
    {
        if (loadingProgressCallBack != null)
        {
            loadingProgressCallBack();
        }
    }

    #region MissionConfig
    private void InitMissionConfig(string path)
    {
        TextAsset xml = Resources.Load<TextAsset>(path);
        XmlDocument document = new XmlDocument();
        document.LoadXml(xml.text);
        XmlNodeList xmlNodeList = document.SelectSingleNode("root").ChildNodes;
        for (int i = 0; i < xmlNodeList.Count; i++)
        {
            XmlElement xmlElement = xmlNodeList[i] as XmlElement;
            if (xmlElement.GetAttributeNode("_id") == null)
            {
                continue;
            }
            string missionId = xmlElement.GetAttributeNode("_id").InnerText;
            MissionConfig config = new MissionConfig
            {
                _id = missionId
            };
            foreach (XmlElement element in xmlNodeList[i].ChildNodes)
            {
                switch (element.Name)
                {
                    case "missionName":
                        config.missionName = element.InnerText;
                        break;
                    case "npcID":
                        config.npcID = element.InnerText;
                        break;
                    case "dialogAvaterImageArray":
                        config.dialogAvaterImageArray = element.InnerText;
                        break;
                    case "dialogTextArray":
                        config.dialogTextArray = element.InnerText;
                        break;
                    case "dialogAudioArray":
                        config.dialogAudioArray = element.InnerText;
                        break;
                    case "actionID":
                        config.actionID = element.InnerText;
                        break;
                    case "guidText":
                        config.guidText = element.InnerText;
                        break;
                }
            }
            missionConfigDict.Add(missionId, config);
        }
    }

    public MissionConfig GetMissionConfig(string missionId)
    {
        return DictTools.GetDictValue<string, MissionConfig>(missionConfigDict, missionId);
    }
    #endregion

    #region WeaponsConfig
    private void InitWeaponBreakConfig(string path)
    {
        TextAsset xml = Resources.Load<TextAsset>(path);
        XmlDocument document = new XmlDocument();
        document.LoadXml(xml.text);
        XmlNodeList xmlNodeList = document.SelectSingleNode("root").ChildNodes;
        for (int i = 0; i < xmlNodeList.Count; i++)
        {
            XmlElement xmlElement = xmlNodeList[i] as XmlElement;
            if (xmlElement.GetAttributeNode("_id") == null)
            {
                continue;
            }
            string weaponBreakId = xmlElement.GetAttributeNode("_id").InnerText;
            WeaponBreakConfig config = new WeaponBreakConfig
            {
                _id = weaponBreakId
            };
            foreach (XmlElement element in xmlNodeList[i].ChildNodes)
            {
                switch (element.Name)
                {
                    case "weaponBreakCoin":
                        config.weaponBreakCoin = int.Parse(element.InnerText);
                        break;
                    case "weaponBreakMaterial1":
                        config.weaponBreakMaterial1 = element.InnerText;
                        break;
                    case "weaponBreakMaterial2":
                        config.weaponBreakMaterial2 = element.InnerText;
                        break;                    
                }
            }
            weaponBreakConfigDict.Add(weaponBreakId, config);
        }
    }

    private void InitWeaponInfoConfig(string path)
    {
        TextAsset xml = Resources.Load<TextAsset>(path);
        XmlDocument document = new XmlDocument();
        document.LoadXml(xml.text);
        XmlNodeList xmlNodeList = document.SelectSingleNode("root").ChildNodes;
        for (int i = 0; i < xmlNodeList.Count; i++)
        {
            XmlElement xmlElement = xmlNodeList[i] as XmlElement;
            if (xmlElement.GetAttributeNode("_id") == null)
            {
                continue;
            }
            string weaponInfoId = xmlElement.GetAttributeNode("_id").InnerText;
            WeaponInfoConfig config = new WeaponInfoConfig
            {
                _id = weaponInfoId
            };
            foreach (XmlElement element in xmlNodeList[i].ChildNodes)
            {
                switch (element.Name)
                {
                    case "weaponName":
                        config.weaponName = element.InnerText;
                        break;
                    case "weaponDescribe":
                        config.weaponDescribe = element.InnerText;
                        break;
                }
            }
            weaponInfoConfigDict.Add(weaponInfoId, config);
        }
    }

    private void InitWeaponValueConfig(string path)
    {
        TextAsset xml = Resources.Load<TextAsset>(path);
        XmlDocument document = new XmlDocument();
        document.LoadXml(xml.text);
        XmlNodeList xmlNodeList = document.SelectSingleNode("root").ChildNodes;
        for (int i = 0; i < xmlNodeList.Count; i++)
        {
            XmlElement xmlElement = xmlNodeList[i] as XmlElement;
            if (xmlElement.GetAttributeNode("_id") == null)
            {
                continue;
            }
            string weaponValueId = xmlElement.GetAttributeNode("_id").InnerText;
            WeaponValueConfig config = new WeaponValueConfig
            {
                _id = weaponValueId
            };
            foreach (XmlElement element in xmlNodeList[i].ChildNodes)
            {
                switch (element.Name)
                {
                    case "weaponLevel":
                        config.weaponLevel = int.Parse(element.InnerText);
                        break;
                    case "weaponBaseATK":
                        config.weaponBaseATK = int.Parse(element.InnerText);
                        break;
                    case "weaponAbility1":
                        config.weaponAbility1 = element.InnerText;
                        break;
                    case "weaponAbility2":
                        config.weaponAbility2 = element.InnerText;
                        break;
                    case "weaponEnhanceLevelExp":
                        config.weaponEnhanceLevelExp = int.Parse(element.InnerText);
                        break;
                }
            }
            weaponValueConfigDict.Add(weaponValueId, config);
        }
    }

    public WeaponBreakConfig GetWeaponBreakConfig(string weaponBreakId)
    {
        return DictTools.GetDictValue<string, WeaponBreakConfig>(weaponBreakConfigDict, weaponBreakId);
    }

    public WeaponInfoConfig GetWeaponInfoConfig(string weaponInfoId)
    {
        return DictTools.GetDictValue<string, WeaponInfoConfig>(weaponInfoConfigDict, weaponInfoId);
    }

    public WeaponValueConfig GetWeaponValueConfig(string weaponValueId)
    {
        return DictTools.GetDictValue<string, WeaponValueConfig>(weaponValueConfigDict, weaponValueId);
    }
    #endregion

    public AudioClip LoadAudioClip(string audioPath, bool isCache)
    {
        AudioClip audioClip = DictTools.GetDictValue<string, AudioClip>(audioClipDict, audioPath);
        if (audioClip == null)
        {
            audioClip = Resources.Load<AudioClip>(audioPath);
            if (isCache)
            {
                audioClipDict.Add(audioPath, audioClip);
            }
        }
        return audioClip;
    }

    public Sprite LoadSprite(string spritePath, bool isCache = false)
    {
        Sprite sprite = DictTools.GetDictValue<string, Sprite>(spriteDict, spritePath);
        if (sprite == null)
        {
            sprite = Resources.Load<Sprite>(spritePath);
            if (isCache)
            {
                spriteDict.Add(spritePath, sprite);
            }
        }
        return sprite;
    }
}
