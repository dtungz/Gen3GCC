using UnityEngine;
using LootLocker.Requests;
using TMPro;
using System.Collections;
using System.Linq;

public class LoginChecker : MonoBehaviour
{
    public TMP_InputField studentIdInput;
    public SheetImporter sheetImporter;
    public GameObject blockAction;
    public GameObject letter;
    public GameObject notify;
    public GameObject result;
    public TMP_Text notifyText;
    public UIEnvelopeOpen letterText;
    public UIEnvelopeOpen resultText;
    private string[] troll = { 
        "Đừng nhập linh tinh nhé, nhập mã sinh viên vào đi", 
        "Sao lại nhập sai nữa vậy",
        "Có phải bạn là IMPOSTER?",
        "..."
    };
    public int trollCount = 0;
    private const int VE_TRUNG_TUYEN_ASSET_ID = 819695;

    public void OnCheckButtonPressed()
    {
        blockAction.SetActive(true);
        string studentID = studentIdInput.text.ToUpper().Trim();
        studentIdInput.text = studentID;
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
            notifyText.text = troll[trollCount];
            trollCount++;
            trollCount %= troll.Count();
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

                                notifyText.text = "Đang mở thư...";

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
                                notifyText.text = "Chưa có thư gửi đến cho bạn! (Chưa đến thời gian công bố kết quả)";
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
            letterText.SetText(sheetImporter.GetDesByMSV(studentIdInput.text));
            if (isWinner)
            {
                resultText.SetText("Chúc mừng em, em từ giờ đã trở thành một thành viên chính thức của Câu lạc bộ Nhà Sáng Tạo Game PTIT.Mong rằng GCC sẽ luôn là nơi để em gắn bó, vui chơi, rèn luyện kỹ năng và cùng mọi người tạo ra thật nhiều kỷ niệm đáng nhớ.".Normalize());
            }
            else
            {
                resultText.SetText("Cảm ơn em đã quan tâm và đăng ký tham gia Câu lạc bộ Nhà Sáng Tạo Game PTIT. Sau khi xem xét, rất tiếc là định hướng hiện tại của CLB và định hướng của em chưa thật sự phù hợp. Mong em thông cảm và tiếp tục giữ vững đam mê với game. Chúc em may mắn và hy vọng sẽ có cơ hội gặp lại em trong những đợt tuyển sau.".Normalize());
            }
            LootLockerSDKManager.EndSession((response) =>{ });
        });
        
    }
}