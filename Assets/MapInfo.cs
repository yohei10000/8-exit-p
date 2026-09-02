using UnityEngine;

public class MapInfo : MonoBehaviour
{
    [Header("このマップの設定")]
    [Tooltip("チェック有り＝異変ありマップ / チェック無し＝異常なしマップ")]
    public bool isAnomalyMap = false;

    // 現在探索中のマップに異変があったかを記憶する（静的変数）
    public static bool LastMapHasAnomaly = false;

    private void Start()
    {
        // マップが読み込まれたら、そのマップの異変情報を記録する
        LastMapHasAnomaly = isAnomalyMap;
        Debug.Log($"【マップ読み込み】このマップの異変状態: {isAnomalyMap}");
    }
}