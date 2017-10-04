using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API.Controllers
{
    public class CategoryController : ApiController
    {
        UnitOfWork _unitOfWork = new UnitOfWork();

        [HttpGet]
        public CategorySearchResultDTO Search([FromUri] int pagesize, [FromUri] int pageindex)
        {
            var query = (from item in _unitOfWork.CategoryRepository.GetAll()
                         select new CategoryDTO()
                         {
                             Id = item.Id,
                             Title = item.Title,
                             Description = item.Description,
                             CreateTime = item.CreateTime,
                             LastUpdateTime = item.LastUpdateTime
                         });
            CategorySearchResultDTO result = new CategorySearchResultDTO();
            result.Total = query.Count();
            result.Results = query.OrderBy(x => x.Title).Skip(pagesize * (pageindex - 1)).Take(pagesize).ToList();
            return result;
        }

        [HttpGet]
        public List<CategoryDTO> GetAll()
        {
            return (from item in _unitOfWork.CategoryRepository.GetAll()
                    select new CategoryDTO()
                    {
                        Id = item.Id,
                        Title = item.Title,
                        Description = item.Description,
                        CreateTime = item.CreateTime,
                        LastUpdateTime = item.LastUpdateTime
                    }).OrderBy(x=>x.Title).ToList();
        }

        [HttpGet]
        public CategoryDTO Get(int id)
        {
            var entity = _unitOfWork.CategoryRepository.GetByID(id);
            if (entity != null)
            {
                return new CategoryDTO()
                {
                    Id = entity.Id,
                    Title = entity.Title,
                    Description = entity.Description
                };
            }
            return null;
        }

        [HttpPost]
        public ResponseDTO Add(CategoryDTO dto)
        {
            Category entity = _unitOfWork.CategoryRepository.GetAll().Where(x => x.Title.Equals(dto.Title)).FirstOrDefault();
            if (entity != null)
            {
                return new ResponseDTO() { Message = "Duplicate" };
            }
            try
            {
                entity = new Category()
                {
                    Title = dto.Title,
                    Description = dto.Description,
                    CreateTime = DateTime.Now,
                    LastUpdateTime = DateTime.Now
                };
                _unitOfWork.CategoryRepository.Add(entity);
                _unitOfWork.Commit();
                return new ResponseDTO() { Message = "Success" };
            }
            catch (Exception ex)
            {
                return new ResponseDTO() { Message = ex.Message };
            }
        }

        [HttpPut]
        public ResponseDTO Update(CategoryDTO dto)
        {
            try
            {
                bool isDuplicate = _unitOfWork.CategoryRepository.GetAll().Any(x => x.Title.Equals(dto.Title) && x.Id != dto.Id);
                if (isDuplicate)
                {
                    return new ResponseDTO() { Message = "Duplicate" };
                }
                Category entity = _unitOfWork.CategoryRepository.GetByID(dto.Id);
                if (entity != null)
                {
                    entity.Title = dto.Title;
                    entity.Description = dto.Description;
                    entity.LastUpdateTime = DateTime.Now;

                    _unitOfWork.CategoryRepository.Update(entity);
                    _unitOfWork.Commit();
                    return new ResponseDTO() { Message = "Success" };
                }
                else
                {
                    return new ResponseDTO() { Message = "NotExist" };
                }
            }
            catch (Exception ex)
            {
                return new ResponseDTO() { Message = ex.Message };
            }
        }
        [HttpDelete]
        public ResponseDTO Delete(int id)
        {
            try
            {
                bool isInUse = _unitOfWork.BookRepository.GetAll().Any(x => x.CategoryId == id);

                if (isInUse)
                {
                    return new ResponseDTO() { Message = "InUse" };
                }
                Category entity = _unitOfWork.CategoryRepository.GetByID(id);
                if (entity != null)
                {
                    _unitOfWork.CategoryRepository.Delete(entity);
                    _unitOfWork.Commit();
                    return new ResponseDTO() { Message = "Success" };
                }
                else
                {
                    return new ResponseDTO() { Message = "NotExist" };
                }
            }
            catch (Exception ex)
            {
                return new ResponseDTO() { Message = ex.Message };
            }
        }
    }
}
