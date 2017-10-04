using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public static class BookBusiness
    {
        public static BookSearchResultModel Search(BookSearchCriteriaModel model)
        {
            using (var client = new HttpClient())
            {
                Utils.ConfigHttpClient(client);

                HttpResponseMessage response = client.GetAsync("api/Book/Search?title="
                    + model.Title + "&des="
                    + model.Description + "&filter="
                    + model.Filter + "&pagesize="
                    + model.PageSize + "&pageindex="
                    + model.PageIndex).Result;
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsAsync<BookSearchResultModel>().Result;
                }
            }
            return new BookSearchResultModel();
        }
        public static List<BookModel> GetAll()
        {
            using (HttpClient client = new HttpClient())
            {
                Utils.ConfigHttpClient(client);
                HttpResponseMessage response = client.GetAsync("api/Book/GetAll").Result;
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsAsync<List<BookModel>>().Result;
                }
            }
            return null;
        }
        public static BookModel Get(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                Utils.ConfigHttpClient(client);
                HttpResponseMessage response = client.GetAsync("api/Book/Get/" + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsAsync<BookModel>().Result;
                }
            }
            return null;
        }
        public static ResponseModel Add(BookModel model)
        {
            using (HttpClient client = new HttpClient())
            {
                Utils.ConfigHttpClient(client);
                HttpResponseMessage response = client.PostAsJsonAsync("api/Book/Add", model).Result;
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsAsync<ResponseModel>().Result;
                }
            }
            return null;
        }
        public static ResponseModel Update(BookModel model)
        {
            using (HttpClient client = new HttpClient())
            {
                Utils.ConfigHttpClient(client);
                HttpResponseMessage response = client.PutAsJsonAsync("api/Book/Update", model).Result;
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsAsync<ResponseModel>().Result;
                }
            }
            return null;
        }
        public static ResponseModel Delete(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                Utils.ConfigHttpClient(client);
                HttpResponseMessage response = client.DeleteAsync("api/Book/Delete/" + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsAsync<ResponseModel>().Result;
                }
            }
            return null;
        }
    }
}
