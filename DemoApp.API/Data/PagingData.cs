using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DemoApp.API.Data
{
    public class PagingParams
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

    public class LinkInfo
    {
        public string Href { get; set; }
        public string Rel { get; set; }
        public string Method { get; set; }
    }

    public class PagingHeader
    {
        public PagingHeader(int totalItems, int pageNumber, int pageSize, int totalPages)
        {
            this.TotalItems = totalItems;
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
            this.TotalPages = totalPages;
        }
        public int TotalItems { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public string ToJson() => JsonConvert.SerializeObject(this, new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        });
    }
}