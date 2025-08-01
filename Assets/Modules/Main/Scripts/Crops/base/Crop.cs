using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Crop", menuName = "Farm/Crop")]
public class Crop : ScriptableObject
{
	[System.Serializable]
	public class CropStage
	{
		public Sprite sprite;
	}
	[SerializeField] private int id;
	[SerializeField] private string cropName;
	[SerializeField] private int productId;
	[SerializeField] private int priceSell;
    [SerializeField] private List<CropStage> stages;

    public int Id { get => id; set => id = value; }
    public string CropName { get => cropName; set => cropName = value; }
    public int PriceSell { get => priceSell; set => priceSell = value; }
    public List<CropStage> Stages { get => stages; set => stages = value; }
    public int ProductId { get => productId; set => productId = value; }
}
