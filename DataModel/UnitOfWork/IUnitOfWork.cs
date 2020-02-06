using DataModel.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.UnitOfWork
{
    public interface IUnitOfWork
    {
        #region Properties
        GenericRepository<Product> ProductRepository { get; }
        GenericRepository<User> UserRepository { get; }
        GenericRepository<Token> TokenRepository { get;}
        #endregion

        #region Public Methods
        /// <summary>
        /// Save Method
        /// </summary>
        void Save();
        #endregion
    }
}
