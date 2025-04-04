using PresentationLayer.Areas.AdminPanel.APITypes;

namespace PresentationLayer.ApiServices
{
    public interface IApiService
    {
        Task<ResponseFromAPI<T>> GetData<T>(string endPoint, string token = null);
        Task<bool> PostData(string endPoint, string jsonData, string token = null);
        Task<bool> PutData(string endPoint, string jsonData, string token = null);
        Task<bool> DeleteData(string endPoint, string token = null);
    }
}
