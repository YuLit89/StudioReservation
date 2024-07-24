using Microsoft.VisualStudio.TestTools.UnitTesting;
using StudioMember.Service.Proxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemberService_Spec
{
    [TestClass]
    public class Class1
    {

        [TestMethod]
        public void member_find()
        {

            var service = new MemberServiceProxy(1000);

            var member = service.Find("1125df20-cfb4-443c-8b8d-080ae35e802d");

        }
    }
}
