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
    private Dictionary<string, IslandMissionConfig> islandMissionConfigDict = new Dictionary<string, IslandMissionConfig>();

    public void InitService()
    {
        Instance = this;
        InitIslandMissionConfig(ConfigConstant.IslandMissionConfigPath);
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

    #region MissionConfig
    private void InitIslandMissionConfig(string path)
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
            string id = xmlElement.GetAttributeNode("_id").InnerText;
            IslandMissionConfig config = new IslandMissionConfig
            {
                _id = id
            };
            foreach (XmlElement element in xmlNodeList[i].ChildNodes)
            {
                switch (element.Name)
                {
                    case "npcID":
                        config.npcID = element.InnerText;
                        break;
                    case "dialogArray":
                        config.dialogArray = element.InnerText;
                        break;
                    case "dialogAudio":
                        config.dialogAudio = element.InnerText;
                        break;
                    case "actionID":
                        config.actionID = element.InnerText;
                        break;
                }
            }
            islandMissionConfigDict.Add(id, config);
        }
    }
    public IslandMissionConfig GetIslandMissionConfig(string id)
    {
        return DictTools.GetDictValue<string, IslandMissionConfig>(islandMissionConfigDict, id);
    }

    #endregion
}
