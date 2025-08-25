using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Buff", menuName = "Farm/Buff")]
[JsonObject(MemberSerialization.OptIn), System.Serializable]
public class BuffBase : ScriptableObject
{
    [JsonProperty]
    [SerializeField] private int id;
    [SerializeField] private string buffName;
    [SerializeField] private string buffDescription;
    [SerializeField] private GameStatCollection stats;
    [SerializeField] private Sprite spriteBuff;

    public int Id { get => id; set => id = value; }
    public string BuffName { get => buffName; set => buffName = value; }
    public string BuffDescription { get => buffDescription; set => buffDescription = value; }
    public Sprite SpriteBuff { get => spriteBuff; set => spriteBuff = value; }
    public GameStatCollection Stats { get => stats; set => stats = value; }
}
