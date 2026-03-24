using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;
public class DataManager : MonoBehaviour
{
    public List<BaseDataSO> m_Listdata = new List<BaseDataSO>();
    public BaseDataSO m_datasss;

    public static DataManager instance;
    async void Awake()
    {
        List<Task> tasks = new List<Task>();
        foreach (var item in m_Listdata)
        {
            tasks.Add(item.InitAsync());
        }
        await Task.WhenAll(tasks);
    }
    private void Start()
    {
        m_datasss = GetData<Char2Data>();
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
