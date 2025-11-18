using UnityEngine;
using LootLocker.Requests;
using TMPro;
using System.Collections;

public class LoginChecker : MonoBehaviour
{
    public TMP_InputField studentIdInput;
    public SheetImporter sheetImporter;
    public GameObject blockAction;
    public GameObject letter;
    public GameObject notify;
    public GameObject result;
    public TMP_Text notifyText;
    public TMP_Text letterText;
    public TMP_Text resultText;

    private const int VE_TRUNG_TUYEN_ASSET_ID = 819695;

    public void OnCheckButtonPressed()
    {
        blockAction.SetActive(true);
        string studentID = studentIdInput.text.ToUpper().Trim();
        if (string.IsNullOrEmpty(studentID))
        {
            notify.SetActive(true);
            notifyText.text = "Vui lòng nhập Mã Sinh Viên!";
            blockAction.SetActive(false);
            return;
        }
        if (!sheetImporter.msvList.Contains(studentID))
        {
            notify.SetActive(true);
            notifyText.text = "Mã Sinh Viên không tồn tại!";
            blockAction.SetActive(false);
            return;
        }
        LootLockerSDKManager.StartGuestSession("B24DCGA115", (response) =>
        {
            if (!response.success)
            {
                notify.SetActive(true);
                notifyText.text = "Lỗi: Không tìm thấy MSV hoặc lỗi kết nối.";
                blockAction.SetActive(false);
                return;
            }
            else
            {
                LootLockerSDKManager.GetInventory((response) =>
                {
                    if (!response.success)
                    {
                        notify.SetActive(true);
                        notifyText.text = "Lỗi: Không thể lấy dữ liệu.";
                        blockAction.SetActive(false);
                        return;
                    }
                    else
                    {
                        bool isWinner = false;
                        foreach (var item in response.inventory)
                        {
                            if (item.asset.id == VE_TRUNG_TUYEN_ASSET_ID)
                            {
                                isWinner = true;
                                break;
                            }
                        }

                        LootLockerSDKManager.EndSession((response) =>
                        {
                            if (isWinner)
                            { 
                                notify.SetActive(true);

                                notifyText.text = "Đang kiểm tra...";

                                LootLockerSDKManager.StartGuestSession(studentID, (response) =>
                                {
                                    if (!response.success)
                                    {
                                        notify.SetActive(true);
                                        notifyText.text = "Lỗi: Lỗi kết nối. ";
                                        blockAction.SetActive(false);
                                        return;
                                    }
                                    else
                                    {
                                        CheckInventory();
                                        blockAction.SetActive(false);
                                    }
                                });
                            }
                            else
                            {
                                notify.SetActive(true);
                                notifyText.text = "Chưa đến thời gian công bố kết quả!";
                                blockAction.SetActive(false);
                            }
                        });
                    }
                });
            }
        });
        
    }

    private void CheckInventory()
    {
        LootLockerSDKManager.GetInventory((response) =>
        {
            if (!response.success)
            {
                notify.SetActive(true);
                notifyText.text = "Lỗi: Không thể lấy dữ liệu kho đồ. " + response.errorData;
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
            letter.SetActive(true);
            letterText.text = sheetImporter.GetDesByMSV(studentIdInput.text);
            result.SetActive(true);
            if (isWinner)
            {
                resultText.text = "Chúc mừng em, em từ giờ đã trở thành một thành viên chính thức của Câu lạc bộ Nhà Sáng Tạo Game PTIT.Mong rằng GCC sẽ luôn là nơi để em gắn bó, vui chơi, rèn luyện kỹ năng và cùng mọi người tạo ra thật nhiều kỷ niệm đáng nhớ.".Normalize();
            }
            else
            {
                resultText.text = "Cảm ơn em đã quan tâm và đăng ký tham gia Câu lạc bộ Nhà Sáng Tạo Game PTIT. Sau khi xem xét, rất tiếc là định hướng hiện tại của CLB và định hướng của em chưa thật sự phù hợp. Mong em thông cảm và tiếp tục giữ vững đam mê với game. Chúc em may mắn và hy vọng sẽ có cơ hội gặp lại em trong những đợt tuyển sau.".Normalize();
            }
            notify.SetActive(false);
            LootLockerSDKManager.EndSession((response) =>{ });
        });
        
    }
}