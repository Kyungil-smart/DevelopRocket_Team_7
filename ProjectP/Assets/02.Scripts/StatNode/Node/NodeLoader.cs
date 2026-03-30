using System;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
public class NodeLoader : MonoBehaviour
{
    [Header("불러올 시트 링크 전체 입력")]
    [SerializeField] private string _url;
    
    [Header("사이트 URL 끝에 gid 번호 입력")]//기본 첫 시트는 0 인거 같음 그래도 확인 필수
    [SerializeField] private int _gid;

    [Header("스텟 노드 데이터를 저장할 타겟 SO 연결")] 
    [SerializeField] private NodeDataSO _dataContainer;
    
    [Header("특수 노드 데이터를 저장할 타겟 SO 연결")] 
    [SerializeField] private NodeDataSO _specialDataContainer;
    
    [Header("UI 스크립트 연결")] 
    [SerializeField] private StatTreeView _treeView; // UI 스크립트 연결
    
    private SheetLoader<NodeInfo> _data;
    private SheetLoader<SpecialNodeInfo> _specialData;
   
    async void Start()
    {
        // 1. 로더 생성
        _data = new SheetLoader<NodeInfo>(_url, _gid);
        
        // 2. 데이터가 다 로드될 때까지 기다렸다가(await) 리스트를 받아옵니다.
        // GetDataAsync()의 반환 타입이 Task<List<charData>>이므로 await가 필수입니다.
        List<NodeInfo> loadedList = await _data.GetDataAsync();
        List<SpecialNodeInfo> loadedSpecialList = await _specialData.GetDataAsync();
        
        // SO의 리스트를 비운 후 새로 받아온 데이터를 채움
        if (_dataContainer != null)
        {
            _dataContainer.NodeInfos.Clear();
            _dataContainer.NodeInfos.AddRange(loadedList);
        }

        if (_specialDataContainer != null)
        {
            _specialDataContainer.SpecialNodeInfos.Clear();
            _specialDataContainer.SpecialNodeInfos.AddRange(loadedSpecialList);
        }
        
        // 트리 그래프 에셋에 직접 접근하여 노드를 찾아 init 실행
        if (_treeView != null && _treeView.statGraph != null)
        {
            foreach (var node in _treeView.statGraph.nodes)
            {
                // 노드가 StatNode 타입인 경우에만 초기화 실행
                if (node is StatNode statNode)
                {
                    statNode.InitData();
                }
            }
        }
        
        // 불러온 후 UI 새로고침
        if (_treeView != null)
        {
            _treeView.GenerateTree();
        }
        
    }

}
