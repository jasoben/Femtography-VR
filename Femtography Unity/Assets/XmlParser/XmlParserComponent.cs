using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using UnityEngine;

class XmlParserComponent : MonoBehaviour
{
    public string FileName = "";
    public string ComponentName = "";
    public bool WebURL = false;
    public bool CaseSensitive = true;
    public bool ShowDebugLogging = false;
    public bool CreateNewGameObjectForEachElement = false;
    public string NewGameObjectName = "NewGameObject";
    public bool AddCounterToNewGameObjectName = true;

    void Start()
    {
        try
        {
            Type t = Type.GetType(ComponentName, false, !CaseSensitive);
            if (t == null)
            {
                Debug.LogError("XmlParser: Component [" + ComponentName + "] not found, perhaps a spelling error" + (CaseSensitive ? " (or case sensitivity error)" : ""));
            }
            else
            {
                if (WebURL)
                {
                    StartCoroutine(ParseFromUrl(t));
                }
                else
                {
                    typeof(XmlParser).GetMethod("ReadInternal", BindingFlags.NonPublic | BindingFlags.Static).MakeGenericMethod(new Type[] { t }).Invoke(null, new object[] { FileName, CaseSensitive, ShowDebugLogging, true, CreateNewGameObjectForEachElement ? null : gameObject, NewGameObjectName, AddCounterToNewGameObjectName, WebURL });
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("XmlParser: Error encountered: " + e.ToString());
        }
    }

    private IEnumerator ParseFromUrl(Type t)
    {
        WWW www = new WWW(FileName);
        yield return www;
        typeof(XmlParser).GetMethod("ReadInternal", BindingFlags.NonPublic | BindingFlags.Static).MakeGenericMethod(new Type[] { t }).Invoke(null, new object[] { www.text, CaseSensitive, ShowDebugLogging, true, CreateNewGameObjectForEachElement ? null : gameObject, NewGameObjectName, AddCounterToNewGameObjectName, WebURL });
    }
}