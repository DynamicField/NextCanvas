namespace NextCanvas.Interactivity
{
    public interface IModifyObjectInteraction : IUserInteraction
    {
        object ObjectToModify { get; set; }
        string HeaderText { get; set; }
        string HeaderStart { get; set; }
        bool IsObjectCreation { get; set; }
    }
}