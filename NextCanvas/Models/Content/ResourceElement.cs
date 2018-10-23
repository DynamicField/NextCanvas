namespace NextCanvas.Models.Content
{
    public abstract class ResourceElement : ContentElement
    {
        public Resource Resource { get; set; }

        protected ResourceElement()
        {
        }

        protected ResourceElement(Resource resource)
        {
            Resource = resource;
        }
    }
}