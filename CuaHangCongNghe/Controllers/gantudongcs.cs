

using CuaHangCongNghe.Models.Tables;

namespace CuaHangCongNghe.Controllers
{
    public partial class gantudongcs 
    {
        private static List<int> n = new List<int>();
        
        public int Gantudong()
        {
            using (var db = new storeContext())
            {
                var user = db.Users.ToList();
                foreach(var user1 in user)
                {
                    n.Add(user1.UserId);
                }
            }
            

            Random rnd = new Random();
            int random = rnd.Next(0, 100);                    
            while (n.Contains(random))
            {
                rnd = new Random();
                random = rnd.Next(0, 9);
                          
            }

            n.Add(random);

            return random;
        }

    }
}
public partial class IndexViewModel
{
    public List<User> Users { get; set; }

  

}