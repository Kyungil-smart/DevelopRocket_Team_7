using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
[CreateAssetMenu(menuName = "Localization/English", fileName = "English")]
public class EnglishSO : BaseDataSO
{
    private SheetLoader<English> data;
    [SerializeField] public List<English> Ldata = new List<English>();
    public override async Task InitAsync()
    {
        data = new SheetLoader<English>(url, gid);

        // 2. 데이터가 다 로드될 때까지 기다렸다가(await) 리스트를 받아옵니다.
        // GetDataAsync()의 반환 타입이 Task<List<charData>>이므로 await가 필수입니다.
        Ldata = await data.GetDataAsync();
    }
}
