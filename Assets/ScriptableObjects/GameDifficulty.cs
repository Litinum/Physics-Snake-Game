using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

[CreateAssetMenu(fileName = "GameDifficulty", menuName = "Scriptable Objects/GameSettings/GameDifficulty")]
public class GameDifficulty : ScriptableObject
{
    public DifficultySettings difficultySettings { get; private set; }
    public List<Food> foodToSpawn = new List<Food>();
    [Range(0, 30)] public float speedPercIncrease;
    public int speedIncreaseAfter;

    public void SetGameDifficulty(DifficultySettings NewDifficultySettings)
    {
        difficultySettings = NewDifficultySettings;
        foodToSpawn.Clear();

        Food food = Resources.Load<Food>("Scriptables/Food/StaticFood");
        Food mouseFood = Resources.Load<Food>("Scriptables/Food/Mouse");

        switch (difficultySettings)
        {
            case DifficultySettings.Easy:
                speedPercIncrease = 0;
                speedIncreaseAfter = 0;
                food.maxToSpawn = 10;
                break;
            case DifficultySettings.Medium:
                speedPercIncrease = 0.5f;
                speedIncreaseAfter = 7;
                food.maxToSpawn = 10;
                mouseFood.maxToSpawn = 3;
                foodToSpawn.Add(mouseFood);
                break;
            case DifficultySettings.Hard:
                speedPercIncrease = 2f;
                speedIncreaseAfter = 5;
                food.maxToSpawn = 3;
                mouseFood.maxToSpawn = 7;
                foodToSpawn.Add(mouseFood);
                break;
        }

        foodToSpawn.Add(food);
    }
}

public enum DifficultySettings
{
    Easy,
    Medium,
    Hard
}
