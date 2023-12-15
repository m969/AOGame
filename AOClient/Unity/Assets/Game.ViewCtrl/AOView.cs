using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AO
{
    public static class AOView
    {
        public static void UITest()
        {
            UIFunctions.Open<LoginWindow>();
            UIFunctions.Open<LoginWindow>(beforeOpen:(x) => { 
                x.Account = "123";
                x.Password = "456";
            });
        }
    }
}
