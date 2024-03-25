using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;
using System.Globalization;
using System.Threading;
using UnityEngine.Networking;

public static class _CSVOnlineReader
{
    private static readonly char[] TRIM_CHARS = { '\"', '\r' };
    private const string SPLIT_RE = ",";
    private const string LINE_SPLIT_RE = @"\n|\r";
    private const string comma = "\\n";

    public static List<Dictionary<string, string>> ReadGSheet(string sheet, int gid = 0)
    {
        var www = UnityWebRequest.Get(
            $"https://docs.google.com/spreadsheets/d/{sheet}/export?format=csv&id={sheet}&gid={gid}");
        Debug.Log($"Load:\nhttps://docs.google.com/spreadsheets/d/{sheet}/export?format=csv&id={sheet}&gid={gid}");
        www.SendWebRequest();
        while (!www.isDone) { }
        Debug.Log(www.downloadHandler.text);
        return ReadNew(www.downloadHandler.text);
    }

    public static void ApplyRowToObject(object obj, Dictionary<string, string> row)
    {
        //lobj
    }

    public static List<Dictionary<string, string>> Read(TextAsset data)
    {
        if (data != null) return Read(data.text);
        Debug.LogError("[ReadData] Read data null");
        return new List<Dictionary<string, string>>();
    }

    public static List<Dictionary<string, string>> ReadNew(string data)
    {
        CultureInfo ci = new CultureInfo("en-us");
        Thread.CurrentThread.CurrentCulture = ci;
        Thread.CurrentThread.CurrentUICulture = ci;
        data += '\n';
        var list = new List<Dictionary<string, string>>();
        var row = 0;
        var col = 0;
        var value = "";
        var headers = new List<string>();
        var entries = new Dictionary<string, string>();
        var inString = false;
        for (var i = 0; i < data.Length; i++)
        {
            var c = data[i];
            switch (c)
            {
                case '"' when i < data.Length - 1 && data[i + 1] != '"':
                    inString = !inString;
                    break;
                case ',' when !inString:
                case '\n' when !inString:
                    if (row == 0)
                    {
                        headers.Add(value.Trim(TRIM_CHARS));
                    }
                    else
                    {
                        if (!entries.ContainsKey(headers[col]))
                        {
                            entries.Add(headers[col], value.Trim(TRIM_CHARS));
                        }
                    }
                    value = "";
                    if (c == '\n')
                    {
                        if (row > 0) list.Add(entries);
                        entries = new Dictionary<string, string>();
                        col = 0;
                        row++;
                    }
                    else col++;

                    break;
                default:
                    value += c;
                    break;
            }
        }
        return list;
    }
    public static List<Dictionary<string, string>> Read(string data)
    {
        CultureInfo ci = new CultureInfo("en-us");
        Thread.CurrentThread.CurrentCulture = ci;
        Thread.CurrentThread.CurrentUICulture = ci;
        var list = new List<Dictionary<string, string>>();
        if (data == null)
        {
            Debug.LogError("[ReadData] Read data null");
            return list;
        }

        var lines = Regex.Split(data, LINE_SPLIT_RE);
        if (lines.Length <= 1)
        {
            return list;
        }
        var header = Regex.Split(lines[0], SPLIT_RE);
        for (var i = 1; i < lines.Length; i++)
        {
            var values = Regex.Split(lines[i], SPLIT_RE);
            if (values.Length == 0 || string.IsNullOrEmpty(values[0]))
            {
                continue;
            }
            var entries = new Dictionary<string, string>();
            for (var j = 0; j < header.Length && j < values.Length; j++)
            {
                var value = values[j];
                entries[header[j].Trim(TRIM_CHARS)] = value;
            }

            list.Add(entries);
        }

        return list;
    }

    public static void SetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
    {
        if (dictionary.ContainsKey(key))
        {
            dictionary[key] = value;
        }
        else
        {
            dictionary.Add(key, value);
        }
    }

    public static T Create<T>(Dictionary<string, string> row) where T : new()
    {
        return (T)new T().Populate(row);
    }

    public static object Populate(this object obj, Dictionary<string, string> row)
    {
        var type = obj.GetType();
        Debug.Log(type.Name);
        foreach (var field in type.GetFields())
        {
            if (!row.ContainsKey(field.Name)) continue;
            try
            {
                var parseMethod = field.FieldType.GetMethod(nameof(int.Parse), new[] { typeof(string) });
                var value = parseMethod != null
                    ? parseMethod.Invoke(null, new object[] { row[field.Name] })
                    : row[field.Name];
                obj.SetFieldValue(field.Name, value);
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }
        if (row.ContainsKey("extend_data") && row["extend_data"] != "")
        {
            Debug.Log(row["extend_data"]);
            //JsonConvert.PopulateObject(row["extend_data"], obj);
        }
        return obj;
    }

    public static void SetFieldValue(this object obj, string fieldName, object value)
    {
        obj.GetType().GetField(fieldName)?.SetValue(obj, value);
    }

    public static object GetFieldValue(this object obj, string fieldName)
    {
        return obj.GetType().GetField(fieldName)?.GetValue(obj);
    }

    public static void SetPropertyValue(this object obj, string propName, object value)
    {
        obj.GetType().GetProperty(propName)?.SetValue(obj, value, null);
    }

    public static object GetPropertyValue(this object obj, string propName)
    {
        return obj.GetType().GetProperty(propName)?.GetValue(obj, null);
    }
}