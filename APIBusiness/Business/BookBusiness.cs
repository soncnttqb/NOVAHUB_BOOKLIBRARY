using APIBusiness.DataTransferObject;
using APIBusiness.Utilities;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace APIBusiness.Business
{
    public class BookBusiness : BaseBusiness
    {
        public BookSearchResultDTO Search(BookSearchCriteriaDTO criteria)
        {
            IQueryable<Book> query = UnitOfWork.BookRepository.GetAll();
            if (!string.IsNullOrEmpty(criteria.Title)) query = query.Where(x => x.Title.Contains(criteria.Title));
            if (!string.IsNullOrEmpty(criteria.Description)) query = query.Where(x => !string.IsNullOrEmpty(x.Description) && x.Description.Contains(criteria.Description));
            if (!string.IsNullOrEmpty(criteria.Publisher)) query = query.Where(x => !string.IsNullOrEmpty(x.Publisher) && x.Publisher.Contains(criteria.Publisher));
            if (!string.IsNullOrEmpty(criteria.Year)) query = query.Where(x => x.Year.ToString().Contains(criteria.Year));
            if (!string.IsNullOrEmpty(criteria.Category)) query = query.Where(x => x.Category.Title.Contains(criteria.Category));
            if (!string.IsNullOrEmpty(criteria.Author)) query = query.Where(x => x.Author.Title.Contains(criteria.Author));

            BookSearchResultDTO result = new BookSearchResultDTO();
            result.Total = query.Count();
            result.Results = query.OrderBy(x => x.Title)
                .Skip(criteria.PageSize * (criteria.PageIndex - 1))
                .Take(criteria.PageSize).ToList()
                .Select(c => Mapper.ToDTO(c)).ToList();
            return result;
        }

        public BookDTO Get(int id)
        {
            return Mapper.ToDTO(UnitOfWork.BookRepository.GetByID(id));
        }

        public ResponseDTO Add(BookDTO dto)
        {
            try
            {
                Book entity = new Book();
                Mapper.ToEntity(entity, dto);
                entity.CreateTime = DateTime.Now;
                UnitOfWork.BookRepository.Add(entity);
                UnitOfWork.Commit();
                return new ResponseDTO() { ResponseCode = Enums.ResponseCode.Success };
            }
            catch (Exception ex)
            {
                return new ResponseDTO() { ResponseCode = Enums.ResponseCode.Error, Message = ex.Message };
            }
        }
        
        public ResponseDTO Update(BookDTO dto)
        {
            try
            {
                Book entity = UnitOfWork.BookRepository.GetByID(dto.Id);
                if (entity != null)
                {
                    Mapper.ToEntity(entity, dto);
                    UnitOfWork.BookRepository.Update(entity);
                    UnitOfWork.Commit();
                    return new ResponseDTO() { ResponseCode = Enums.ResponseCode.Success };
                }
                else
                {
                    return new ResponseDTO() { ResponseCode = Enums.ResponseCode.NotExist };
                }
            }
            catch (Exception ex)
            {
                return new ResponseDTO() { ResponseCode = Enums.ResponseCode.Error, Message = ex.Message };
            }
        }
        public ResponseDTO Delete(int id)
        {
            try
            {
                Book entity = UnitOfWork.BookRepository.GetByID(id);
                if (entity != null)
                {
                    UnitOfWork.BookRepository.Delete(entity);
                    UnitOfWork.Commit();
                    return new ResponseDTO() { ResponseCode = Enums.ResponseCode.Success };
                }
                else
                {
                    return new ResponseDTO() { ResponseCode = Enums.ResponseCode.NotExist };
                }
            }
            catch (Exception ex)
            {
                return new ResponseDTO() { ResponseCode = Enums.ResponseCode.Error, Message = ex.Message };
            }
        }
    }
}
