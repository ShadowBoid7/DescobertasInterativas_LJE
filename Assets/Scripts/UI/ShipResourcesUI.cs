using UnityEngine;
using TMPro;

public class ShipResourcesUI : MonoBehaviour
{
    [SerializeField] private ShipResources shipResources;

    [Header("UI")]
    [SerializeField] private TMP_Text waterText;
    [SerializeField] private TMP_Text foodText;
    [SerializeField] private TMP_Text conditionText;

    private void Update()
    {
        if (shipResources == null) return;

        waterText.text = $"Água: {shipResources.water.currentValue:0}/{shipResources.water.maxValue}";
        foodText.text = $"Comida: {shipResources.food.currentValue:0}/{shipResources.food.maxValue}";
        conditionText.text = $"Condições: {shipResources.condition.currentValue:0}/{shipResources.condition.maxValue}";
    }
}

