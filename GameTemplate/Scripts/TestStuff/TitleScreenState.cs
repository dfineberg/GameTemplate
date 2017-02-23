public class TitleScreenState : AbstractMenuState
{
    public TitleScreenState() : base("Test/UI/TitleScreen")
    {
    }

    protected override void HandleUiButtonPressed(int i)
    {
        NextState = new LoadSceneState();
    }
}