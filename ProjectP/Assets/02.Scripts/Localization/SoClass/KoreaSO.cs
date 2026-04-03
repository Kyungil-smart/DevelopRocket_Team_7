using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
[CreateAssetMenu(menuName = "Localization/Korea", fileName = "Korea")]
public class KoreaSO : BaseDataSO
{
    private SheetLoader<Korea> data;
    [SerializeField]public List<Korea> m_data = new List<Korea>();
    public override async Task InitAsync()
    {
        data = new SheetLoader<Korea>(url, gid);

        // 2. 데이터가 다 로드될 때까지 기다렸다가(await) 리스트를 받아옵니다.
        // GetDataAsync()의 반환 타입이 Task<List<T>>이므로 await가 필수입니다.
        m_data = await data.GetDataAsync();
    }
}
