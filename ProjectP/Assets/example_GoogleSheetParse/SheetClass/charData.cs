using UnityEngine;
  
[System.Serializable]
public class charData :ISheetParsable, IIdentifiable
{
    public int id;
    [field: SerializeField] public string Name { get; set; }
    public float HP;
    public int Attack;
    
    public void ApplyRowData(string [] Data)
    {
      
        this.id = int.Parse(Data[0]);
        this.Name = Data[1];
        this.HP = float.Parse(Data[2]);
        this.Attack = int.Parse(Data[3]);

        
    }
}
