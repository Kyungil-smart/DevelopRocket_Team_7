using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : Singleton<SceneChanger>
{
    // 임시 코드. 테스트 완료 후에는 삭제 예정.
    [Header("아래는 임시로 넣은 에셋. 테스트 후 삭제 예정")]
    [SerializeField] private SceneAsset InGameScene;
    
    public void ChangeScene(int sceneId)
    {
        if (InGameScene == null) SceneManager.LoadScene(sceneId);
        SceneManager.LoadScene(InGameScene.name);
    }
    
    public void ChangeScene(string sceneName)
    {
        if (InGameScene == null) SceneManager.LoadScene(sceneName);
        SceneManager.LoadScene(InGameScene.name);
    }
}