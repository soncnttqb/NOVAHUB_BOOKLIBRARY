using Business.Models;
using Business.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Business.Bussiness
{
    public class AuthorBusiness:BaseBusiness
    {
        public AuthorSearchResultModel Search(BasePagingModel model)
        {
            string url = "api/Author/Search?pagesize=" + model.PageSize + "&pageindex=" + model.PageIndex;
            return DoRequest<AuthorSearchResultModel, AuthorSearchResultModel>(url, Enums.RequestType.Get, null);
        }

        public List<SelectionItem> GetSelectListAuthor()
        {
            List<SelectionItem> list = new List<SelectionItem>();
            list.Add(new SelectionItem() { ValueMember = 0, DisplayMember = string.Empty });
            List<AuthorModel> authors = GetAll();
            if (authors != null)
            {
                list.AddRange((from au in authors select new SelectionItem() { ValueMember = au.Id, DisplayMember = au.Title }).ToList());
            }
            return list;
        }
        public List<AuthorModel> GetAll()
        {
            return DoRequest<List<AuthorModel>, List<AuthorModel>>("api/Author/GetAll", Enums.RequestType.Get, null);
            //using (HttpClient client = new HttpClient())
            //{
            //    Utils.ConfigHttpClient(client);
            //    HttpResponseMessage response = client.GetAsync("api/Author/GetAll").Result;
            //    if (response.IsSuccessStatusCode)
            //    {
            //        return response.Content.ReadAsAsync<List<AuthorModel>>().Result;
            //    }
            //}
            //return null;
        }
        public AuthorModel Get(int id)
        {
            return DoRequest<AuthorModel, AuthorModel>("api/Author/Get/" + id, Enums.RequestType.Get, null);
        }
        public ResponseModel Add(AuthorModel model)
        {
            return DoRequest<ResponseModel, AuthorModel>("api/Author/Add/", Enums.RequestType.Post, model);
        }
        public ResponseModel Update(AuthorModel model)
        {
            return DoRequest<ResponseModel, AuthorModel>("api/Author/Update/", Enums.RequestType.Put, model);
        }
        public ResponseModel Delete(int id)
        {
            return DoRequest<ResponseModel, ResponseModel>("api/Author/Delete/" + id, Enums.RequestType.Delete, null);
        }
    }
}
