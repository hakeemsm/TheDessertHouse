using System.Web.Security;

namespace TheDessertHouse.Domain
{
    public class MembershipCreateResult
    {
        public MembershipUser MembershipUserObject { get; set; }

        public MembershipCreateStatus CreateStatus { get; set; }
    }
}