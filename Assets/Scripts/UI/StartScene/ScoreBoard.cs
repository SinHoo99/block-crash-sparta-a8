using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBoard : MonoBehaviour
{
    private List<RectTransform> rectList;
    private void Awake()
    {
        rectList = new List<RectTransform>();
        List<RankInfo> rankList = RankBoardManager.Instance.GetRankList();
        for (int i = 0; i < rankList.Count; i++)
        {
            RectTransform rectTransform = gameObject.transform.Find($"Ranking{i+1}").GetComponent<RectTransform>();
            RankInfo rank = rankList[i];
            if (rank != null)
            {
                rectTransform.GetChild(1).GetComponent<Text>().text = rank.Name;
                rectTransform.GetChild(2).GetComponent<Text>().text = rank.Score.ToString();
            }
        }
    }
}
