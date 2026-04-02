using UnityEngine;
[System.Serializable]
public class English : ISheetParsable, IIdentifiable
{
    public string Description;
    [field: SerializeField] public string Name { get; set; }
    public void ApplyRowData(string[] Data)
    {
        Description = Data[0];
        Name = Data[1];
    }
}
