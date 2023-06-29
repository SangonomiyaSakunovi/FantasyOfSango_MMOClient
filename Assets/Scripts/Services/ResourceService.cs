using SangoMMOCommons.Tools;
using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.SceneManagement;

//Developer : SangonomiyaSakunovi

public class ResourceService : BaseService
{
    public static ResourceService Instance = null;

    private Dictionary<string, AudioClip> audioClipDict = new Dictionary<string, AudioClip>();
    private Dictionary<string, Sprite> spriteDict = new Dictionary<string, Sprite>();

    private Dictionary<string, MissionConfig> missionConfigDict = new Dictionary<string, MissionConfig>();
    private Dictionary<string, WeaponBreakConfig> weaponBreakConfigDict = new Dictionary<string, WeaponBreakConfig>();
    private Dictionary<string, WeaponDetailsConfig> weaponDetailsConfigDict = new Dictionary<string, WeaponDetailsConfig>();
    private Dictionary<string, WeaponValueConfig> weaponValueConfigDict = new Dictionary<string, WeaponValueConfig>();

    private Action loadingProgressCallBack = null;

    public override void InitService()
    {
        Instance = this;
        InitMissionConfig(ConfigConstant.MissionConfigPath_01);
        InitWeaponBreakConfig(ConfigConstant.WeaponBreakConfigPath_01);
        InitWeaponDetailsConfig(ConfigConstant.WeaponDetailsConfigPath_01);
        InitWeaponValueConfig(ConfigConstant.WeaponValueConfigPath_01);
    }

    private void Update()
    {
        if (loadingProgressCallBack != null)
        {
            loadingProgressCallBack();
        }
    }

    public void AsyncLoadScene(string sceneName, Action loadedActionCallBack)
    {
        LoadingSystem.Instance.OpenLoadingWindow();
        LoadingSystem.Instance.SetTips("感谢您参加本次测试");
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        loadingProgressCallBack = () =>
            {
                float loadingProgress = asyncOperation.progress;
                LoadingSystem.Instance.SetLoadingProgress(loadingProgress);
                if (loadingProgress == 1)
                {
                    if (loadedActionCallBack != null)
                    {
                        loadedActionCallBack();
                    }
                    loadingProgressCallBack = null;
                    asyncOperation = null;
                    LoadingSystem.Instance.CloseLoadingWindow();
                }
            };
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
                    case "coinRewards":
                        config.coinRewards = int.Parse(element.InnerText);
                        break;
                    case "worldExpRewards":
                        config.worldExpRewards = int.Parse(element.InnerText);
                        break;
                    case "material1Rewards":
                        config.material1Rewards = element.InnerText;
                        break;
                    case "material2Rewards":
                        config.material2Rewards = element.InnerText;
                        break;
                }
            }
            missionConfigDict.Add(missionId, config);
        }
    }

    public MissionConfig GetMissionConfig(string missionId)
    {
        return DictTool.GetDictValue<string, MissionConfig>(missionId, missionConfigDict);
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

    private void InitWeaponDetailsConfig(string path)
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
            WeaponDetailsConfig config = new WeaponDetailsConfig
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
                    case "weaponQuanlity":
                        config.weaponQuanlity = int.Parse(element.InnerText);
                        break;
                    case "weaponDescribe":
                        config.weaponDescribe = element.InnerText;
                        break;
                }
            }
            weaponDetailsConfigDict.Add(weaponInfoId, config);
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
                    case "weaponAccumulateExp":
                        config.weaponAccumulateExp = int.Parse(element.InnerText);
                        break;
                }
            }
            weaponValueConfigDict.Add(weaponValueId, config);
        }
    }

    public WeaponBreakConfig GetWeaponBreakConfig(string weaponBreakId)
    {
        return DictTool.GetDictValue<string, WeaponBreakConfig>(weaponBreakId, weaponBreakConfigDict);
    }

    public WeaponDetailsConfig GetWeaponDetailsConfig(string weaponDetailsId)
    {
        return DictTool.GetDictValue<string, WeaponDetailsConfig>(weaponDetailsId, weaponDetailsConfigDict);
    }

    public WeaponValueConfig GetWeaponValueConfig(string weaponValueId)
    {
        return DictTool.GetDictValue<string, WeaponValueConfig>(weaponValueId, weaponValueConfigDict);
    }
    #endregion

    public AudioClip LoadAudioClip(string audioPath, bool isCache)
    {
        AudioClip audioClip = DictTool.GetDictValue<string, AudioClip>(audioPath, audioClipDict);
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
        Sprite sprite = DictTool.GetDictValue<string, Sprite>(spritePath, spriteDict);
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
