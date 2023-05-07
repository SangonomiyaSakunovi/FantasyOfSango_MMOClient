using SangoCommon.Tools;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Developer : SangonomiyaSakunovi
//Discription: The Resource Service.

public class ResourceService : MonoBehaviour
{
    public static ResourceService Instance = null;

    private Dictionary<string, AudioClip> audioClipDict = new Dictionary<string, AudioClip>();

    public void InitService()
    {
        Instance = this;
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
}
