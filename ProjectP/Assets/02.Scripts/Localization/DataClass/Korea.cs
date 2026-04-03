using UnityEngine;
[System.Serializable]
public class Korea: ISheetParsable, IIdentifiable
{
    public string Description;
    [field: SerializeField] public string Name { get; set; }
    public void ApplyRowData(string[] Data)
    {
        Description = Data[0];
        Name = Data[1];
    }
}
