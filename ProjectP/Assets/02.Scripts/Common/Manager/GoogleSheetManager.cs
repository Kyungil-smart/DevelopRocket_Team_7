using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;
public class GoogleSheetManager : Singleton<GoogleSheetManager>
{
    public List<BaseDataSO> m_Listdata = new List<BaseDataSO>();
    public BaseDataSO m_datasss;

     
    async void Awake()
    {
        base.Awake();
        List<Task> tasks = new List<Task>();
        foreach (var item in m_Listdata)
        {
            tasks.Add(item.InitAsync());
        }
        await Task.WhenAll(tasks);
    }
    private void Start()
    {
        m_datasss = GetData<Sample_Google_Sheet_SO_Class>();
    }
    public BaseDataSO GetData<T>() where T : BaseDataSO
    {
        foreach (var item in m_Listdata)
        {
            bool isSameType = item.GetType() == typeof(T);
            if (isSameType)
            {
                return item;
            }
        }
        return null;
    }
}
