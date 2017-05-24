using System;

[Serializable]
public struct SaveFile
{
    // character customisation
    public int CharacterType; // 0: boy, 1: girl

    public string Name;

    // planets progress
    public int[] PlanetProgress;

    public float GetPlanetProgress(int planetNo)
    {
        return (float) PlanetProgress[planetNo] / GameManager.PlanetDefinitions[planetNo].PlaysToComplete;
    }

    public bool IsPlanetUnlocked(int planetNo)
    {
        if (planetNo == 0)
            return true;

        return GetPlanetProgress(planetNo - 1) > 0.5f;
    }

    public bool[] GetPlanetUnlocks()
    {
        var unlocks = new bool[GameManager.PlanetDefinitions.Length];

        for (var i = 0; i < unlocks.Length; i++)
            unlocks[i] = IsPlanetUnlocked(i);

        return unlocks;
    }
}