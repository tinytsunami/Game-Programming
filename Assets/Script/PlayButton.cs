﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    public void OnButtonClick(){
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
}
