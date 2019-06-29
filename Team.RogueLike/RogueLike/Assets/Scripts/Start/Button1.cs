using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button1 : MonoBehaviour
{
    public string nextScene;
    public void OnClick()
    {
        FadeSceneManager.FadeOut(nextScene);
    }
}
