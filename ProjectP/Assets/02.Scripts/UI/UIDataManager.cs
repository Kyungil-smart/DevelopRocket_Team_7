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
    private Dictionary<int, string[]> _rawData = new();
    private TaskCompletionSource<bool> _loadTaskSource = new ();
    
    [SerializeField] private LanguageType _languageType;
    private TextData _textDataSO;
    private Dictionary<int, Dictionary<LanguageType, string>> _textData = new();

    private void Awake()
    {
        if (_textDataSO != null) _textDataSO = ScriptableObject.CreateInstance<TextData>();
        // string path = "Assets/05.SO/Datas/UI/TextData.asset";
    }
    
    private void OnEnable()
    {
        ConvertData();
        PostManager.Instance.Subscribe<int, string>(PostMessageKey.UITextReqeust, SearchText);
    }

    private void OnDisable()
    {
        PostManager.Instance.Unsubscribe<int, string>(PostMessageKey.UITextReqeust, SearchText);
    }

    private string SearchText(int textId)
    {
        return _textData[textId][_languageType];
    }
    
    private void ConvertData()
    {   // SO to Dict
        
    }

    private void LoadDataFromSheet()
    {
        
    }
    
    public void NotifyLoadComplete()
    {
        _loadTaskSource.TrySetResult(true);
    }

    private async void RequestSheet()
    {
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
        // 줄 분리
        string[] lines = csv.Split(new[] { "\r\n", "\n" }, System.StringSplitOptions.RemoveEmptyEntries);
      
        for (int i = 1; i < lines.Length; i++) // 0번은 헤더
        {
            string[] temp = lines[i].Split(',');
            // temp[0] ; text
            // temp[1] ; id
            
            _rawData.Add(i, lines[i].Split(','));
        }
    }
}