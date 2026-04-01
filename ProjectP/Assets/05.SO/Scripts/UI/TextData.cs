using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct TextLine
{
    public int textId;
    public LanguageType languageType;
    public string text;
}


[CreateAssetMenu(menuName = "Data/UIText")]
public class TextData : ScriptableObject
{
    public List<TextLine> texts = new();
}