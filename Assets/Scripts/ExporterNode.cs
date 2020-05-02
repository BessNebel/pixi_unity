using System.Collections.Generic;

public class ExporterNode
{
  public static Dictionary<string, string> ConstructorTemplates = new Dictionary<string, string> {
    { "UIPopup", "\n\n\tconstructor(x: number, y: number) {\n\t\tsuper(x, y);\n<CONSTRUCTOR_BODY>\n\t};"},
    { "UISprite", "\n\n\tconstructor(textureName: string, x: number, y: number<<ADDITIONAL_PARAMETERS>>) {\n\t\tsuper(textureName, x, y);\n<CONSTRUCTOR_BODY>\n\t};"},
    { "UILabel", "\n\n\tconstructor(text: string, size: number, color: number, x: number, y: number<<ADDITIONAL_PARAMETERS>>) {\n\t\tsuper(text, size, color, x, y);\n<CONSTRUCTOR_BODY>\n\t};"},
    { "UIButton", "\n\n\tconstructor(background: string, x: number, y: number<<ADDITIONAL_PARAMETERS>>) {\n\t\tsuper(background, x, y);\n<CONSTRUCTOR_BODY>\n\t};"}
  };

  public enum NodeType {
    UIPopup,
    UISprite,
    UILabel,
    UIButton,
    Unknown
  };

  public string Name = "Unnamed";
  public string ClassName = "Unnamed";
  public float x = 0;
  public float y = 0;
  public string Parent = "";
  public NodeType Type = NodeType.Unknown;
  public string InitParameters = "";
  public List<ExporterNode> Children = new List<ExporterNode>();

  public string GetDefinition()
  {
    if (Type == NodeType.UIPopup)
    {
      return $"export default class {ClassName}";
    }

    return "class " + ClassName;
  }

  public void GenerateClassName()
  {
    if (Type == NodeType.UIPopup)
    {
      ClassName = Name;
    }
    else
    {
      ClassName = Type.ToString();

      var parts = new List<string>();

      foreach (var child in Children)
      {
        parts.Add(child.Name);
        parts.Add(child.Type.ToString());
      }

      ClassName += string.Join("", parts);
    }
  }
}
