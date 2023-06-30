using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class MainMenu : MonoBehaviour
{
    public VectorValue position;
    public FlagValue missions;
    public CodeValue code;

    public void PlayGame()
    {
        position.initialValue.x = 1;
        position.initialValue.y = 0;
        position.initialValue.z = 0;
        missions.bullsFlag = false;
        missions.checkersFlag = false;
        missions.mathFlag = false;
        missions.pathFlag = false;
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void LoadGame()
    {
        position.initialValue.x = PlayerPrefs.GetFloat("xPos");
        position.initialValue.y = PlayerPrefs.GetFloat("yPos");
        position.initialValue.z = 0;
        code.code = PlayerPrefs.GetString("code");
        SceneManager.LoadScene(PlayerPrefs.GetInt("Current scene"));
        missions.bullsFlag = Convert.ToBoolean(PlayerPrefs.GetInt("bulls"));
        missions.mathFlag = Convert.ToBoolean(PlayerPrefs.GetInt("math"));
        missions.pathFlag = Convert.ToBoolean(PlayerPrefs.GetInt("path"));
        missions.checkersFlag = Convert.ToBoolean(PlayerPrefs.GetInt("checkers"));
    }
}
