using UnityEngine;
using TMPro;

public class TextLoader : MonoBehaviour
{
    [SerializeField] private int textId;
    private TextMeshProUGUI _textGui;

    private void Awake()
    {
        _textGui = GetComponentInChildren<TextMeshProUGUI>();
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
        _textGui.text = PostManager.Instance.Request<int, string>(PostMessageKey.UITextReqeust, textId);
    }

    public void SetTextId(int textId)
    {
        this.textId = textId;
        ChangeText(true);
    }
}