using Entities;
using Entities.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIBusiness.Business
{
    public class BaseBusiness
    {
        private UnitOfWork _unitOfWork;
        protected UnitOfWork UnitOfWork
        {
            get
            {
                if (_unitOfWork == null)
                    _unitOfWork = new UnitOfWork();
                return _unitOfWork;
            }
        }
    }
}
