public class SaveManager : GenericSaveManager<SaveFile>
{
    public override void CreateNewSaveFile()
    {
        base.CreateNewSaveFile();
        SaveFile.Helmet = -1;
    }
}
