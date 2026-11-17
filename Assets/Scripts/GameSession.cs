using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSession : MonoBehaviour
{
    public static GameSession Instance { get; private set; }

    private const string PlayerNameKey = "player_name";
    private const string MenuScene = "MainMenu";

    private WaveManager waveManager;
    private BaseHealth baseHealth;
    private Difficulty difficulty;

    private int score;
    private int kills;
    private float elapsed;
    private float timeLimit;
    private bool finished;
    private bool victory;
    private bool saved;

    private TMP_Text hudText;
    private GameObject endPanel;
    private TMP_Text endTitle;
    private TMP_Text endDetails;
    private TMP_InputField nameInput;
    private Button saveButton;

    public bool Finished => finished;
    public int Score => score;

    public static GameSession Create(WaveManager waveManager, Difficulty difficulty)
    {
        GameSession session = waveManager.gameObject.AddComponent<GameSession>();
        session.waveManager = waveManager;
        session.difficulty = difficulty;
        return session;
    }

    private void Awake()
    {
        Instance = this;
        Time.timeScale = 1f;
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    private void Start()
    {
        baseHealth = FindFirstObjectByType<BaseHealth>();
        timeLimit = waveManager != null ? waveManager.TimeLimit : 0f;
        BuildUI();
        UpdateHud();
    }

    private void Update()
    {
        if (finished)
            return;

        elapsed += Time.deltaTime;

        if (timeLimit > 0f && elapsed >= timeLimit)
        {
            elapsed = timeLimit;
            Finish(false, "TIME'S UP");
            return;
        }

        UpdateHud();
    }

    public void RegisterKill(int points)
    {
        if (finished)
            return;

        kills++;
        score += points;
    }

    public void OnBaseDestroyed()
    {
        Finish(false, "GAME OVER");
    }

    public void OnAllWavesCleared()
    {
        Finish(true, "VICTORY!");
    }

    private void Finish(bool won, string title)
    {
        if (finished)
            return;

        finished = true;
        victory = won;

        int healthBonus = 0;
        if (won && baseHealth != null)
            healthBonus = Mathf.CeilToInt(baseHealth.Health);

        int timeBonus = 0;
        if (won && timeLimit > 0f)
            timeBonus = Mathf.CeilToInt(timeLimit - elapsed);

        score += healthBonus + timeBonus;
        Time.timeScale = 0f;

        BuildManager buildManager = FindFirstObjectByType<BuildManager>();
        if (buildManager != null)
            buildManager.CancelBuild();

        UpdateHud();

        endTitle.text = title;
        endTitle.color = won ? new Color(1f, 0.9f, 0f) : new Color(1f, 0.3f, 0.3f);
        endDetails.text =
            "Difficulty: " + difficulty +
            "\nKills: " + kills + "   Time: " + FormatTime(elapsed) +
            (won ? "\nBase health bonus: +" + healthBonus + "   Time bonus: +" + timeBonus : "") +
            "\n<size=130%><b>Score: " + score + "</b></size>";

        endPanel.SetActive(true);
        nameInput.text = PlayerPrefs.GetString(PlayerNameKey, "");
        nameInput.Select();
        nameInput.ActivateInputField();
    }

    public void SaveScore()
    {
        if (!finished || saved)
            return;

        string playerName = nameInput.text.Replace("<", "").Replace(">", "").Trim();
        if (string.IsNullOrEmpty(playerName))
            playerName = "Player";

        PlayerPrefs.SetString(PlayerNameKey, playerName);

        RankingEntry entry = new RankingEntry
        {
            playerName = playerName,
            score = score,
            kills = kills,
            time = elapsed,
            victory = victory,
            date = DateTime.Now.ToString("dd/MM/yyyy HH:mm")
        };

        int position = RankingManager.Add(difficulty, entry);

        saved = true;
        saveButton.interactable = false;
        nameInput.interactable = false;
        SetButtonLabel(saveButton, "Saved!");

        endDetails.text += position > 0
            ? "\n<color=#FFE500>Ranking position: #" + position + "</color>"
            : "\nNot in the top " + RankingManager.MaxEntries;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(MenuScene);
    }

    private void UpdateHud()
    {
        if (hudText == null)
            return;

        string health = baseHealth != null
            ? Mathf.CeilToInt(baseHealth.Health) + "/" + Mathf.CeilToInt(baseHealth.MaxHealth)
            : "0";

        string wave = waveManager != null
            ? waveManager.CurrentWaveNumber + "/" + waveManager.WaveCount
            : "-";

        hudText.text =
            "Base: " + health +
            "\nWave: " + wave +
            "\nScore: " + score +
            "\nKills: " + kills +
            "\n" + TimeLabel();
    }

    private string TimeLabel()
    {
        if (timeLimit <= 0f)
            return "Time: " + FormatTime(elapsed);

        float remaining = Mathf.Max(timeLimit - elapsed, 0f);
        string label = "Time left: " + FormatTime(Mathf.Ceil(remaining));
        return remaining <= 20f ? "<color=#FF4D4D>" + label + "</color>" : label;
    }

    private static string FormatTime(float seconds)
    {
        int total = Mathf.FloorToInt(seconds);
        return (total / 60).ToString("00") + ":" + (total % 60).ToString("00");
    }

    private void BuildUI()
    {
        EnsureEventSystem();

        GameObject canvasObject = new GameObject("GameUI", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        Canvas canvas = canvasObject.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 100;

        CanvasScaler scaler = canvasObject.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        scaler.matchWidthOrHeight = 0.5f;

        Transform root = canvasObject.transform;

        hudText = CreateText(root, "HUD", 32, TextAlignmentOptions.TopLeft);
        hudText.outlineWidth = 0.2f;
        hudText.outlineColor = Color.black;
        hudText.rectTransform.pivot = new Vector2(0f, 1f);
        Place(hudText.rectTransform, new Vector2(0f, 1f), new Vector2(30f, -20f), new Vector2(500f, 220f));

        endPanel = new GameObject("EndPanel", typeof(RectTransform), typeof(Image));
        endPanel.transform.SetParent(root, false);
        endPanel.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0.8f);
        RectTransform panelRect = endPanel.GetComponent<RectTransform>();
        panelRect.anchorMin = Vector2.zero;
        panelRect.anchorMax = Vector2.one;
        panelRect.offsetMin = Vector2.zero;
        panelRect.offsetMax = Vector2.zero;

        Transform panel = endPanel.transform;

        endTitle = CreateText(panel, "Title", 110, TextAlignmentOptions.Center);
        endTitle.fontStyle = FontStyles.Bold;
        Place(endTitle.rectTransform, new Vector2(0.5f, 0.5f), new Vector2(0f, 350f), new Vector2(1400f, 160f));

        endDetails = CreateText(panel, "Details", 40, TextAlignmentOptions.Center);
        Place(endDetails.rectTransform, new Vector2(0.5f, 0.5f), new Vector2(0f, 130f), new Vector2(1400f, 300f));

        nameInput = CreateInput(panel);
        Place((RectTransform)nameInput.transform, new Vector2(0.5f, 0.5f), new Vector2(0f, -80f), new Vector2(600f, 80f));
        nameInput.onSubmit.AddListener(_ => SaveScore());

        saveButton = CreateButton(panel, "Save score", SaveScore);
        Place((RectTransform)saveButton.transform, new Vector2(0.5f, 0.5f), new Vector2(0f, -190f), new Vector2(420f, 90f));

        Button restartButton = CreateButton(panel, "Play again", Restart);
        Place((RectTransform)restartButton.transform, new Vector2(0.5f, 0.5f), new Vector2(-230f, -330f), new Vector2(420f, 90f));

        Button menuButton = CreateButton(panel, "Main menu", GoToMenu);
        Place((RectTransform)menuButton.transform, new Vector2(0.5f, 0.5f), new Vector2(230f, -330f), new Vector2(420f, 90f));

        endPanel.SetActive(false);
    }

    private static void EnsureEventSystem()
    {
        if (FindFirstObjectByType<EventSystem>() != null)
            return;

        GameObject eventSystem = new GameObject("EventSystem", typeof(EventSystem), typeof(InputSystemUIInputModule));
        eventSystem.GetComponent<InputSystemUIInputModule>().AssignDefaultActions();
    }

    private static TMP_Text CreateText(Transform parent, string name, float size, TextAlignmentOptions alignment)
    {
        GameObject textObject = new GameObject(name, typeof(RectTransform));
        textObject.transform.SetParent(parent, false);

        TextMeshProUGUI text = textObject.AddComponent<TextMeshProUGUI>();
        text.fontSize = size;
        text.alignment = alignment;
        text.color = Color.white;
        text.raycastTarget = false;
        return text;
    }

    private static TMP_InputField CreateInput(Transform parent)
    {
        GameObject inputObject = TMP_DefaultControls.CreateInputField(new TMP_DefaultControls.Resources());
        inputObject.name = "NameInput";
        inputObject.transform.SetParent(parent, false);

        TMP_InputField input = inputObject.GetComponent<TMP_InputField>();
        input.characterLimit = 12;
        input.pointSize = 40;

        TMP_Text placeholder = input.placeholder as TMP_Text;
        if (placeholder != null)
            placeholder.text = "Your name";

        input.textComponent.alignment = TextAlignmentOptions.Center;
        if (placeholder != null)
            placeholder.alignment = TextAlignmentOptions.Center;

        return input;
    }

    private static Button CreateButton(Transform parent, string label, UnityAction action)
    {
        GameObject buttonObject = TMP_DefaultControls.CreateButton(new TMP_DefaultControls.Resources());
        buttonObject.name = label + " Button";
        buttonObject.transform.SetParent(parent, false);

        buttonObject.GetComponent<Image>().color = new Color(1f, 0.9f, 0f);

        Button button = buttonObject.GetComponent<Button>();
        button.onClick.AddListener(action);

        TMP_Text text = buttonObject.GetComponentInChildren<TMP_Text>();
        text.fontSize = 40;
        text.fontStyle = FontStyles.Bold;
        text.color = new Color(0.15f, 0.15f, 0.15f);

        SetButtonLabel(button, label);
        return button;
    }

    private static void SetButtonLabel(Button button, string label)
    {
        TMP_Text text = button.GetComponentInChildren<TMP_Text>();
        if (text != null)
            text.text = label;
    }

    private static void Place(RectTransform rect, Vector2 anchor, Vector2 position, Vector2 size)
    {
        rect.anchorMin = anchor;
        rect.anchorMax = anchor;
        rect.anchoredPosition = position;
        rect.sizeDelta = size;
    }
}
