using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
[CreateAssetMenu(menuName ="chardata",fileName ="test")]
public class Char2Data : BaseDataSO
{
    private SheetLoader<Attackinfo> data;
    [SerializeField] public List<Attackinfo> Ldata = new List<Attackinfo>();
    public override async Task InitAsync()
    {
        data = new SheetLoader<Attackinfo>(url, gid);

        // 2. 데이터가 다 로드될 때까지 기다렸다가(await) 리스트를 받아옵니다.
        // GetDataAsync()의 반환 타입이 Task<List<charData>>이므로 await가 필수입니다.
        Ldata = await data.GetDataAsync();
    }
}
