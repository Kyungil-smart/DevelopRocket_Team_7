using UnityEngine;
[System.Serializable]
public class Attackinfo : ISheetParsable, IIdentifiable
{
    public int normalAttack;// 일반 공격
    public int enhancedattack;//강화 공격
    public int SkillAttack; //스킬공격
    [field: SerializeField] public string Name { get; set; }
    public void ApplyRowData(string[] Data)
    {
        normalAttack=int.Parse(Data[0]);
        enhancedattack= int.Parse(Data[1]);
        SkillAttack= int.Parse(Data[2]);
        Name = Data[3];

    }
}
