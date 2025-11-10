using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coffee.Models
{
    public interface IOrderState
    {
        void Handle(Order order);
    }

}