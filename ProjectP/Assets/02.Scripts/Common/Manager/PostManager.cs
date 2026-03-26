using System;
using System.Collections.Generic;
using UnityEngine;

class PostMessages<T> : IPostMessages
{
    public Action<T> actions;
}

/// <summary>
/// 객체간 Data Communication 을 위한 중앙 관리 매니저
/// </summary>
public class PostManager : Singleton<PostManager>
{
    private Dictionary<PostMessageKey, IPostMessages> _subscribes = new ();
    
    /// <summary>
    /// 함수 구독  
    /// </summary>
    /// <param name="key">데이터 키</param>
    /// <param name="callback">Callback 함수</param>
    /// <typeparam name="T">데이터 타입</typeparam>
    public void Subscribe<T>(PostMessageKey key, Action<T> callback)
    {
        if (!_subscribes.TryGetValue(key, out var postMessages))
        {
            postMessages = new PostMessages<T>();
            _subscribes[key] = postMessages;
        }

        if (postMessages is PostMessages<T> pm)
        {
            pm.actions += callback;
        }
    }
    /// <summary>
    /// 함수 구취
    /// </summary>
    /// <param name="key">데이터 키</param>
    /// <param name="callback">Callback 함수</param>
    /// <typeparam name="T">데이터 타입</typeparam>
    public void Unsubscribe<T>(PostMessageKey key, Action<T> callback)
    {
        if (_subscribes.TryGetValue(key, out var postMessages) && postMessages is PostMessages<T> pm)
            pm.actions -= callback;
    }

    /// <summary>
    /// 데이터 변경 알림
    /// </summary>
    /// <param name="key">데이터 키</param>
    /// <param name="data">변경된 데이터</param>
    /// <typeparam name="T">데이터 타입</typeparam>
    public void Post<T>(PostMessageKey key, T data)
    {
        if (!_subscribes.TryGetValue(key, out var postMessages))
        {
            postMessages = new PostMessages<T>();
            _subscribes[key] = postMessages;
        }
        (postMessages as PostMessages<T>)?.actions.Invoke(data);
    }
}