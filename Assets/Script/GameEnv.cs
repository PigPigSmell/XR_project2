using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEnv : MonoBehaviour
{
    public SkyboxController m_skyboxController;
    public List<Environment> m_envs = null;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            m_skyboxController.NewEnvironment(m_envs[1]);
        }
    }
}

[Serializable]
public class Environment
{
    public int m_worldRotation = 0;
    public Texture m_background = null;
    public AudioClip m_ambientNoise = null;
}