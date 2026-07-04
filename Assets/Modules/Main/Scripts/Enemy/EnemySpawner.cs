using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private string enemyName;

    [Header("Dimension Settings: ")]
    [SerializeField] private bool isAlive;
    [SerializeField] private string dimension;

    private GameObject enemy;
    private bool isInitialized;
    public string EnemyName { get => enemyName; set => enemyName = value; }
    public string Dimension { get => dimension; set => dimension = value; }
    public bool IsInitialized { get => isInitialized; set => isInitialized = value; }

    private void Start()
    {
        SpawnEnemy();

        IsInitialized = true;
    }

    public void SpawnEnemy()
    {
        //Debug.Log($"Spawn enemy: {gameObject.name}, enemy name: {enemyName}", gameObject);
        enemy = ObjectPooler.Instance.SpawnFromPool(enemyName, transform.position, Quaternion.identity);
        isAlive = true;
    }

    public void ResetEnemy()
    {
        if (enemy != null)
        {
            enemy.SetActive(false);
        }
        enemy = null;
    }

    public void OnChangeDimension()
    {
        if (Dimension.Equals(DimensionController.Instance.CurrentDimension))
        {
            OnShowEnemy();
        }
        else
        {
            OnHideEnemy();
        }
    }

    private void OnShowEnemy()
    {

        if (isAlive)
        {
            ResetEnemy();
            SpawnEnemy();
        }



        isAlive = false;
    }

    private void OnHideEnemy()
    {
        if (enemy != null)
        {
            isAlive = enemy.activeSelf;

            enemy.SetActive(false);


        }
        ResetEnemy();

    }
}
