using System.Collections;
using System.Collections.Generic;
using Luxodd.Game.Scripts.Game.Leaderboard;
using TMPro;
using UnityEngine;

public class LeaderBoard : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] playerNameText;
    [SerializeField] private TextMeshProUGUI[] playerScoreText;
    [SerializeField] private int leaderboardSize;
    [SerializeField] private TextMeshProUGUI errorText;

    public void OnEnable()
    {
        NetworkManager.Instance.WebSocketCommandHandler.SendLeaderboardRequestCommand(OnGetLeaderboardSuccess, OnGetLeaderboardFail);
    }

    void OnGetLeaderboardSuccess(LeaderboardDataResponse response)
    {
        if (response.Leaderboard != null)
        {
            var playerList = response.Leaderboard;

            for (int i = 0; i < leaderboardSize; i++)
            {
                if (i < playerList.Count  && playerList[i] != null)
                {
                    playerNameText[i].text = playerList[i].PlayerName;
                    playerScoreText[i].text = playerList[i].TotalScore.ToString();
                }
                else
                {
                    playerNameText[i].text = "Empty";
                    playerScoreText[i].text = "0";
                }
            }
        }
    }

    void OnGetLeaderboardFail(int code, string message)
    {
        errorText.gameObject.SetActive(true);
        errorText.text = message;
    }
}
