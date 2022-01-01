using System;
namespace SeeTrue.Tests
{
    public class DecodedJWT
    {
        public Guid sub { get; set; }
        public string aud { get; set; }
        public string iss { get; set; }
        public int exp { get; set; }
        public Guid lid { get; set; }
        public string role { get; set; }
        public Guid gid { get; set; }
    }
}
