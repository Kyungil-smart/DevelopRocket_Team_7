using UnityEngine;
using TMPro;

public class TextLoader : MonoBehaviour
{
    [SerializeField] private int textId;
    private TextMeshProUGUI _textGui;

    private void Awake()
    {
        _textGui = GetComponent<TextMeshProUGUI>();
    }
    
    private void OnEnable()
    {
        ChangeText(true);
        PostManager.Instance.Subscribe<bool>(PostMessageKey.RequestChangeText, ChangeText);
    }

    private void OnDisable()
    {
        PostManager.Instance.Unsubscribe<bool>(PostMessageKey.RequestChangeText, ChangeText);
    }

    private void ChangeText(bool dummy)
    {
        if (textId == 0)
        {
            _textGui.text = "";
            return;
        }
        if (_textGui == null) _textGui = GetComponent<TextMeshProUGUI>();

        try
        {
            _textGui.text = PostManager.Instance.Request<int, string>(PostMessageKey.UITextReqeust, textId);
        }
        catch
        {
            Debug.Log($"{textId} has not {PostMessageKey.UITextReqeust} text.");
            _textGui.text = "NotFound";
        }
        
    }

    public void SetTextId(int textId)
    {
        this.textId = textId;
        ChangeText(true);
    }
}