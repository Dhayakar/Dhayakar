using System;
using System.Collections.Generic;
using System.Text;


namespace WYNK.Data.Model.ViewModel
{
    public class UsersListView
    {

        public Users Users { get; set; }
        public User_Role User_Role { get; set; }
        public ICollection<UDetails> UDetails { get; set; }
    }

    public class UDetails
    {
        public int usid { get; set; }
        public string username { get; set; }
        public string role { get; set; }
        public string status { get; set; }
    }


}
