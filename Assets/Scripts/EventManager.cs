using System;//需要 System
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public delegate void CallBack();//需要有参数的直接在这加上需要的参数就可以实现参数广播。
public enum EventTypee//广播事件
{
    test,
    None,
    GetDoubleJump,
    GetDash,
    GetDark,
    BGMChange1,
    BGMChange2
}
public class EventManager : MonoBehaviour
{
    private static Dictionary<EventTypee, Delegate> dic = new Dictionary<EventTypee, Delegate>();
    /// <summary>
    /// 添加事件监听
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="callBack"></param>
    public static void AddListener(EventTypee eventTypee, CallBack callBack)
    {
        if (!dic.ContainsKey(eventTypee))
        {
            dic.Add(eventTypee, null);
        }
        //异常抛出
        Delegate m = dic[eventTypee];
        if (m != null && m.GetType() != callBack.GetType())
        {
            throw new Exception(string.Format("事件{0}为不同委托，对应为{1}，添加类型为{2}", eventTypee, m.GetType(), callBack.GetType()));
        }
        dic[eventTypee] = (CallBack)dic[eventTypee] + callBack;

    }
    /// <summary>
    /// 移除监听事件
    /// </summary>
    /// <param name="eventTypee"></param>
    /// <param name="callBack"></param>
    public static void RemoveListener(EventTypee eventTypee, CallBack callBack)
    {
        //异常抛出
        if (dic.ContainsKey(eventTypee))
        {
            Delegate m = dic[eventTypee];
            if (m == null)
            {
                throw new Exception(string.Format("错误；事件{0}没有添加", eventTypee));
            }
            else if (m.GetType() != callBack.GetType())
            {
                throw new Exception(string.Format("移除错误，事件{0}于事件{1}为不同委托", m.GetType(), callBack.GetType()));
            }

        }
        else
        {
            throw new Exception(string.Format("错误，没有事件{0}", eventTypee));
        }
        dic[eventTypee] = (CallBack)dic[eventTypee] - callBack;
        if (dic[eventTypee] == null)
        {
            dic.Remove(eventTypee);
        }
    }
    /// <summary>
    /// 事件的广播
    /// </summary>
    /// <param name="eventTypee"></param>
    public static void Broadcast(EventTypee eventTypee)
    {
        Delegate m;
        if (dic.TryGetValue(eventTypee, out m))
        {
            CallBack callBack = m as CallBack;//强制转换失败时，会返回 null
            if (callBack != null)
            {
                callBack();
            }
            else
            {
                throw new Exception(string.Format("错误，事件已存在", eventTypee));
            }
        }
    }
}