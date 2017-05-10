using UnityEngine;
// ReSharper disable InconsistentNaming

public abstract class AbstractState
{
    public AbstractState NextState { get; protected set; }

    protected GameObject gameObject { get; private set; }

    protected Transform transform
    {
        get { return gameObject != null ? gameObject.transform : null; }
    }

    public abstract void OnEnter();

    public abstract void OnExit();

    public void SetGameObject(GameObject o)
    {
        gameObject = o;
    }
}