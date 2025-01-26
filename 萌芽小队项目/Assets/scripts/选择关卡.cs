using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class 选择关卡 : MonoBehaviour
{
    public void SelectScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
