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
    public class AuthorBusiness : BaseBusiness
    {
        public AuthorSearchResultDTO Search(AuthorSearchCriteriaDTO criteria)
        {
            var query = UnitOfWork.AuthorRepository.GetAll();
            AuthorSearchResultDTO result = new AuthorSearchResultDTO();
            result.Total = query.Count();
            result.Results = query.OrderBy(x => x.Title)
                .Skip(criteria.PageSize * (criteria.PageIndex - 1))
                .Take(criteria.PageSize).ToList()
                .Select(c=>Mapper.ToDTO(c)).ToList();
            return result;
        }
        public List<AuthorDTO> GetAll()
        {
            return UnitOfWork.AuthorRepository.GetAll().ToList().Select(x => Mapper.ToDTO(x)).ToList();
        }
        public AuthorDTO Get(int id)
        {
            return Mapper.ToDTO(UnitOfWork.AuthorRepository.GetByID(id));
        }
        
        public ResponseDTO Add(AuthorDTO dto)
        {
            try
            {
                Author entity = new Author();
                Mapper.ToEntity(entity, dto);
                entity.CreateTime = entity.LastUpdateTime = DateTime.Now;
                UnitOfWork.AuthorRepository.Add(entity);
                UnitOfWork.Commit();
                return new ResponseDTO() { ResponseCode = Enums.ResponseCode.Success };
            }
            catch (Exception ex)
            {
                return new ResponseDTO() { ResponseCode = Enums.ResponseCode.Error, Message = ex.Message };
            }
        }
        
        public ResponseDTO Update(AuthorDTO dto)
        {
            try
            {
                Author entity = UnitOfWork.AuthorRepository.GetByID(dto.Id);
                if (entity != null)
                {
                    Mapper.ToEntity(entity, dto);
                    UnitOfWork.AuthorRepository.Update(entity);
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
                bool isInUse = UnitOfWork.BookRepository.GetAll().Any(x => x.AuthorId == id);

                if (isInUse)
                {
                    return new ResponseDTO() { ResponseCode = Enums.ResponseCode.InUse };
                }
                Author entity = UnitOfWork.AuthorRepository.GetByID(id);
                if (entity != null)
                {
                    UnitOfWork.AuthorRepository.Delete(entity);
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
