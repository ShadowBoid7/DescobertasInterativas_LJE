using UnityEngine;
using TMPro;

public class CrewResourcesUI : MonoBehaviour
{
    [SerializeField] private CrewResources crewResources;

    [Header("UI")]
    [SerializeField] private TMP_Text hungerText;
    [SerializeField] private TMP_Text thirstText;
    [SerializeField] private TMP_Text moraleText;

    private void Update()
    {
        if (crewResources == null) return;

        hungerText.text = $"Fome: {crewResources.hunger.currentValue:0}/{crewResources.hunger.maxValue}";
        thirstText.text = $"Sede: {crewResources.thirst.currentValue:0}/{crewResources.thirst.maxValue}";
        moraleText.text = $"Moral: {crewResources.morale.currentValue:0}/{crewResources.morale.maxValue}";
    }
}

