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
    public class CategoryBusiness : BaseBusiness
    {
        public CategorySearchResultModel Search(BasePagingModel model)
        {
            string url = "api/Category/Search?pagesize=" + model.PageSize + "&pageindex=" + model.PageIndex;
            return DoRequest<CategorySearchResultModel, CategorySearchResultModel>(url, Enums.RequestType.Get, null);
        }
        public List<SelectionItem> GetSelectListCategory()
        {
            List<SelectionItem> list = new List<SelectionItem>();
            list.Add(new SelectionItem() { ValueMember = 0, DisplayMember = string.Empty });
            List<CategoryModel> categories = GetAll();
            if (categories != null)
            {
                list.AddRange((from au in categories select new SelectionItem() { ValueMember = au.Id, DisplayMember = au.Title }).ToList());
            }
            return list;
        }
        public List<CategoryModel> GetAll()
        {
            return DoRequest<List<CategoryModel>, List<CategoryModel>>("api/Category/GetAll", Enums.RequestType.Get, null);
        }
        public CategoryModel Get(int id)
        {
            return DoRequest<CategoryModel, AuthorModel>("api/Category/Get/" + id, Enums.RequestType.Get, null);
        }
        public ResponseModel Add(CategoryModel model)
        {
            return DoRequest<ResponseModel, CategoryModel>("api/Category/Add/", Enums.RequestType.Post, model);
        }
        public ResponseModel Update(CategoryModel model)
        {
            return DoRequest<ResponseModel, CategoryModel>("api/Category/Update/", Enums.RequestType.Put, model);
        }
        public ResponseModel Delete(int id)
        {
            return DoRequest<ResponseModel, ResponseModel>("api/Category/Delete/" + id, Enums.RequestType.Delete, null);
        }
    }
}
