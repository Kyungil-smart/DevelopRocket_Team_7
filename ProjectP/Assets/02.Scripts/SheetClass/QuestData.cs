using UnityEngine;
[System.Serializable]
public class QuestData : ISheetParsable, IIdentifiable
{
    public int id;
    public string Descritction;
    [field: SerializeField] public string Name { get; set; }
    public void ApplyRowData(string[] Data)
    {

        this.id = int.Parse(Data[0]);
        this.Name = Data[1];
        this.Descritction = Data[2];
       


    }
}
