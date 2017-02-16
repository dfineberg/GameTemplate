using System;
using System.Collections;
using System.Collections.Generic;
using Promises;
using UnityEngine;

public class TitleScreenState : AbstractUIState
{
    public TitleScreenState() : base("Test/UI/TitleScreen") { }

    protected override void HandleUIButtonPressed(int i)
    {
        nextState = new LoadSceneState();
    }
}
