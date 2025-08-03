using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WorldItemController : MonoBehaviour
{
    private static WorldItemController instance;

    public static WorldItemController Instance { get => instance; set => instance = value; }

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SpawnItem(int valueItemId, Vector3 position)
    {
        var targetObject = ObjectPooler.Instance.SpawnFromPool("world_item", position, Quaternion.identity);
        var worlditem = targetObject.GetComponent<WorldItem>();

        var item = ItemDatabase.Instance.Items
            .Where(predicate =>
            {
                return predicate.Id == valueItemId;
            })
            .FirstOrDefault();

        worlditem.Initialize(valueItemId, item.Sprite);
    }
}
