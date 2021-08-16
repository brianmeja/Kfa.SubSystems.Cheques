namespace Kfa.SubSystems.Cheques.Contracts.Classes
{
    public struct UserObject
    {
        public string UserName { set; get; }
        public int UserId { set; get; }
        public string Token { set; get; }
        public string LoginId { set; get; }
        public string Prefix { set; get; }
        public string OriginatorId { set; get; }
    }
}