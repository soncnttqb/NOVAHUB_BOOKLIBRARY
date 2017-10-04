using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API.Controllers
{
    public class AuthorController : ApiController
    {
        UnitOfWork _unitOfWork = new UnitOfWork();

        [HttpGet]
        public AuthorSearchResultDTO Search([FromUri] int pagesize, [FromUri] int pageindex)
        {
            var query = (from item in _unitOfWork.AuthorRepository.GetAll()
                    select new AuthorDTO()
                    {
                        Id = item.Id,
                        Title = item.Title,
                        Description = item.Description,
                        CoverPhoto = item.CoverPhoto,
                        CreateTime = item.CreateTime,
                        LastUpdateTime = item.LastUpdateTime
                    });
            AuthorSearchResultDTO result = new AuthorSearchResultDTO();
            result.Total = query.Count();
            result.Results = query.OrderBy(x => x.Title).Skip(pagesize * (pageindex - 1)).Take(pagesize).ToList();
            return result;
        }

        [HttpGet]
        public List<AuthorDTO> GetAll()
        {
            return (from item in _unitOfWork.AuthorRepository.GetAll()
                    select new AuthorDTO()
                    {
                        Id = item.Id,
                        Title = item.Title,
                        Description = item.Description,
                        CoverPhoto = item.CoverPhoto,
                        CreateTime = item.CreateTime,
                        LastUpdateTime = item.LastUpdateTime
                    }).OrderBy(x=>x.Title).ToList();
        }

        [HttpGet]
        public AuthorDTO Get(int id)
        {
            var entity = _unitOfWork.AuthorRepository.GetByID(id);
            if (entity != null)
            {
                return new AuthorDTO()
                {
                    Id = entity.Id,
                    Title = entity.Title,
                    Description = entity.Description,
                    CoverPhoto = entity.CoverPhoto
                };
            }
            return null;
        }

        [HttpPost]
        public ResponseDTO Add(AuthorDTO dto)
        {
            try
            {
                _unitOfWork.AuthorRepository.Add(new Author()
                {
                    Title = dto.Title,
                    Description = dto.Description,
                    CoverPhoto=dto.CoverPhoto,
                    CreateTime = DateTime.Now,
                    LastUpdateTime = DateTime.Now
                });
                _unitOfWork.Commit();
                return new ResponseDTO() { Message = "Success" };
            }
            catch (Exception ex)
            {
                return new ResponseDTO() { Message = ex.Message };
            }
        }

        [HttpPut]
        public ResponseDTO Update(AuthorDTO dto)
        {
            try
            {
                Author entity = _unitOfWork.AuthorRepository.GetByID(dto.Id);
                if (entity != null)
                {
                    entity.Title = dto.Title;
                    entity.Description = dto.Description;
                    entity.CoverPhoto = dto.CoverPhoto;
                    entity.LastUpdateTime = DateTime.Now;

                    _unitOfWork.AuthorRepository.Update(entity);
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
                bool isInUse = _unitOfWork.BookRepository.GetAll().Any(x => x.AuthorId == id);

                if (isInUse)
                {
                    return new ResponseDTO() { Message = "InUse" };
                }
                Author entity = _unitOfWork.AuthorRepository.GetByID(id);
                if (entity != null)
                {
                    _unitOfWork.AuthorRepository.Delete(entity);
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
