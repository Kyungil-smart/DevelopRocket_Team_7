using System;
using UnityEngine;

public class NodeUISoundManager : MonoBehaviour
{
    // 노드 클릭시 출력할 사운드
    public AudioClip ShareClickClip;
    // 노드 레벨업시 출력할 사운드
    public AudioClip ShareNodeLevelUpClip;

    // 노드 레벨업 시도 횟수
    private int _levelUpCount;

    private void Awake()
    {
        _levelUpCount = 0;
    }

    private void OnEnable()
    {
        PostManager.Instance.Subscribe<int>(PostMessageKey.NodeLevelUp,PlayLevelUpClip);
    }

    // 노드 레벨업 시 출력할 클립
    private void PlayLevelUpClip(int levelUpCount)
    {
        // 기존 레벨업 로직이 3번 시도후 실제 레벨업 진행으로 구현하였음
        // 위 방식과 비슷하게 구현
        _levelUpCount += levelUpCount;

        if (_levelUpCount >= 3)
        {
            AudioManager.Instance.OnSfxPlayOnShot(ShareNodeLevelUpClip);
            _levelUpCount = 0;
        }
    }

    private void OnDisable()
    {
        PostManager.Instance.Unsubscribe<int>(PostMessageKey.NodeLevelUp,PlayLevelUpClip);
    }
}
