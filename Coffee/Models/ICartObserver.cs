using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coffee.Models
{
    public interface ICartObserver
    {
        void Update();
    }
}