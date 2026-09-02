using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnomalyManager : MonoBehaviour
{
    [Header("異変オブジェクトのリスト")]
    [SerializeField] private List<GameObject> anomalyObjects = new List<GameObject>();

    // 1ゲーム内で過去に出現した異変の番号（インデックス）を保持するリスト
    private static List<int> usedIndices = new List<int>();

    void Start()
    {
        // 1. まず全ての異変オブジェクトを非表示にする
        foreach (GameObject obj in anomalyObjects)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }

        // 2. まだ出現していない異変のインデックス（番号）をリストアップする
        List<int> unusedIndices = new List<int>();
        for (int i = 0; i < anomalyObjects.Count; i++)
        {
            // 出現済みリストに含まれていなければ「未出現リスト」に追加
            if (!usedIndices.Contains(i))
            {
                unusedIndices.Add(i);
            }
        }

        // 3. 未出現の異変が残っていれば、その中からランダムで1つ選んで表示する
        if (unusedIndices.Count > 0)
        {
            // 未出現リストの中でランダムな位置を選ぶ
            int randomListIndex = Random.Range(0, unusedIndices.Count);
            int chosenAnomalyIndex = unusedIndices[randomListIndex];

            // 選ばれた異変を表示
            if (anomalyObjects[chosenAnomalyIndex] != null)
            {
                anomalyObjects[chosenAnomalyIndex].SetActive(true);
            }

            // 出現済みリストに登録して、次回以降出ないようにする
            usedIndices.Add(chosenAnomalyIndex);
        }
        else
        {
            Debug.Log("すべての異変が出尽くしました（または異変が登録されていません）");
        }
    }

    /// <summary>
    /// ゲームオーバー時やタイトルへ戻った時など、
    /// 1ゲームをリセットして最初から遊ぶ時に呼び出すメソッド
    /// </summary>
    public static void ResetAnomalyHistory()
    {
        usedIndices.Clear();
    }
}