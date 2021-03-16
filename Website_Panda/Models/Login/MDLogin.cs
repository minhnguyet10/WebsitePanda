using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Website_Panda.Models.DAL;

namespace Website_Panda.Models.Login
{
    public class MDLogin
    {
        DB_Optical db = null;
        public MDLogin()
        {
            db = new DB_Optical();
        }

        //public object CommonConstants { get; private set; }

        public Administrator GetById(string userName)
        {
            return db.Administrators.SingleOrDefault(x => x.UserName == userName);
        }
        
        public int Login(string userName, string passWord, bool isLoginAdmin = false)
        {
            var result = db.Administrators.SingleOrDefault(x => x.UserName == userName);
            if (result == null)
            {
                return 0;
            }
            else
            {
                if (isLoginAdmin == true)
                {
                    if (result.GroupID == Common.ADMIN_GROUP)
                    {
                        if (result.Status == false)
                        {
                            return -1;
                        }
                        else
                        {
                            if (result.Password == passWord)
                                return 1;
                            else
                                return -2;
                        }
                    }
                    else if (result.GroupID == Common.MEMBER_GROUP)
                    {
                        if (result.Status == false)
                        {
                            return -1;
                        }
                        else
                        {
                            if (result.Password == passWord)
                                return 4;
                            else
                                return -2;
                        }
                    }
                    else if (result.GroupID == Common.SHIPPER)
                    {
                        if (result.Status == false)
                        {
                            return -1;
                        }
                        else
                        {
                            if (result.Password == passWord)
                                return 5;
                            else
                                return -2;
                        }
                    }
                    else
                    {
                        return -3;
                    }
                }
                else
                {
                    if (result.Password == passWord)
                        return 1;
                    else
                        return -2;
                }
            }
        }
    }
}