using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

[Serializable]
public struct SaveFile
{
    // character customisation
    public int CharacterType; // 0: boy, 1: girl
    public string Name;
    public int Helmet; // -1 means default helmet
    public int Suit;
    
    // tutorial
    public bool MainMenuTutorial;
    public bool PlanetTutorial;

    // planets progress
    public int[] PlanetProgress;
    
    // puzzle progress
    public int CurrentPuzzle;
    public bool[][] PuzzleProgress;
    public string LastDate;
    public bool MorningPuzzleComplete;
    public bool EveningPuzzleComplete;
    
    // unlocks
    public bool[] HelmetUnlocks;
    public bool[] SuitUnlocks;

    public int lastPuzzleCount;
    public int lastClothingCount;

    public void CheckArrays()
    {
        if (PlanetProgress == null)
            PlanetProgress = new int[GameManager.PlanetDefinitions.Length];
        else if (PlanetProgress.Length < GameManager.PlanetDefinitions.Length)
            Array.Resize(ref PlanetProgress, GameManager.PlanetDefinitions.Length);

        if (HelmetUnlocks == null)
            HelmetUnlocks = new bool[GameManager.ClothingLibrary.Helmets.Length];
        else if (HelmetUnlocks.Length < GameManager.ClothingLibrary.Helmets.Length)
            Array.Resize(ref HelmetUnlocks, GameManager.ClothingLibrary.Helmets.Length);

//        for (var i = 0; i < HelmetUnlocks.Length; i++)
//            HelmetUnlocks[i] = true;

        if (SuitUnlocks == null)
            SuitUnlocks = new bool[GameManager.ClothingLibrary.Suits.Length];
        else if (SuitUnlocks.Length < GameManager.ClothingLibrary.Suits.Length)
            Array.Resize(ref SuitUnlocks, GameManager.ClothingLibrary.Suits.Length);

//        for (var i = 0; i < SuitUnlocks.Length; i++)
//            SuitUnlocks[i] = true;

        SuitUnlocks[0] = true;

        if (PuzzleProgress == null)
        {
            PuzzleProgress = new bool[GameManager.PuzzleLibrary.Strings.Length][];

            for (var i = 0; i < PuzzleProgress.Length; i++)
                PuzzleProgress[i] = new bool[12];
        }
        else if (PuzzleProgress.Length < GameManager.PuzzleLibrary.Strings.Length)
        {
            var oldLength = PuzzleProgress.Length;

            Array.Resize(ref PuzzleProgress, GameManager.PuzzleLibrary.Strings.Length);

            for (var i = oldLength; i < PuzzleProgress.Length; i++)
                PuzzleProgress[i] = new bool[12];
        }
        
        var todayString = DateTime.Today.ToString(CultureInfo.InvariantCulture);

        if (todayString != LastDate)
        {
            LastDate = todayString;
            MorningPuzzleComplete = false;
            EveningPuzzleComplete = false;
        }
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

        return GetPlanetProgress(planetNo - 1) >= 0.3f;
    }

    public bool[] GetPlanetUnlocks()
    {
        CheckArrays();
        
        var unlocks = new bool[GameManager.PlanetDefinitions.Length];

        for (var i = 0; i < unlocks.Length; i++)
            unlocks[i] = IsPlanetUnlocked(i);

        return unlocks;
    }

    public bool[] GetCurrentPuzzleUnlocks()
    {
        CheckArrays();
        
        return PuzzleProgress[CurrentPuzzle];
    }

    public List<int> GetCurrentLockedPuzzlePieces()
    {
        CheckArrays();
        
        var pieces = new List<int>();

        for (var i = 0; i < PuzzleProgress[CurrentPuzzle].Length; i++)
            if (!PuzzleProgress[CurrentPuzzle][i])
                pieces.Add(i);

        return pieces;
    }

    public void UnlockCurrentPuzzlePiece(int pieceNo)
    {
        CheckArrays();
        
        PuzzleProgress[CurrentPuzzle][pieceNo] = true;
    }

    public bool GetPuzzleComplete(int puzzleNo)
    {
        CheckArrays();
        
        return PuzzleProgress[puzzleNo].All(b => b);
    }

    public bool GetAllPuzzlesComplete()
    {
        CheckArrays();

        for (var i = 0; i < PuzzleProgress.Length; i++)
            if (!GetPuzzleComplete(i))
                return false;

        return true;
    }

    public void IncrementPuzzle()
    {
        CheckArrays();

        CurrentPuzzle = Mathf.Min(CurrentPuzzle + 1, GameManager.PuzzleLibrary.Strings.Length - 1);
    }
}