using UnityEngine;

[System.Serializable]
public class Resource
{
    public string name;
    public float currentValue = 100f;
    public float maxValue = 100f;
    public float drainPerSecond = 1f;

    public void Tick(float deltaTime)
    {
        currentValue -= drainPerSecond * deltaTime;
        currentValue = Mathf.Clamp(currentValue, 0f, maxValue);
    }
}

public class CrewResources : MonoBehaviour
{
    [Header("Resources")]
    public Resource hunger = new Resource { name = "Hunger" };
    public Resource thirst = new Resource { name = "Thirst" };
    public Resource morale = new Resource { name = "Morale" };

    private void Update()
    {
        float dt = Time.deltaTime;

        hunger.Tick(dt);
        thirst.Tick(dt);
        morale.Tick(dt);
    }

    public void ApplyMoraleBoost(float amount, float waterCost, float foodCost)
    {
        thirst.currentValue -= waterCost;
        hunger.currentValue -= foodCost;
        morale.currentValue += amount;

        thirst.currentValue = Mathf.Clamp(thirst.currentValue, 0, thirst.maxValue);
        hunger.currentValue = Mathf.Clamp(hunger.currentValue, 0, hunger.maxValue);
        morale.currentValue = Mathf.Clamp(morale.currentValue, 0, morale.maxValue);

        ConsoleOverlay.Log($"Moral aumentada!");
    }

}


