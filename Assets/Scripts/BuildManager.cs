using UnityEngine;

public class BuildManager : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private BuildPreview previewPrefab;
    [SerializeField] private GameObject[] towerPrefabs;
    [SerializeField] private int[] towerCosts = { 60, 40 };
    [SerializeField] private string[] towerNames = { "Ranger", "Ninja" };
    [SerializeField] private AudioClip buildSound;

    private GameObject selectedTowerPrefab;
    private int selectedCost;
    private BuildPreview currentPreview;

    private void Update()
    {
        if (GameSession.Instance != null && GameSession.Instance.Finished)
            return;

        if (Input.GetKeyDown(KeyCode.Alpha1))
{
    Debug.Log("Tecla 1 detectada");
    SelectTower(0);
}

        if (Input.GetKeyDown(KeyCode.Alpha2))
            SelectTower(1);

        if (currentPreview == null)
            return;

        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        currentPreview.UpdatePreview(mousePosition);

        if (Input.GetMouseButtonDown(0) && currentPreview.CanBuild)
            BuildTower();

        if (Input.GetMouseButtonDown(1))
            CancelBuild();
    }

    private void SelectTower(int index)
{
    if (index < 0 || index >= towerPrefabs.Length)
        return;

    CancelBuild();

    int cost = CostOf(index);
    GameSession session = GameSession.Instance;
    if (session != null && !session.CanAfford(cost))
    {
        session.ShowMessage("Not enough gold for " + NameOf(index) + " (" + cost + ")");
        return;
    }

    selectedTowerPrefab = towerPrefabs[index];
    selectedCost = cost;
    currentPreview = Instantiate(previewPrefab);

    Debug.Log("Prévia criada: " + currentPreview.name);
}

    private void BuildTower()
    {
        GameSession session = GameSession.Instance;
        if (session != null && !session.TrySpend(selectedCost))
        {
            session.ShowMessage("Not enough gold");
            CancelBuild();
            return;
        }

        Instantiate(selectedTowerPrefab, currentPreview.transform.position, Quaternion.identity);
        AudioManager.Instance.PlaySFX(buildSound);
        CancelBuild();
    }

    public void CancelBuild()
    {
        if (currentPreview != null)
            Destroy(currentPreview.gameObject);

        currentPreview = null;
        selectedTowerPrefab = null;
        selectedCost = 0;
    }

    public string ShopText(int gold)
    {
        string text = "";

        for (int i = 0; i < towerPrefabs.Length; i++)
        {
            int cost = CostOf(i);
            string entry = "[" + (i + 1) + "] " + NameOf(i) + " - " + cost + "g";
            text += (gold >= cost ? entry : "<color=#888888>" + entry + "</color>") + "      ";
        }

        return text.TrimEnd();
    }

    private int CostOf(int index)
    {
        return towerCosts != null && index < towerCosts.Length ? towerCosts[index] : 0;
    }

    private string NameOf(int index)
    {
        return towerNames != null && index < towerNames.Length ? towerNames[index] : "Tower " + (index + 1);
    }
}