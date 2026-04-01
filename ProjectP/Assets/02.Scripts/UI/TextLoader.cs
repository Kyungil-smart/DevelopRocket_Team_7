using UnityEngine;
using TMPro;

public class TextLoader : MonoBehaviour
{
    [SerializeField] private int textId;
    private TextMeshProUGUI _textGui;
    
    public void OnEnable()
    {
        if (textId == 0)
        {
            _textGui.text = "textId 에 올바른 ID 를 넣으세요";
            return;
        }
        _textGui.text = PostManager.Instance.Request<int, string>(PostMessageKey.UITextReqeust, textId);
    }
}