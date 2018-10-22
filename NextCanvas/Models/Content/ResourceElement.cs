namespace NextCanvas.Models.Content
{
    public class ResourceElement : ContentElement
    {
        public ResourceElement()
        {
            
        }
        public ResourceElement(Resource resource)
        {
            Resource = resource;
        }

        public Resource Resource { get; set; }
    }
}