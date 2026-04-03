using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : Singleton<SceneChanger>
{
    public void ChangeScene(int sceneId) => SceneManager.LoadScene(sceneId);
    public void ChangeScene(string sceneName) => SceneManager.LoadScene(sceneName);
}