using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API.Controllers
{
    public class BookController : ApiController
    {
        UnitOfWork _unitOfWork = new UnitOfWork();
        [HttpGet]
        public BookSearchResultDTO Search([FromUri] string title, [FromUri] string des, [FromUri] string filter, [FromUri] int pagesize,[FromUri] int pageindex)
        {
            var query = (from item in _unitOfWork.BookRepository.GetAll()
                         select new BookDTO()
                         {
                             Id = item.Id,
                             Title = item.Title,
                             Description = item.Description,
                             CoverPhoto = item.CoverPhoto,
                             CreateTime = item.CreateTime,
                             Publisher = item.Publisher,
                             LastUpdateTime = item.LastUpdateTime,
                             CategoryId = item.CategoryId,
                             AuthorId = item.AuthorId,
                             Year = item.Year,
                             Author = new AuthorDTO()
                             {
                                 Id = item.Author.Id,
                                 Title = item.Author.Title,
                                 Description = item.Author.Description,
                                 CoverPhoto = item.Author.CoverPhoto
                             },
                             Category = new CategoryDTO()
                             {
                                 Id = item.Category.Id,
                                 Title = item.Category.Title,
                                 Description = item.Category.Description
                             }
                         }).Where(x=> 
                         (string.IsNullOrEmpty(title) || x.Title.Contains(title)) 
                         && (string.IsNullOrEmpty(des) || x.Description.Contains(des))
                         && (string.IsNullOrEmpty(filter) || (!string.IsNullOrEmpty(filter) &&(x.Category.Title.Contains(filter) || x.Author.Title.Contains(filter) || x.Publisher.Contains(filter) ||x.Year.ToString().Contains(filter) ))));

            BookSearchResultDTO result = new BookSearchResultDTO();
            result.Total = query.Count();
            result.Results = query.OrderBy(x => x.Title).Skip(pagesize * (pageindex - 1)).Take(pagesize).ToList();
            return result;
        }
        [HttpGet]
        public List<BookDTO> GetAll()
        {
            return (from item in _unitOfWork.BookRepository.GetAll()
                    select new BookDTO()
                    {
                        Id = item.Id,
                        Title = item.Title,
                        Description = item.Description,
                        CoverPhoto = item.CoverPhoto,
                        CreateTime = item.CreateTime,
                        Publisher = item.Publisher,
                        LastUpdateTime = item.LastUpdateTime,
                        CategoryId = item.CategoryId,
                        AuthorId = item.AuthorId,
                        Year = item.Year,
                        Author = new AuthorDTO()
                        {
                            Id = item.Author.Id,
                            Title = item.Author.Title,
                            Description = item.Author.Description,
                            CoverPhoto = item.Author.CoverPhoto
                        },
                        Category = new CategoryDTO()
                        {
                            Id = item.Category.Id,
                            Title = item.Category.Title,
                            Description = item.Category.Description
                        }
                    }).OrderBy(x => x.Title).ToList();
        }

        [HttpGet]
        public BookDTO Get(int id)
        {
            var entity = _unitOfWork.BookRepository.GetByID(id);
            if (entity != null)
            {
                return new BookDTO()
                {
                    Id = entity.Id,
                    Title = entity.Title,
                    Description = entity.Description,
                    CoverPhoto = entity.CoverPhoto,
                    CreateTime = entity.CreateTime,
                    Publisher = entity.Publisher,
                    LastUpdateTime = entity.LastUpdateTime,
                    CategoryId = entity.CategoryId,
                    AuthorId = entity.AuthorId,
                    Year = entity.Year
                };
            }
            return null;
        }

        [HttpPost]
        public ResponseDTO Add(BookDTO dto)
        {
            try
            {
                _unitOfWork.BookRepository.Add(new Book()
                {
                    Title = dto.Title,
                    Description = dto.Description,
                    CoverPhoto = dto.CoverPhoto,
                    Publisher = dto.Publisher,
                    Year = dto.Year,
                    AuthorId = dto.AuthorId,
                    CategoryId = dto.CategoryId,
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
        public ResponseDTO Update(BookDTO dto)
        {
            try
            {
                Book entity = _unitOfWork.BookRepository.GetByID(dto.Id);
                if (entity != null)
                {
                    entity.Title = dto.Title;
                    entity.Description = dto.Description;
                    entity.CoverPhoto = dto.CoverPhoto;
                    entity.Publisher = dto.Publisher;
                    entity.Year = dto.Year;
                    entity.AuthorId = dto.AuthorId;
                    entity.CategoryId = dto.CategoryId;
                    entity.LastUpdateTime = DateTime.Now;

                    _unitOfWork.BookRepository.Update(entity);
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
                Book entity = _unitOfWork.BookRepository.GetByID(id);
                if (entity != null)
                {
                    _unitOfWork.BookRepository.Delete(entity);
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
