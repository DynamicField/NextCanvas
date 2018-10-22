namespace NextCanvas.Models.Content
{
    public abstract class ResourceElement : ContentElement
    {
        protected ResourceElement()
        {
            
        }

        protected ResourceElement(Resource resource)
        {
            Resource = resource;
        }

        public Resource Resource { get; set; }
    }
}