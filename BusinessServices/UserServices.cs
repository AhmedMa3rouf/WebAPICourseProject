using DataModel.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessServices
{
    public class UserServices : IUserService
    {
        readonly UnitOfWork _unitOfWork;
        public UserServices(UnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Method to authonticate the user by username & password
        /// </summary>
        /// <param name="userName">User Name</param>
        /// <param name="password">Passord</param>
        /// <returns>user id if authonticate, else 0</returns>
        public int Authenticate(string userName, string password)
        {
            var user = _unitOfWork.UserRepository.
                Get(u => u.UserName == userName && u.Password == password);
            return (user != null && user.UserId > 0) ? user.UserId : 0;
        }
    }
}
