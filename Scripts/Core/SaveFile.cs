using System;
using UnityEngine;

[Serializable]
public struct SaveFile
{
    // character customisation
    public int CharacterType; // 0: boy, 1: girl
    public string Name;
    public int Helmet; // -1 means default helmet
    public int Suit;

    // planets progress
    public int[] PlanetProgress;
    
    // unlocks
    public bool[] HelmetUnlocks;
    public bool[] SuitUnlocks;

    private void CheckArrays()
    {
        if (PlanetProgress == null)
            PlanetProgress = new int[GameManager.PlanetDefinitions.Length];
        else if(PlanetProgress.Length < GameManager.PlanetDefinitions.Length)
            Array.Resize(ref PlanetProgress, GameManager.PlanetDefinitions.Length);

        if(HelmetUnlocks == null)
            HelmetUnlocks = new bool[GameManager.ClothingLibrary.Helmets.Length];
        else if(HelmetUnlocks.Length < GameManager.ClothingLibrary.Helmets.Length)
            Array.Resize(ref HelmetUnlocks, GameManager.ClothingLibrary.Helmets.Length);
        
        if(SuitUnlocks == null)
            SuitUnlocks = new bool[GameManager.ClothingLibrary.Suits.Length];
        else if(SuitUnlocks.Length < GameManager.ClothingLibrary.Suits.Length)
            Array.Resize(ref SuitUnlocks, GameManager.ClothingLibrary.Suits.Length);

        SuitUnlocks[0] = true;
    }

    public void PlanetVisited(int planetNo)
    {
        CheckArrays();

        PlanetProgress[planetNo]++;
    }

    public int SelectNextHelmet()
    {
        CheckArrays();
        
        do
        {
            Helmet++;

            if (Helmet == HelmetUnlocks.Length)
                Helmet = -1;

        } while (Helmet != -1 && !HelmetUnlocks[Helmet]);

        return Helmet;
    }

    public int SelectPreviousHelmet()
    {
        CheckArrays();

        do
        {
            Helmet--;

            if (Helmet == -2)
                Helmet = HelmetUnlocks.Length - 1;

        } while (Helmet != -1 && !HelmetUnlocks[Helmet]);

        return Helmet;
    }

    public int SelectNextSuit()
    {
        CheckArrays();

        do
        {
            Suit = (Suit + 1) % SuitUnlocks.Length;
        } while (!SuitUnlocks[Suit]);

        return Suit;
    }

    public int SelectPreviousSuit()
    {
        CheckArrays();

        do
        {
            Suit--;

            if (Suit == -1)
                Suit = SuitUnlocks.Length - 1;
        } while (!SuitUnlocks[Suit]);

        return Suit;
    }

    public float GetPlanetProgress(int planetNo)
    {
        CheckArrays();
        
        var f = (float) PlanetProgress[planetNo] / GameManager.PlanetDefinitions[planetNo].PlaysToComplete;
        return Mathf.Clamp01(f);
    }

    public bool IsPlanetUnlocked(int planetNo)
    {
        CheckArrays();
        
        if (planetNo == 0)
            return true;

        return GetPlanetProgress(planetNo - 1) >= 0.5f;
    }

    public bool[] GetPlanetUnlocks()
    {
        CheckArrays();
        
        var unlocks = new bool[GameManager.PlanetDefinitions.Length];

        for (var i = 0; i < unlocks.Length; i++)
            unlocks[i] = IsPlanetUnlocked(i);

        return unlocks;
    }
}