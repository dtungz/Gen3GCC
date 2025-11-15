using UnityEngine;
using LootLocker.Requests;
using TMPro;
using System.Collections;

public class LoginChecker : MonoBehaviour
{
    public TMP_InputField studentIdInput;
    public TMP_Text resultText;
    public SheetImporter sheetImporter;

    private const int VE_TRUNG_TUYEN_ASSET_ID = 819695;

    //[ContextMenu("CreatAccount")]
    //public void RegisterAll()
    //{
    //    StartCoroutine(RigAll());
    //}
    //public IEnumerator RigAll()
    //{
    //    for (int i = 0; i < sheetImporter.tenList.Count; i++)
    //    {
    //        string studentID = sheetImporter.msvList[i];
    //        string studentName = sheetImporter.tenList[i];
    //        bool done1 = false;
    //        LootLockerSDKManager.StartGuestSession(studentID, (response) =>
    //        {
    //            if (!response.success)
    //            {
    //                resultText.text = "Lỗi: Không tìm thấy MSV hoặc lỗi kết nối. " + response.errorData;
    //                return;
    //            }
    //            else
    //            {
    //                string name = studentName + studentID.Substring(5);
    //                LootLockerSDKManager.SetPlayerName(studentName, (response) =>
    //                {
    //                    LootLockerSDKManager.EndSession((response) =>
    //                    {
    //                        done1 = true;
    //                    });
    //                });
                    
    //            }
                
    //        });

    //        yield return new WaitUntil(()=>done1);
    //        yield return new WaitForSeconds(0.2f);
    //        Debug.Log("Done: " + i);
    //    }
    //}

    public void OnCheckButtonPressed()
    {
        LootLockerSDKManager.StartGuestSession("B24DCGA115", (response) =>
        {
            if (!response.success)
            {
                resultText.text = "Lỗi: Không tìm thấy MSV hoặc lỗi kết nối. " + response.errorData;
            }
            LootLockerSDKManager.GetInventory((response) =>
            {
                if (!response.success)
                {
                    resultText.text = "Lỗi: Không thể lấy dữ liệu kho đồ. " + response.errorData;
                    return;
                }

                bool isWinner = false;

                foreach (var item in response.inventory)
                {
                    if (item.asset.id == VE_TRUNG_TUYEN_ASSET_ID)
                    {
                        isWinner = true;
                        break;
                    }
                }

                LootLockerSDKManager.EndSession((response) => {
                    if(isWinner)
                    {
                        string studentID = studentIdInput.text.ToUpper().Trim();
                        if (string.IsNullOrEmpty(studentID))
                        {
                            resultText.text = "Vui lòng nhập Mã Sinh Viên!";
                            return;
                        }
                        if (!sheetImporter.msvList.Contains(studentID))
                        {
                            resultText.text = "Mã Sinh Viên không tồn tại!";
                            return;
                        }

                        resultText.text = "Đang kiểm tra...";

                        LootLockerSDKManager.StartGuestSession(studentID, (response) =>
                        {
                            if (!response.success)
                            {
                                resultText.text = "Lỗi: Không tìm thấy MSV hoặc lỗi kết nối. " + response.errorData;
                                return;
                            }
                            // Bước 2: Đăng nhập OK, kiểm tra kho đồ
                            CheckInventory();
                        });
                    }
                    else
                    {
                        resultText.text = "Chưa đến thời gian công bố kết quả!";
                    }
                });
            });

        });
        
    }

    private void CheckInventory()
    {
        LootLockerSDKManager.GetInventory((response) =>
        {
            if (!response.success)
            {
                resultText.text = "Lỗi: Không thể lấy dữ liệu kho đồ. " + response.errorData;
                return;
            }

            bool isWinner = false; 

            foreach (var item in response.inventory)
            {
                if (item.asset.id == VE_TRUNG_TUYEN_ASSET_ID)
                {
                    isWinner = true; 
                    break;
                }
            }
            
            if (isWinner)
            {
                resultText.text = "Chúc mừng! Bạn đã trúng tuyển (Pass).";
            }
            else
            {
                resultText.text = "Rất tiếc, bạn không trúng tuyển (Fail).";
            }
            LootLockerSDKManager.EndSession((response) =>{ });
        });
        
    }
}