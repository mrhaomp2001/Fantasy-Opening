using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RuleTileDetector : Singleton<RuleTileDetector>, IUpdatable
{
    [SerializeField] private Grid grid;
    [SerializeField] private List<Tilemap> tilemaps;

    private Vector3Int lastCell;

    private TileBase currentTile;
    private TileBase lastValidTile;

    void Start()
    {
        lastCell = grid.WorldToCell(PlayerController.Instance.RbPlayer.transform.position);

        currentTile = GetTileAtCell(lastCell);

        if (currentTile != null)
            lastValidTile = currentTile;

    }
    private void OnEnable()
    {
        UpdateController.Instance.Updatables.Add(this);
    }
    private void OnDisable()
    {
        UpdateController.Instance.Updatables.Remove(this);

    }
    private void UpdateFunction()
    {
        if (PlayerController.Instance == null)
        {
            return;
        }
        if (PlayerController.Instance.RbPlayer.gameObject == null)
        {
            return;
        }

        Vector3Int currentCell = grid.WorldToCell(PlayerController.Instance.RbPlayer.transform.position);

        if (currentCell == lastCell) return;

        lastCell = currentCell;

        currentTile = GetTileAtCell(currentCell);

        OnTileChanged(currentTile);
    }

    TileBase GetTileAtCell(Vector3Int cell)
    {
        for (int i = 0; i < tilemaps.Count; i++)
        {
            if (!tilemaps[i].gameObject.activeSelf)
            {
                continue;
            }
            TileBase tile = tilemaps[i].GetTile(cell);

            if (tile != null)
                return tile;
        }

        return null;
    }

    void OnTileChanged(TileBase newTile)
    {
        // nếu là None → bỏ qua, không update
        if (newTile == null)
        {
            Debug.Log("Tile mới: None (giữ nguyên tile cũ)");
            return;
        }

        // cập nhật tile hợp lệ
        lastValidTile = newTile;

        Debug.Log("Tile mới: " + newTile.name);

        // xử lý tại đây bằng lastValidTile hoặc newTile đều được
    }

    // dùng ở chỗ khác nếu cần
    public TileBase GetCurrentValidTile()
    {
        return lastValidTile;
    }

    public void OnUpdate()
    {
        UpdateFunction();
    }

    
}