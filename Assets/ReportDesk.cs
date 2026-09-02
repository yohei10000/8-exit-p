using UnityEngine;

public class ReportDesk : MonoBehaviour
{
    [Header("正誤設定")]
    [Tooltip("チェック有り＝異変ありステージ / チェック無し＝異常なしステージ")]
    [SerializeField] private bool isAnomalyPresent = false;

    [Header("待機場所のシーン名")]
    [SerializeField] private string waitingSceneName = "taiki Scene"; // 待機部屋の正確なシーン名

    private bool isNearDesk = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isNearDesk = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isNearDesk = false;
        }
    }

    private void Update()
    {
        if (!isNearDesk) return;

        // 左クリック または Eキー： 閉鎖（異変あり）と報告
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.E))
        {
            SubmitReport(reportedAnomaly: true);
        }
        // 右クリック または Rキー： 異常なし と報告
        else if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.R))
        {
            SubmitReport(reportedAnomaly: false);
        }
    }

    /// <summary>
    /// 報告の正誤判定処理
    /// </summary>
    /// <param name="reportedAnomaly">プレイヤーの報告（true:異変あり, false:異常なし）</param>


    private void SubmitReport(bool reportedAnomaly)
    {
        // 待機シーンに入る前に居た「直前のマップの異変情報」と比較する
        bool actualAnomaly = MapInfo.LastMapHasAnomaly;
        bool isCorrect = (reportedAnomaly == actualAnomaly);

        if (isCorrect)
        {
            Debug.Log($"【正解！】 報告:{reportedAnomaly} / 実際:{actualAnomaly} ➔ Day {DayManager.currentDay + 1} へ");

            if (DayManager.Instance != null)
            {
                DayManager.Instance.ProceedToNextDay(waitingSceneName);
            }
        }
        else
        {
            Debug.Log($"【不正解…】 報告:{reportedAnomaly} / 実際:{actualAnomaly} ➔ Day 1 にリセット");

            if (DayManager.Instance != null)
            {
                DayManager.Instance.ResetToDay1(waitingSceneName);
            }
        }
    }

    private void OnGUI()
    {
        if (isNearDesk)
        {
            GUIStyle style = new GUIStyle();
            style.fontSize = 25;
            style.normal.textColor = Color.white;
            GUI.Label(new Rect(20, 20, 600, 100), "【報告デスク】\n左クリック or [E]: 閉鎖 (異変あり)\n右クリック or [R]: 異常なし", style);
        }
    }
}