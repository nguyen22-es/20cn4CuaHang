using CuaHangCongNghe.Models;
using CuaHangCongNghe.Repository;
using Shop.Models;


namespace CuaHangCongNghe.Services
{
    public class UserService
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
             return userRepository.TryGetByUserId(id).toUserViewModel();
         }


    }
}