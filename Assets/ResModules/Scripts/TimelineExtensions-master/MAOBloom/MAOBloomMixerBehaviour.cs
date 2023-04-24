// This code is automatically generated by PostProcessing Extension Wizard.
// For more information, please visit 
// https://github.com/ShiinaRinne/TimelineExtensions

using System;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class MAOBloomMixerBehaviour : PlayableBehaviour
{
    float m_DefaultThreshold;
    float m_DefaultIntensity;
    float m_DefaultScatter;
    float m_DefaultClamp;
    Color m_DefaultTint;
    bool m_DefaultHighQualityFiltering;
    int m_DefaultSkipIterations;
    Texture m_DefaultDirtTexture;
    float m_DefaultDirtIntensity;

    Bloom m_TrackBinding;
    bool m_FirstFrameHappened;

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        ((Volume) playerData).profile.TryGet(out m_TrackBinding);
        if (m_TrackBinding == null)
            return;
        
        if(!m_FirstFrameHappened)
        {
            m_DefaultThreshold = m_TrackBinding.threshold.value;
            m_DefaultIntensity = m_TrackBinding.intensity.value;
            m_DefaultScatter = m_TrackBinding.scatter.value;
            m_DefaultClamp = m_TrackBinding.clamp.value;
            m_DefaultTint = m_TrackBinding.tint.value;
            m_DefaultHighQualityFiltering = m_TrackBinding.highQualityFiltering.value;
            m_DefaultSkipIterations = m_TrackBinding.skipIterations.value;
            m_DefaultDirtTexture = m_TrackBinding.dirtTexture.value;
            m_DefaultDirtIntensity = m_TrackBinding.dirtIntensity.value;

            m_FirstFrameHappened = true;
        }


        int inputCount = playable.GetInputCount();
        float blendedThreshold = 0f;
        float blendedIntensity = 0f;
        float blendedScatter = 0f;
        float blendedClamp = 0f;
        Color blendedTint = Color.clear;
        bool blendedHighQualityFiltering = false;
        float blendedSkipIterations = 0;
        Texture blendedDirtTexture = new Texture2D(1,1);
        float blendedDirtIntensity = 0f;

        float totalWeight = 0f;
        float greatestWeight = 0f;
        int currentInputs = 0;

        for(int i = 0; i < inputCount; i++)
        {
            float inputWeight = playable.GetInputWeight(i);
            ScriptPlayable<MAOBloomBehaviour> inputPlayable =(ScriptPlayable<MAOBloomBehaviour>)playable.GetInput(i);
            MAOBloomBehaviour input = inputPlayable.GetBehaviour();
            
            blendedThreshold += input.Threshold * inputWeight;
            blendedIntensity += input.Intensity * inputWeight;
            blendedScatter += input.Scatter * inputWeight;
            blendedClamp += input.Clamp * inputWeight;
            blendedTint += input.Tint * inputWeight;
            blendedHighQualityFiltering = inputWeight > 0.5 ? input.HighQualityFiltering : blendedHighQualityFiltering;
            blendedSkipIterations += input.SkipIterations * inputWeight;
            blendedDirtTexture = inputWeight > 0.5 ? input.DirtTexture : blendedDirtTexture;
            blendedDirtIntensity += input.DirtIntensity * inputWeight;

            totalWeight += inputWeight;

            if (inputWeight > greatestWeight)
            {
                greatestWeight = inputWeight;
            }

            if (!Mathf.Approximately (inputWeight, 0f))
                currentInputs++;
        }
        m_TrackBinding.threshold.value = blendedThreshold + m_DefaultThreshold * (1f-totalWeight);
        m_TrackBinding.intensity.value = blendedIntensity + m_DefaultIntensity * (1f-totalWeight);
        m_TrackBinding.scatter.value = blendedScatter + m_DefaultScatter * (1f-totalWeight);
        m_TrackBinding.clamp.value = blendedClamp + m_DefaultClamp * (1f-totalWeight);
        m_TrackBinding.tint.value = blendedTint + m_DefaultTint * (1f-totalWeight);
        m_TrackBinding.highQualityFiltering.value = blendedHighQualityFiltering;
        m_TrackBinding.skipIterations.value = Mathf.RoundToInt(blendedSkipIterations + m_DefaultSkipIterations * (1f-totalWeight));
        m_TrackBinding.dirtTexture.value = blendedDirtTexture;
        m_TrackBinding.dirtIntensity.value = blendedDirtIntensity + m_DefaultDirtIntensity * (1f-totalWeight);

    }



    public override void OnPlayableDestroy (Playable playable)
    {
        m_FirstFrameHappened = false;

        if(m_TrackBinding == null)
            return;

        m_TrackBinding.threshold.value = m_DefaultThreshold;
        m_TrackBinding.intensity.value = m_DefaultIntensity;
        m_TrackBinding.scatter.value = m_DefaultScatter;
        m_TrackBinding.clamp.value = m_DefaultClamp;
        m_TrackBinding.tint.value = m_DefaultTint;
        m_TrackBinding.highQualityFiltering.value = m_DefaultHighQualityFiltering;
        m_TrackBinding.skipIterations.value = m_DefaultSkipIterations;
        m_TrackBinding.dirtTexture.value = m_DefaultDirtTexture;
        m_TrackBinding.dirtIntensity.value = m_DefaultDirtIntensity;

    }
}
