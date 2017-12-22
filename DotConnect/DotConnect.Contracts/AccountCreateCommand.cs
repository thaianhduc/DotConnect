using System;
using System.Runtime.Serialization;

namespace DotConnect.Contracts
{
    [DataContract(Namespace = Namespaces.DotConnect)]
    public class AccountCreateCommand
    {
        [DataMember(IsRequired = true)]
        public string Name { get; set; }
        [DataMember(IsRequired = true)]
        public string Email { get; set; }
    }

    [DataContract(Namespace = Namespaces.DotConnect)]
    public class AccountCreateCommandResult
    {
        [DataMember(IsRequired = true)]
        public Guid AccountId { get; set; }
    }
}
