using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;
using UnityEngine;

public class XmlParser
{
    /// <summary>
    /// Reads an XML file, and returns a list of objects of a given type with their data members (Properties and Fields) filled in.
    /// </summary>
    /// <typeparam name="T">Type of objects it will parse from the XML file.</typeparam>
    /// <param name="fileName">Filename of the XML file.</param>
    /// <param name="caseSensitive">If true: parse the XML file case sensitive with respect to Property and Field names.</param>
    /// <param name="showDebugLogging">If true: outputs debug logging in the Unity console.</param>
    /// <returns>Returns a list of specified type, read from the XML file.</returns>
    public static List<T> Read<T>(string fileName, bool caseSensitive = false, bool showDebugLogging = false) where T : class, new()
    {
        return ReadInternal<T>(fileName, caseSensitive, showDebugLogging, false, null, "", false, false);
    }

    /// <summary>
    /// Reads an XML file, and calls the downloadComplete function with the result list when it is complete. Use this within a Coroutine.
    /// </summary>
    /// <typeparam name="T">Type of objects it will parse from the XML file.</typeparam>
    /// <param name="url">URL of the XML file.</param>
    /// <param name="downloadComplete">This function gets called when the download and parsing is complete.</param>
    /// <param name="caseSensitive">If true: parse the XML file case sensitive with respect to Property and Field names.</param>
    /// <param name="showDebugLogging">If true: outputs debug logging in the Unity console.</param>
    public static IEnumerator ReadFromUrl<T>(string url, Action<List<T>> downloadComplete, bool caseSensitive = false, bool showDebugLogging = false) where T : class, new()
    {
        if (showDebugLogging)
        {
            Debug.Log("XmlParser: Starting XML file download from [" + url + "]");
        }

        WWW www = new WWW(url);
        yield return www;
        downloadComplete(ReadInternal<T>(www.text, caseSensitive, showDebugLogging, false, null, "", false, true));        
    }

    /// <summary>
    /// Write a list of objects to an XML file.
    /// </summary>
    /// <typeparam name="T">Type of objects it will write to the XML file.</typeparam>
    /// <param name="objects">List of objects it will write to the XML file.</param>
    /// <param name="fileName">Filename of the XML file to write to.</param>
    /// <param name="overwrite">If true: it will overwrite any existing file. If false and file exists, it tries to append to the root element.</param>
    /// <param name="onlyPublic">If true: only write public properties and fields, not internal/private/protected/etc.</param>
    /// <param name="showDebugLogging">If true: outputs debug logging in the Unity console.</param>
    public static void Write<T>(IEnumerable<T> objects, string fileName, bool overwrite, bool onlyPublic = true, bool showDebugLogging = false) where T : class
    {
        if (showDebugLogging)
        {
            Debug.Log("XmlParser: Starting XML file writing for type [" + typeof(T).Name + "] to file [" + fileName + "]");
        }

        XmlDocument xd = new XmlDocument();
        try
        {
            bool exists = File.Exists(fileName);
            XmlElement root = null;
            if (exists && !overwrite)
            {
                xd.Load(fileName);
                root = xd.DocumentElement;
            }
            if (root == null)
            {
                root = xd.CreateElement("Root");
                xd.AppendChild(root);
            }
            string containerName = typeof(T).Name + "List";
            XmlNode container = root.SelectSingleNode(containerName);
            if (container == null)
            {
                container = xd.CreateElement(containerName);
                root.AppendChild(container);
            }

            BindingFlags bf = onlyPublic ? (BindingFlags.Public | BindingFlags.Instance) : (BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (T t in objects)
            {
                XmlElement xe = xd.CreateElement(typeof(T).Name);
                if (showDebugLogging)
                {
                    Debug.Log("Writing new element [" + typeof(T).Name + "]");
                }
                foreach (PropertyInfo pi in typeof(T).GetProperties(bf))
                {
                    object o = pi.GetValue(t, null);
                    if (o != null)
                    {
                        XmlElement prop = xd.CreateElement(pi.Name);
                        prop.InnerText = o.ToString();
                        if (showDebugLogging)
                        {
                            Debug.Log("Writing child node [" + pi.Name + "] with value [" + prop.InnerText + "]");
                        }
                        xe.AppendChild(prop);
                    }
                }
                foreach (FieldInfo fi in typeof(T).GetFields(bf))
                {
                    if (!fi.Name.Contains("k__BackingField"))
                    {
                        object o = fi.GetValue(t);
                        if (o != null)
                        {
                            XmlElement prop = xd.CreateElement(fi.Name);
                            prop.InnerText = o.ToString();
                            if (showDebugLogging)
                            {
                                Debug.Log("Writing child node [" + fi.Name + "] with value [" + prop.InnerText + "]");
                            }
                            xe.AppendChild(prop);
                        }
                    }
                }
                container.AppendChild(xe);
            }

            if (exists)
            {
                File.Delete(fileName);
            }

            xd.Save(fileName);
        }
        catch (Exception e)
        {
            Debug.LogError("XmlParser: Error encountered: " + e.ToString());
        }
        
        if (showDebugLogging)
        {
            Debug.Log("XmlParser: Finished writing XML file.");
        }
    }

    private static List<T> ReadInternal<T>(string fileName, bool caseSensitive, bool showDebugLogging, bool useComponents, GameObject gameObject, string gameObjectName, bool addCounter, bool filenameIsContents) where T : class, new()
    {
        if (showDebugLogging)
        {
            Debug.Log("XmlParser: Starting XML file reading for type [" + typeof(T).Name + "] from " + (filenameIsContents ? "downloaded data" : "file [" + fileName + "]"));
        }

        List<T> l = new List<T>();

        XmlTextReader xr = null;

        BindingFlags bf = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        if (!caseSensitive)
        {
            bf |= BindingFlags.IgnoreCase;
        }

        try
        {
            if (filenameIsContents)
            {
                xr = new XmlTextReader(new StringReader(fileName));
            }
            else
            {
                UnityEngine.Object o = Resources.Load(fileName);

                if (o != null)
                {
                    xr = new XmlTextReader(new StringReader(((TextAsset)o).text));
                }
                else
                {
                    xr = new XmlTextReader(fileName);
                }
            }
            GameObject go = gameObject;
            int i = 1;
            T t = null;
            PropertyInfo pi = null;
            FieldInfo fi = null;
            bool insideObject = false;

            while (xr.Read())
            {
                switch (xr.NodeType)
                {
                    case XmlNodeType.Element:
                    {
                        if (showDebugLogging)
                        {
                            Debug.Log("XmlParser: Parsing <" + xr.LocalName + ">");
                        }
                        if (xr.LocalName == typeof(T).Name)
                        {
                            if (useComponents)
                            {
                                if (gameObject == null)
                                {
                                    go = new GameObject();
                                    go.name = gameObjectName + (addCounter ? " " + i : "");
                                    ++i;
                                }
                                t = (T)(object)go.AddComponent(typeof(T));
                            }
                            else
                            {
                                t = new T();
                            }
                            l.Add(t);
                            insideObject = !xr.IsEmptyElement;

                            if (xr.MoveToFirstAttribute())
                            {
                                if (showDebugLogging)
                                {
                                    Debug.Log("XmlParser: Parsing @ <" + xr.LocalName + ">");
                                }

                                UpdatePiFi<T>(caseSensitive, xr, bf, ref pi, ref fi);
                                SetValue(xr, t, pi, fi);
                                
                                while (xr.MoveToNextAttribute())
                                {
                                    if (showDebugLogging)
                                    {
                                        Debug.Log("XmlParser: Parsing @ <" + xr.LocalName + ">");
                                    }

                                    UpdatePiFi<T>(caseSensitive, xr, bf, ref pi, ref fi);
                                    SetValue(xr, t, pi, fi);
                                }
                            }

                            if (!insideObject)
                            {
                                t = null;
                                pi = null;
                                fi = null;
                                insideObject = false;
                            }
                        }
                        else if (insideObject)
                        {
                            UpdatePiFi<T>(caseSensitive, xr, bf, ref pi, ref fi);
                        }
                        break;
                    }
                    case XmlNodeType.EndElement:
                    {
                        if (showDebugLogging)
                        {
                            Debug.Log("XmlParser: Parsing </" + xr.LocalName + ">");
                        }
                        if (xr.LocalName == typeof(T).Name)
                        {
                            t = null;
                            pi = null;
                            fi = null;
                            insideObject = false;
                        }
                        break;
                    }
                    case XmlNodeType.Text:
                    case XmlNodeType.CDATA:
                    {
                        if (showDebugLogging)
                        {
                            Debug.Log("XmlParser: Parsing [" + xr.Value + "]");
                        }
                        SetValue<T>(xr, t, pi, fi);
                        break;
                    }
                    default:
                    {
                        break;
                    }
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("XmlParserXml: Error encountered: " + e.ToString());
        }
        finally
        {
            if (xr != null)
            {
                xr.Close();
            }
        }

        if (showDebugLogging)
        {
            Debug.Log("XmlParser: Finished reading XML file. Objects returned: " + l.Count);
        }

        return l;
    }

    private static void UpdatePiFi<T>(bool caseSensitive, XmlTextReader xr, BindingFlags bf, ref PropertyInfo pi, ref FieldInfo fi) where T : class, new()
    {
        pi = typeof(T).GetProperty(xr.LocalName, bf);
        fi = typeof(T).GetField(xr.LocalName, bf);
        if (pi == null && fi == null)
        {
            Debug.LogWarning("XmlParser: Type [" + typeof(T).Name + "] does not contain a Property or a Field with name [" + xr.LocalName + "], perhaps a spelling error" + (caseSensitive ? " (or case sensitivity error)" : ""));
        }
    }

    private static void SetValue<T>(XmlTextReader xr, T t, PropertyInfo pi, FieldInfo fi) where T : class, new()
    {
        if (pi != null)
        {
            if (pi.PropertyType == typeof(string))
            {
                pi.SetValue(t, xr.Value, null);
            }
            else if (pi.PropertyType == typeof(Int32))
            {
                pi.SetValue(t, Convert.ToInt32(xr.Value), null);
            }
            else if (pi.PropertyType == typeof(bool))
            {
                pi.SetValue(t, Convert.ToBoolean(xr.Value), null);
            }
            else if (pi.PropertyType == typeof(Single))
            {
                pi.SetValue(t, Convert.ToSingle(xr.Value), null);
            }
            else if (pi.PropertyType == typeof(Int16))
            {
                pi.SetValue(t, Convert.ToInt16(xr.Value), null);
            }
            else if (pi.PropertyType == typeof(Int64))
            {
                pi.SetValue(t, Convert.ToInt64(xr.Value), null);
            }
            else if (pi.PropertyType == typeof(UInt16))
            {
                pi.SetValue(t, Convert.ToUInt16(xr.Value), null);
            }
            else if (pi.PropertyType == typeof(UInt32))
            {
                pi.SetValue(t, Convert.ToUInt32(xr.Value), null);
            }
            else if (pi.PropertyType == typeof(UInt64))
            {
                pi.SetValue(t, Convert.ToUInt64(xr.Value), null);
            }
            else if (pi.PropertyType == typeof(byte))
            {
                pi.SetValue(t, Convert.ToByte(xr.Value), null);
            }
            else if (pi.PropertyType == typeof(char))
            {
                pi.SetValue(t, Convert.ToChar(xr.Value), null);
            }
            else if (pi.PropertyType == typeof(DateTime))
            {
                pi.SetValue(t, Convert.ToDateTime(xr.Value), null);
            }
            else if (pi.PropertyType == typeof(decimal))
            {
                pi.SetValue(t, Convert.ToDecimal(xr.Value), null);
            }
            else if (pi.PropertyType == typeof(double))
            {
                pi.SetValue(t, Convert.ToDouble(xr.Value), null);
            }
            else if (pi.PropertyType == typeof(sbyte))
            {
                pi.SetValue(t, Convert.ToSByte(xr.Value), null);
            }
            else
            {
                Debug.LogError("XmlParser: Property of type [" + pi.PropertyType.ToString() + "] not supported");
            }
        }
        if (fi != null)
        {
            if (fi.FieldType == typeof(string))
            {
                fi.SetValue(t, xr.Value);
            }
            else if (fi.FieldType == typeof(Int32))
            {
                fi.SetValue(t, Convert.ToInt32(xr.Value));
            }
            else if (fi.FieldType == typeof(bool))
            {
                fi.SetValue(t, Convert.ToBoolean(xr.Value));
            }
            else if (fi.FieldType == typeof(Single))
            {
                fi.SetValue(t, Convert.ToSingle(xr.Value));
            }
            else if (fi.FieldType == typeof(Int16))
            {
                fi.SetValue(t, Convert.ToInt16(xr.Value));
            }
            else if (fi.FieldType == typeof(Int64))
            {
                fi.SetValue(t, Convert.ToInt64(xr.Value));
            }
            else if (fi.FieldType == typeof(UInt16))
            {
                fi.SetValue(t, Convert.ToUInt16(xr.Value));
            }
            else if (fi.FieldType == typeof(UInt32))
            {
                fi.SetValue(t, Convert.ToUInt32(xr.Value));
            }
            else if (fi.FieldType == typeof(UInt64))
            {
                fi.SetValue(t, Convert.ToUInt64(xr.Value));
            }
            else if (fi.FieldType == typeof(byte))
            {
                fi.SetValue(t, Convert.ToByte(xr.Value));
            }
            else if (fi.FieldType == typeof(char))
            {
                fi.SetValue(t, Convert.ToChar(xr.Value));
            }
            else if (fi.FieldType == typeof(DateTime))
            {
                fi.SetValue(t, Convert.ToDateTime(xr.Value));
            }
            else if (fi.FieldType == typeof(decimal))
            {
                fi.SetValue(t, Convert.ToDecimal(xr.Value));
            }
            else if (fi.FieldType == typeof(double))
            {
                fi.SetValue(t, Convert.ToDouble(xr.Value));
            }
            else if (fi.FieldType == typeof(sbyte))
            {
                fi.SetValue(t, Convert.ToSByte(xr.Value));
            }
            else
            {
                Debug.LogError("XmlParser: Field of type [" + fi.FieldType.ToString() + "] not supported");
            }
        }
    }
}