using CuaHangCongNghe.Models.Shop;
using Microsoft.EntityFrameworkCore;


namespace CuaHangCongNghe.Repository
{
    public class UserDbRepository : UserRepository
    {
        private readonly storeContext storeContext;

        public UserDbRepository(storeContext storeContext)
        {
            this.storeContext = storeContext;
        }

        public void AddInformation(User userInfo)
        {
            var user = storeContext.Users.FirstOrDefault(x => x.UserId == userInfo.UserId);
            if (userInfo.AddressUser != null)
                user.AddressUser = userInfo.AddressUser;
            if (userInfo.PhoneUser != null)
                user.PhoneUser = userInfo.PhoneUser;
            if (userInfo.NameUser != null)
                user.NameUser = userInfo.NameUser;
            if (userInfo.EmailUser != null)
                user.EmailUser = userInfo.EmailUser;

            if (userInfo.RegistrationDate != null)
                user.RegistrationDate = userInfo.RegistrationDate;
      
            storeContext.SaveChanges();
        }




        public void CreateUser(User user)
        {
            storeContext.Users.Add(user);
            storeContext.SaveChanges();
        }

        public void DeleteOder(int itemId, Product product, int quantity)
        {
            var item = storeContext.Orderitems.FirstOrDefault(x => x.OrderItemsId == itemId);
            if(item != null)
            {
                storeContext.Orderitems.Remove(item);
            }
            
 
        }

        public List<User> GetAll()
        {
            return storeContext.Users.AsNoTracking().ToList();
        }


        public User TryGetByUserId(string id)
        {
            var users = GetAll();

            foreach(var user in users)
            {
                if(user.UserId == id  )
                {
                    return user;
                }

            }
            return null;

        }
    }
}
