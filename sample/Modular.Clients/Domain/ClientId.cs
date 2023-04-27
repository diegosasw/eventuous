using Eventuous;

namespace Modular.Clients.Domain;

public record ClientId 
    : AggregateId
{
    public ClientId(string id) 
        : base(id)
    {
    }
}