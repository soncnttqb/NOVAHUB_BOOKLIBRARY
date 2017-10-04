using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public static class AuthorBusiness
    {
        public static AuthorSearchResultModel Search(BasePagingModel model)
        {
            using (var client = new HttpClient())
            {
                Utils.ConfigHttpClient(client);

                HttpResponseMessage response = client.GetAsync("api/Author/Search?pagesize=" + model.PageSize + "&pageindex="+ model.PageIndex).Result;
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsAsync<AuthorSearchResultModel>().Result;
                }
            }
            return new AuthorSearchResultModel();
        }

        public static List<SelectionItem> GetSelectListAuthor()
        {
            List<SelectionItem> list = new List<SelectionItem>();
            list.Add(new SelectionItem() { ValueMember = 0, DisplayMember = string.Empty });
            List<AuthorModel> authors = AuthorBusiness.GetAll();
            if (authors != null)
            {
                list.AddRange((from au in authors select new SelectionItem() { ValueMember = au.Id, DisplayMember = au.Title }).ToList());
            }
            return list;
        }
        public static List<AuthorModel> GetAll()
        {
            using (HttpClient client = new HttpClient())
            {
                Utils.ConfigHttpClient(client);
                HttpResponseMessage response = client.GetAsync("api/Author/GetAll").Result;
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsAsync<List<AuthorModel>>().Result;
                }
            }
            return null;
        }
        public static AuthorModel Get(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                Utils.ConfigHttpClient(client);
                HttpResponseMessage response = client.GetAsync("api/Author/Get/" + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsAsync<AuthorModel>().Result;
                }
            }
            return null;
        }
        public static ResponseModel Add(AuthorModel model)
        {
            using (HttpClient client = new HttpClient())
            {
                Utils.ConfigHttpClient(client);
                HttpResponseMessage response = client.PostAsJsonAsync("api/Author/Add", model).Result;
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsAsync<ResponseModel>().Result;
                }
            }
            return null;
        }
        public static ResponseModel Update(AuthorModel model)
        {
            using (HttpClient client = new HttpClient())
            {
                Utils.ConfigHttpClient(client);
                HttpResponseMessage response = client.PutAsJsonAsync("api/Author/Update", model).Result;
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
                HttpResponseMessage response = client.DeleteAsync("api/Author/Delete/" + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsAsync<ResponseModel>().Result;
                }
            }
            return null;
        }
    }
}
