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
    public class CategoryBusiness : BaseBusiness
    {
        public CategorySearchResultDTO Search(CategorySearchCriteriaDTO criteria)
        {
            var query = UnitOfWork.CategoryRepository.GetAll();
            CategorySearchResultDTO result = new CategorySearchResultDTO();
            result.Total = query.Count();
            result.Results = query.OrderBy(x => x.Title)
                .Skip(criteria.PageSize * (criteria.PageIndex - 1))
                .Take(criteria.PageSize).ToList()
                .Select(c => Mapper.ToDTO(c)).ToList();
            return result;
        }
        public List<CategoryDTO> GetAll()
        {
            return UnitOfWork.CategoryRepository.GetAll().ToList().Select(x => Mapper.ToDTO(x)).ToList();
        }
        public CategoryDTO Get(int id)
        {
            return Mapper.ToDTO(UnitOfWork.CategoryRepository.GetByID(id));
        }
        public ResponseDTO Add(CategoryDTO dto)
        {
            Category entity = UnitOfWork.CategoryRepository.GetAll().Where(x => x.Title.Equals(dto.Title)).FirstOrDefault();
            if (entity != null)
            {
                return new ResponseDTO() { ResponseCode = Enums.ResponseCode.Duplicate };
            }
            try
            {
                entity = new Category();
                Mapper.ToEntity(entity, dto);
                entity.CreateTime = DateTime.Now;
                UnitOfWork.CategoryRepository.Add(entity);
                UnitOfWork.Commit();
                return new ResponseDTO() { ResponseCode = Enums.ResponseCode.Success };
            }
            catch (Exception ex)
            {
                return new ResponseDTO() { ResponseCode = Enums.ResponseCode.Error, Message = ex.Message };
            }
        }
        public ResponseDTO Update(CategoryDTO dto)
        {
            try
            {
                bool isDuplicate = UnitOfWork.CategoryRepository.GetAll().Any(x => x.Title.Equals(dto.Title) && x.Id != dto.Id);
                if (isDuplicate)
                {
                    return new ResponseDTO() { ResponseCode = Enums.ResponseCode.Duplicate };
                }
                Category entity = UnitOfWork.CategoryRepository.GetByID(dto.Id);

                if (entity != null)
                {
                    Mapper.ToEntity(entity, dto);
                    UnitOfWork.CategoryRepository.Update(entity);
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
                bool isInUse = UnitOfWork.BookRepository.GetAll().Any(x => x.CategoryId == id);

                if (isInUse)
                {
                    return new ResponseDTO() { ResponseCode = Enums.ResponseCode.InUse };
                }
                Category entity = UnitOfWork.CategoryRepository.GetByID(id);
                if (entity != null)
                {
                    UnitOfWork.CategoryRepository.Delete(entity);
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
