
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
    public class BookBusiness : BaseBusiness
    {
        public BookSearchResultModel Search(BookSearchCriteriaModel model)
        {
            string url = "api/Book/Search?title=" + model.Title
                    + "&description=" + model.Description
                    + "&publisher=" + model.Publisher
                    + "&year=" + model.Year
                    + "&category=" + model.Category
                    + "&author=" + model.Author
                    + "&pagesize=" + model.PageSize
                    + "&pageindex=" + model.PageIndex;
            return DoRequest<BookSearchResultModel, BookSearchResultModel>(url, Enums.RequestType.Get, null);
        }
        public BookModel Get(int id)
        {
            return DoRequest<BookModel, BookModel>("api/Book/Get/" + id, Enums.RequestType.Get, null);
        }
        public ResponseModel Add(BookModel model)
        {
            return DoRequest<ResponseModel, BookModel>("api/Book/Add", Enums.RequestType.Post, model);
        }
        public ResponseModel Update(BookModel model)
        {
            return DoRequest<ResponseModel, BookModel>("api/Book/Update", Enums.RequestType.Put, model);
        }
        public ResponseModel Delete(int id)
        {
            return DoRequest<ResponseModel, BookModel>("api/Book/Delete/" + id, Enums.RequestType.Delete, null);
        }
    }
}
