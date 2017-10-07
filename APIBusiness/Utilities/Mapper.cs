using APIBusiness.DataTransferObject;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIBusiness.Utilities
{
    public static class Mapper
    {
        public static UserDTO ToDTO(User entity)
        {
            return entity == null ? null : new UserDTO()
            {
                Id = entity.Id,
                Password = entity.Password,
                Email = entity.Email,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                RoleType = entity.Role.RoleType
            };
        }
        public static BookDTO ToDTO(Book enity)
        {
            return enity == null ? null : new BookDTO()
            {
                Id = enity.Id,
                Title = enity.Title,
                Description = enity.Description,
                CoverPhoto = enity.CoverPhoto,
                CreateTime = enity.CreateTime,
                Publisher = enity.Publisher,
                LastUpdateTime = enity.LastUpdateTime,
                CategoryId = enity.CategoryId,
                AuthorId = enity.AuthorId,
                Year = enity.Year,
                Author = Mapper.ToDTO(enity.Author),
                Category = Mapper.ToDTO(enity.Category)
            };
        }
        public static AuthorDTO ToDTO(Author entity)
        {
            return entity == null ? null : new AuthorDTO()
            {
                Id = entity.Id,
                Title = entity.Title,
                Description = entity.Description,
                CoverPhoto = entity.CoverPhoto,
                CreateTime = entity.CreateTime,
                LastUpdateTime = entity.LastUpdateTime
            };
        }
        public static CategoryDTO ToDTO(Category entity)
        {
            return entity == null ? null : new CategoryDTO()
            {
                Id = entity.Id,
                Title = entity.Title,
                Description = entity.Description,
                CreateTime = entity.CreateTime,
                LastUpdateTime = entity.LastUpdateTime
            };
        }

        public static void ToEntity(Book entity, BookDTO dto)
        {
            if (entity == null) entity = new Book();
            entity.Title = dto.Title;
            entity.Description = dto.Description;
            entity.CoverPhoto = dto.CoverPhoto;
            entity.Publisher = dto.Publisher;
            entity.Year = dto.Year;
            entity.AuthorId = dto.AuthorId;
            entity.CategoryId = dto.CategoryId;
            entity.LastUpdateTime = DateTime.Now;
        }
        public static void ToEntity(Author entity, AuthorDTO dto)
        {
            entity.Title = dto.Title;
            entity.Description = dto.Description;
            entity.CoverPhoto = dto.CoverPhoto;
            entity.LastUpdateTime = DateTime.Now;
        }
        public static void ToEntity(Category entity, CategoryDTO dto)
        {
            entity.Title = dto.Title;
            entity.Description = dto.Description;
            entity.LastUpdateTime = DateTime.Now;
        }
    }
}
