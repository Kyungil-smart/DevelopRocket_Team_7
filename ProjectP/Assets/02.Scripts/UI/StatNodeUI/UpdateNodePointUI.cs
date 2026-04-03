using UnityEngine;
using TMPro;

public class UpdateNodePointUI : MonoBehaviour
{
    private TextMeshProUGUI _label;

    private void Awake()
    {
        _label = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        PostManager.Instance.Subscribe<string>(PostMessageKey.NodePointTextUIUpdate,UpdateText);
        _label.text = StatNodeManager.Instance.NodePoint.ToString();
    }

    private void UpdateText(string pointText)
    {
        _label.text = pointText;
    }

    private void OnDisable()
    {
        PostManager.Instance.Unsubscribe<string>(PostMessageKey.NodePointTextUIUpdate,UpdateText);
    }
}
