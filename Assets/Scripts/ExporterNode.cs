using System.Collections.Generic;

public class ExporterNode
{
  public enum NodeType {
    UIPopup,
    UISprite,
    UILabel,
    UIButton,
    Unknown
  };

  public int Id = 0;
  public string Name = "Unnamed";
  public float x = 0;
  public float y = 0;
  public NodeType Type = NodeType.Unknown;
  public string InitParameters = "";
  public List<ExporterNode> Children = new List<ExporterNode>();
}
