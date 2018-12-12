#region

using Newtonsoft.Json;

#endregion

namespace NextCanvas
{
    /// <summary>
    ///     This is the main window model *SWOOSH*
    /// </summary>
    public class MainWindowModel
    {
        [JsonIgnore] public Document Document { get; set; } = new Document();
    }
}