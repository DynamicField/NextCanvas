namespace NextCanvas.Interactivity
{
    public interface IModifyObjectInteraction : IUserInteraction
    {
        object ObjectToModify { get; set; }
    }
}