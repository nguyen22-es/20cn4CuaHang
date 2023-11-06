using CuaHangCongNghe.Models;
using CuaHangCongNghe.Models.Shop;
using CuaHangCongNghe.Repository;
using CuaHangCongNghe.Service;
using CuaHangCongNghe.Models;


namespace CuaHangCongNghe.Services
{
    public class UserService : IUserService
    {
        private readonly  UserRepository userRepository;

        public UserService(UserRepository userRepository)
        {
            this.userRepository = userRepository;
        }



         public void AddInformation(string userId, UserViewModel userViewModel)
         {
             var orderInfo = userViewModel.ToUserInfo();
             orderInfo.UserId = userRepository.TryGetByUserId(userId).UserId;
             userRepository.AddInformation(orderInfo);
         }


         public List<UserViewModel> GetAll()
         {
             var allUser = userRepository.GetAll();
             var UserViewModels = new List<UserViewModel>();
             if (allUser != null)
             {
                 foreach (var user in allUser)
                 {
                     UserViewModels.Add(user.toUserViewModel());
                 }
             }
             return UserViewModels;
         }

         public UserViewModel GetUser(string id)
         {
           var user =  userRepository.TryGetByUserId(id).toUserViewModel();


            if (user != null)
            {
                return user;
            }


            return null;
        }
        public User GetUserl(string id)
        {
            var user = userRepository.TryGetByUserId(id);
           
            return user;
        }



        public void createUser(string IdUser)
        {
            User user = new User()
            {
                UserId = IdUser,
            };

             userRepository.CreateUser(user);
        }


    }
}