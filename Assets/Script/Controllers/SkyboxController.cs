using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxController : Controller
{
    protected override IEnumerator Apply(Environment environment)
    {
        // fade out
        float startValue = RenderSettings.skybox.GetFloat("_Exposure");
        yield return StartCoroutine(Interpolate(0.25f, startValue, 0.0f, UpdateExposureCallback));


        // set texture
        RenderSettings.skybox.SetFloat("_Rotation", environment.m_worldRotation);
        RenderSettings.skybox.mainTexture = environment.m_background;

        // fade
        startValue = RenderSettings.skybox.GetFloat("_Exposure");
        yield return StartCoroutine(Interpolate(0.25f, startValue, 1.0f, UpdateExposureCallback));

        Debug.Log("Skybox Applied");

    }
    private void UpdateExposureCallback(float value)
    {
        RenderSettings.skybox.SetFloat("_Exposure", value);
    }

}


