using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;

public class PlayerHUD : MonoBehaviour
{
    [SerializeField] private GameObject uiRoot;
    [SerializeField] private TextMeshProUGUI totalMassText;

    // Leaderboard
    [Header("Leaderboard UI")]
    [SerializeField] private GameObject leaderboardPanel;
    [SerializeField] private TextMeshProUGUI leaderboardEntryPrefab;

    private PlayerController localPlayer;

    private void Awake()
    {
        uiRoot.SetActive(false);
        Menu.OnLocalPlayerGameEntered += HandleLocalPlayerInitialized;
    }


    private void HandleLocalPlayerInitialized() {
        localPlayer = PlayerController.Local;
        uiRoot.SetActive(true);
        StartCoroutine(LeaderboardLoop());
    }

    private void OnDestroy()
    {
        Menu.OnLocalPlayerGameEntered -= HandleLocalPlayerInitialized;
    }

    // Update is called once per frame
    void Update()
    {
        if (localPlayer == null) return;

        if (GameManager.IsConnected()) 
            totalMassText.text = localPlayer.GetMassLabel();
        
    }

    private IEnumerator LeaderboardLoop()
    {
        while(true) 
        {
            RefreshLeaderboard();
            yield return new WaitForSeconds(2f);
        }
    }

    private void RefreshLeaderboard()
    {
        // Nettoyer le leaderboard actuel
        foreach (Transform child in leaderboardPanel.transform)
        {
            Destroy(child.gameObject);
        }

        var circles = GameManager.Conn.Db.Circle.Iter();
        var entities = GameManager.Conn.Db.Entity;

        var leaderboard = circles
            .GroupBy(c => c.PlayerId)
            .Select(group => 
            {
                uint totalMass = 0;
                foreach (var circle in group)
                {
                    var entity = entities.EntityId.Find(circle.EntityId);
                    if (entity != null)
                        totalMass += entity.Mass;
                }

                var playerName = GameManager.Conn.Db.Player.PlayerId.Find(group.Key)?.Name ?? "Unknown";

                return new { playerName, totalMass };
            })
            .OrderByDescending(e => e.totalMass)
            .Take(10);

        var rank = 1;
        
        foreach (var entry in leaderboard)
        {
            string prefix = rank switch
            {
                1 => "[1er]",
                2 => "[2ème]",
                3 => "[3ème]",
                _ => $"{rank}."
            };
            
            var text = Instantiate(leaderboardEntryPrefab, leaderboardPanel.transform);
            text.gameObject.SetActive(true);
            text.text = $"{prefix} {entry.playerName} - {entry.totalMass}";
            rank++;
        }
    }
}
