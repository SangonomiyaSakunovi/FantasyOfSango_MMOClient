using UnityEngine;

//Developer : SangonomiyaSakunovi
//Discription: The Audio Service.

public class AudioService : MonoBehaviour
{
    public static AudioService Instance = null;
    public AudioSource bGAudio;
    public AudioSource uIAudio;

    public void InitServive()
    {
        Instance = this;
    }

    public void PlayBGAudio(string audioName, bool isLoop)
    {
        AudioClip audioClip = ResourceService.Instance.LoadAudioClip("ResAudios/BGAudio/" + audioName, true);
        //Exam if the NewAudio is same as NowPlaying
        if (bGAudio.clip == null || bGAudio.clip.name != audioClip.name)
        {
            bGAudio.clip = audioClip;
            bGAudio.loop = isLoop;
            bGAudio.Play();
        }
    }

    public void PlayUIAudio(string audioName)
    {
        AudioClip audioClip = ResourceService.Instance.LoadAudioClip("ResAudios/UIAudio/" + audioName, true);
        uIAudio.clip = audioClip;
        uIAudio.Play();
    }

    public void LoadAudio(string audioName)
    {
        ResourceService.Instance.LoadAudioClip("ResAudios/BGAudio/" + audioName, true);
    }
}
