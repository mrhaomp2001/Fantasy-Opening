using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUpdatable
{
    public void OnUpdate();
}

public interface IFixedUpdatable
{
    void OnFixedUpdate();
}
public class UpdateController : MonoBehaviour
{
    private static UpdateController instance;
    private List<IUpdatable> updatables = new();
    private List<IFixedUpdatable> fixedUpdateables = new();

    public List<IUpdatable> Updatables { get => updatables; set => updatables = value; }
    public List<IFixedUpdatable> FixedUpdateables { get => fixedUpdateables; set => fixedUpdateables = value; }
    public static UpdateController Instance { get => instance; set => instance = value; }

    private void Awake()
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

    private void Update()
    {
        for (int i = 0; i < updatables.Count; i++)
        {
            var item = updatables[i];
            item.OnUpdate();
        }
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < fixedUpdateables.Count; i++)
        {
            var item = fixedUpdateables[i];
            item.OnFixedUpdate();
        }
    }
}
