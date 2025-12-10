using UnityEngine;

public class ShipResources : MonoBehaviour
{
    [Header("Ship Resources")]
    public Resource water = new Resource { name = "Água", currentValue = 50f, maxValue = 100f, drainPerSecond = 0.5f };
    public Resource food = new Resource { name = "Comida", currentValue = 50f, maxValue = 100f, drainPerSecond = 0.3f };
    public Resource condition = new Resource { name = "Condições", currentValue = 80f, maxValue = 100f, drainPerSecond = 0.2f };

    private void Update()
    {
        float dt = Time.deltaTime;

        water.Tick(dt);
        food.Tick(dt);
        condition.Tick(dt);
    }

    // Chamado pelos minijogos
    public void AddWater(float amount)
    {
        water.currentValue = Mathf.Clamp(water.currentValue + amount, 0, water.maxValue);
        ConsoleOverlay.Log($"Água aumentada! Agora: {water.currentValue:0}");
    }

    public void AddFood(float amount)
    {
        food.currentValue = Mathf.Clamp(food.currentValue + amount, 0, food.maxValue);
        ConsoleOverlay.Log($"Comida aumentada! Agora: {food.currentValue:0}");
    }

    public void RepairShip(float amount)
    {
        condition.currentValue = Mathf.Clamp(condition.currentValue + amount, 0, condition.maxValue);
        ConsoleOverlay.Log($"Condições da nau melhoradas! Agora: {condition.currentValue:0}");
    }
}

