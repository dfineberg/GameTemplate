using System;
using UnityEngine;

[Serializable]
public struct SaveFile
{
    // character customisation
    public int CharacterType; // 0: boy, 1: girl

    public string Name;

    // planets progress
    public int[] PlanetProgress;

    private void CheckArray()
    {
        if (PlanetProgress == null)
            PlanetProgress = new int[GameManager.PlanetDefinitions.Length];
        else if(PlanetProgress.Length < GameManager.PlanetDefinitions.Length)
            Array.Resize(ref PlanetProgress, GameManager.PlanetDefinitions.Length);
    }

    public void PlanetVisited(int planetNo)
    {
        CheckArray();

        PlanetProgress[planetNo]++;
    }

    public float GetPlanetProgress(int planetNo)
    {
        CheckArray();
        
        var f = (float) PlanetProgress[planetNo] / GameManager.PlanetDefinitions[planetNo].PlaysToComplete;
        return Mathf.Clamp01(f);
    }

    public bool IsPlanetUnlocked(int planetNo)
    {
        CheckArray();
        
        if (planetNo == 0)
            return true;

        return GetPlanetProgress(planetNo - 1) >= 0.5f;
    }

    public bool[] GetPlanetUnlocks()
    {
        CheckArray();
        
        var unlocks = new bool[GameManager.PlanetDefinitions.Length];

        for (var i = 0; i < unlocks.Length; i++)
            unlocks[i] = IsPlanetUnlocked(i);

        return unlocks;
    }
}