using System;
using UnityEngine.Networking;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public class UIDataManager : MonoBehaviour
{
    [Header("Gsheet Info")] 
    [SerializeField] private string _url;
    [SerializeField] private List<int> _gids;
    private TaskCompletionSource<bool> _loadTaskSource = new ();
    
    [SerializeField] private LanguageType _languageType;
    [SerializeField] private TextData _textDataSO;
    private Dictionary<int, Dictionary<LanguageType, string>> _textData = new();

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        ConvertData();
        PostManager.Instance.Subscribe<int, string>(PostMessageKey.UITextReqeust, SearchText);
        PostManager.Instance.Subscribe<LanguageType>(PostMessageKey.ChangeLanguage, ChangeLanguage);
    }

    private void OnDisable()
    {
        PostManager.Instance.Unsubscribe<int, string>(PostMessageKey.UITextReqeust, SearchText);
        PostManager.Instance.Unsubscribe<LanguageType>(PostMessageKey.ChangeLanguage, ChangeLanguage);
    }

    private void ChangeLanguage(LanguageType languageType)
    {
        _languageType = languageType;
        PostManager.Instance.Post(PostMessageKey.RequestChangeText, true);
    }
    private string SearchText(int textId)
    {
        if (_textData == null) return "NotFound";
        if (_textData.TryGetValue(textId, out var value))
        {
            if (value.TryGetValue(_languageType, out var text))
                return text;    
            Debug.LogError($"{textId} has not {_languageType} text.");
        }
        return "NotFound";
    }

    private void CreateTextDataAsset()
    {
        Debug.Log("SaveAssets");
#if UNITY_EDITOR
        EditorUtility.SetDirty(_textDataSO);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
#endif
    }
    
    private void ConvertData()
    {   // SO to Dict
        if (_textDataSO)
        {
            foreach (var line in _textDataSO.texts)
            {
                if (!_textData.ContainsKey(line.textId))
                    _textData[line.textId] = new();
                
                _textData[line.textId].Add(line.languageType, line.text);
            }
        }
    }
    
    public void NotifyLoadComplete()
    {
        _loadTaskSource.TrySetResult(true);
    }

    private async Task LoadDataFromSheet()
    {
        _textDataSO.texts.Clear();
        Debug.Log("Gsheet 에서 데이터 가져오기");
        for (int i = 0; i < _gids.Count; i++)
        {
            UnityWebRequest request = UnityWebRequest.Get(ConvertToDownloadUrl(_url, _gids[i].ToString()));
            var operation = request.SendWebRequest();
            while (!operation.isDone)
            {
                await Task.Yield(); // 다음 프레임까지 양보
            }

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(request.error);
            }
            else
            {
                string csv = request.downloadHandler.text;
                ParseCSV(csv, i);
                NotifyLoadComplete();
            }
        }
    }
    
    private string ConvertToDownloadUrl(string url, string gid = "0")
    {
        // 1. "/edit" 및 그 뒤에 오는 모든 내용을 제거
        // 2. "/export?format=csv"를 붙임
        // 3. 특정 시트(gid)를 지정하고 싶다면 "&gid=xxx" 추가

        string pattern = @"/edit.*";
        string replacement = $"/export?format=csv&gid={gid}";

        if (Regex.IsMatch(url, pattern))
        {
            return Regex.Replace(url, pattern, replacement);
        }
        return url; // 패턴이 없으면 그대로 반환
    }
    
    private void ParseCSV(string csv, int lId)
    {
        Debug.Log("CSV Parsing 시작");
        string[] lines = csv.Split(new[] { "\r\n", "\n" }, System.StringSplitOptions.RemoveEmptyEntries);
      
        for (int i = 1; i < lines.Length; i++) // 0번은 헤더
        {
            string[] temp = lines[i].Split(',');
            _textDataSO.texts.Add(new TextLine()
            {
                textId = int.Parse(temp[1]),
                languageType = (LanguageType)lId,
                text = temp[0]
            });
        }
    }

    [ContextMenu("Load Data From Sheet")]
    public async Task OnLoadDataFromSheet()
    {
        await LoadDataFromSheet();
        CreateTextDataAsset();
        ConvertData();
    }

    [ContextMenu("Test/CheckData")]
    public void OnTestCheckData()
    {
        foreach (var text in _textData)
        {
            foreach (var line in text.Value)
            {
                Debug.Log($"{text.Key}: {line.Key}: {line.Value}");
            }
        }
    }
    
    [ContextMenu("Test/Trigger Change Language")]
    public void OnTriggerChangeLanguage()
    {
        ChangeLanguage(_languageType);
    }
}