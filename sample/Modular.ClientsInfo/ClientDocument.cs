using Eventuous.Projections.MongoDB.Tools;

namespace Modular.ClientsInfo;

public record ClientDocument
    : ProjectedDocument
{
    public ClientDocument(string id) 
        : base(id)
    {
    }

    public string Name { get; init; } = string.Empty;
    public string AdminEmail { get; init; } = string.Empty;
    public DateTime CreatedOn { get; init; }
}