using UnityEngine;
using UnityEngine.Networking; // Thư viện để tải dữ liệu web
using System.Collections;
using System.Collections.Generic;
using System;

public class SheetImporter : MonoBehaviour
{
    [Tooltip("Dán link 'Xuất bản lên web' (dạng .csv) của Google Sheet vào đây")]
    public string sheetURL;

    public List<string> tenList = new List<string>();
    public List<string> msvList = new List<string>();
    public List<string> passList = new List<string>();

    [ContextMenu("LoadDataFromSheet")]
    public void LoadFromSheet()
    {
        if (string.IsNullOrEmpty(sheetURL))
        {
            Debug.LogError("Bạn chưa dán link Google Sheet URL vào script!");
            return;
        }

        StartCoroutine(LoadDataFromSheet());
    }

    IEnumerator LoadDataFromSheet()
    {
        Debug.Log("Đang tải dữ liệu từ Google Sheet...");
        using (UnityWebRequest www = UnityWebRequest.Get(sheetURL))
        {
            yield return www.SendWebRequest(); 

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Tải thành công!");
                string csvData = www.downloadHandler.text;

                ParseCSV(csvData);
            }
            else
            {
                Debug.LogError("Lỗi khi tải dữ liệu: " + www.error);
            }
        }
    }

    void ParseCSV(string csvData)
    {
        tenList.Clear();
        msvList.Clear();
        passList.Clear();

        string[] lines = csvData.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

        if (lines.Length <= 1)
        {
            Debug.LogWarning("File CSV rỗng hoặc chỉ có dòng tiêu đề.");
            return;
        }

        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i];
            if (string.IsNullOrWhiteSpace(line)) continue;

            string[] values = line.Split(',');

            if (values.Length >= 3)
            {
                tenList.Add(values[0].Trim());
                msvList.Add(values[1].Trim());
                //passList.Add(values[2].Trim());
            }
        }

        Debug.Log($"Phân tích hoàn tất! Đã tải {tenList.Count} mục.");
        // for(int i = 0; i < tenList.Count; i++)
        // {
        //     Debug.Log($"Tên: {tenList[i]}, MSV: {msvList[i]}, Pass: {passList[i]}");
        // }
    }
}