using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEnv : MonoBehaviour
{
    public SkyboxController m_skyboxController;
    public List<Environment> m_envs = null;
    public int env_index = 0;

    private void Start()
    {
        m_skyboxController.NewEnvironment(m_envs[GameToJson.Data.gameIdx]);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            if(env_index +1 < m_envs.Count)
            {
                env_index += 1;
            } else
            {
                env_index = 0;
            }
            m_skyboxController.NewEnvironment(m_envs[env_index]);
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