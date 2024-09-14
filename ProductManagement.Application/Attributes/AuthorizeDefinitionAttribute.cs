using ProductManagement.Core.Enums;

namespace ProductManagement.Core.Attributes;

public class AuthorizeDefinitionAttribute : Attribute
{
    public string Menu { get; set; }
    public string Definition { get; set; }
    public ActionType ActionType { get; set; }
}