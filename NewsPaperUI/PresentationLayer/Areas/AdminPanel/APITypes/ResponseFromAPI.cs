namespace PresentationLayer.Areas.AdminPanel.APITypes
{
    public class ResponseFromAPI<T>
    {
        public T Data { get; set; }
        public int StatusCode { get; set; }
        public List<string> ErrorMessages { get; set; } 
    }
}
