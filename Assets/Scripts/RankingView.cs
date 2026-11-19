using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class RankingView : MonoBehaviour
{
    [SerializeField] private TMP_Text easyText;
    [SerializeField] private TMP_Text hardText;
    [SerializeField] private int visibleEntries = 5;

    private void OnEnable()
    {
        Refresh();
    }

    public void Refresh()
    {
        Fill(easyText, Difficulty.Easy, "Easy");
        Fill(hardText, Difficulty.Hard, "Hard");
    }

    public void ClearRanking()
    {
        RankingManager.Clear();
        Refresh();
    }

    private void Fill(TMP_Text target, Difficulty difficulty, string title)
    {
        if (target == null)
            return;

        List<RankingEntry> entries = RankingManager.Load(difficulty);
        StringBuilder builder = new StringBuilder();

        builder.AppendLine("<color=#FFE500><b>" + title + "</b></color>");

        if (entries.Count == 0)
            builder.AppendLine("<alpha=#AA>No games yet");

        for (int i = 0; i < entries.Count && i < visibleEntries; i++)
        {
            RankingEntry entry = entries[i];
            string mark = entry.victory ? "" : " <size=70%><alpha=#88>(lost)</size><alpha=#FF>";
            builder.AppendLine((i + 1) + ". " + entry.playerName + mark + "<pos=75%>" + entry.score);
        }

        target.text = builder.ToString();
    }
}
