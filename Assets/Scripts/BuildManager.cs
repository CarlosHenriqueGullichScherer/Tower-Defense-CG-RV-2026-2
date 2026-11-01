using UnityEngine;

public class BuildManager : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private BuildPreview previewPrefab;
    [SerializeField] private GameObject[] towerPrefabs;

    private GameObject selectedTowerPrefab;
    private BuildPreview currentPreview;

    private void Update()
    {
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

    selectedTowerPrefab = towerPrefabs[index];
    currentPreview = Instantiate(previewPrefab);

    Debug.Log("Prévia criada: " + currentPreview.name);
}

    private void BuildTower()
    {
        Instantiate(selectedTowerPrefab, currentPreview.transform.position, Quaternion.identity);
        CancelBuild();
    }

    public void CancelBuild()
    {
        if (currentPreview != null)
            Destroy(currentPreview.gameObject);

        currentPreview = null;
        selectedTowerPrefab = null;
    }
}