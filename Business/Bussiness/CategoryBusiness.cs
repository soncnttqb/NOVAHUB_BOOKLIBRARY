using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public static class CategoryBusiness
    {
        public static CategorySearchResultModel Search(BasePagingModel model)
        {
            using (var client = new HttpClient())
            {
                Utils.ConfigHttpClient(client);

                HttpResponseMessage response = client.GetAsync("api/Category/Search?pagesize=" + model.PageSize + "&pageindex=" + model.PageIndex).Result;
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsAsync<CategorySearchResultModel>().Result;
                }
            }
            return new CategorySearchResultModel();
        }
        public static List<SelectionItem> GetSelectListCategory()
        {
            List<SelectionItem> list = new List<SelectionItem>();
            list.Add(new SelectionItem() { ValueMember = 0, DisplayMember = string.Empty });
            List<CategoryModel> categories = CategoryBusiness.GetAll();
            if (categories != null)
            {
                list.AddRange((from au in categories select new SelectionItem() { ValueMember = au.Id, DisplayMember = au.Title }).ToList());
            }
            return list;
        }
        public static List<CategoryModel> GetAll()
        {
            using (HttpClient client = new HttpClient())
            {
                Utils.ConfigHttpClient(client);
                HttpResponseMessage response = client.GetAsync("api/Category/GetAll").Result;
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsAsync<List<CategoryModel>>().Result;
                }
            }
            return null;
        }
        public static CategoryModel Get(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                Utils.ConfigHttpClient(client);
                HttpResponseMessage response = client.GetAsync("api/Category/Get/" + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsAsync<CategoryModel>().Result;
                }
            }
            return null;
        }
        public static ResponseModel Add(CategoryModel model)
        {
            using (HttpClient client = new HttpClient())
            {
                Utils.ConfigHttpClient(client);
                HttpResponseMessage response = client.PostAsJsonAsync("api/Category/Add", model).Result;
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsAsync<ResponseModel>().Result;
                }
            }
            return null;
        }
        public static ResponseModel Update(CategoryModel model)
        {
            using (HttpClient client = new HttpClient())
            {
                Utils.ConfigHttpClient(client);
                HttpResponseMessage response = client.PutAsJsonAsync("api/Category/Update", model).Result;
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
                HttpResponseMessage response = client.DeleteAsync("api/Category/Delete/" + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsAsync<ResponseModel>().Result;
                }
            }
            return null;
        }
    }
}
