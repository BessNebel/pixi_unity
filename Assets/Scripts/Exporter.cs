using System.IO;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class Exporter
{
  public static string[] imports = {
      "import UISprite from './UI/UISprite';",
      "import UIPopup from './UI/UIPopup';",
      "import UILabel from './UI/UILabel';",
      "import UIButton from './UI/UIButton';"
    };

  public static void Export(GameObject objectToExport)
  {
    var node = GatherInfo(objectToExport);
    var export = GenerateClass(node);

    StreamWriter exportWriter = new StreamWriter(ExportController.ExportFolder + ExportController.ExportAssetsFolder + objectToExport.name + ".ts", false);
    exportWriter.WriteLine(string.Join("\n", imports) + "\n\n" + export);
    exportWriter.Close();
  }

  private static string GenerateClass(ExporterNode node, int deepth = 0)
  {
    var lines = new List<string>();

    if (deepth == 0)
    {
      lines.Add($"export default class {node.Name} extends {node.Type} {{");
    }

    var constructorBody = new List<string>();
    var deepthTab = new string('\t', deepth);

    foreach (var child in node.Children)
    {
      if (child.Children.Count == 0)
      {
        lines.Add($"{deepthTab}\tpublic {child.Name} = new {child.Type}({child.InitParameters});");
      }
      else
      {
        lines.Add($"{deepthTab}\tpublic {child.Name} = new class extends {child.Type} {{");
        lines.Add(GenerateClass(child, deepth + 1));
        lines.Add($"{deepthTab}\t}}");
      }

      constructorBody.Add($"{deepthTab}\t\tthis.addChild(this.{child.Name});");
    }

    lines.Add($"{deepthTab}\tconstructor() {{");
    lines.Add($"{deepthTab}\t\tsuper({node.InitParameters});");
    lines.Add(string.Join("\n", constructorBody));
    lines.Add($"{deepthTab}\t}}");

    if (deepth == 0)
    {
      lines.Add("}");
    }

    return string.Join("\n", lines);
  }

  private static ExporterNode GatherInfo(GameObject go, ExporterNode parent = null)
  {
    var node = new ExporterNode();
    var position = GetCoords(go);

    node.Id = ExportController.Instance.GetNodeId();
    node.Name = go.name;
    node.x = position.x;
    node.y = position.y;    

    var popup = go.GetComponent<PopupController>();
    if (popup != null)
    {
      node.Type = ExporterNode.NodeType.UIPopup;
      node.InitParameters = $"{node.x}, {node.y}";
    }

    var image = go.GetComponent<Image>();
    if (image != null)
    {
      if (go.GetComponent<Button>() != null)
      {
        node.Type = ExporterNode.NodeType.UIButton;
      }
      else
      {
        node.Type = ExporterNode.NodeType.UISprite;
      }

      node.InitParameters = $"\"{ExportController.ExportImagesFolder}{image.sprite.name}.png\", {node.x}, {node.y}";
    }

    var text = go.GetComponent<TextMeshProUGUI>();
    if (text != null)
    {
      node.x += text.rectTransform.rect.width / 2;
      node.y += text.rectTransform.rect.height / 2;

      node.Type = ExporterNode.NodeType.UILabel;
      node.InitParameters = $"\"{text.text}\", {text.fontSize}, {"0x" + ColorUtility.ToHtmlStringRGB(text.color)}, {node.x}, {node.y}";
    }

    for (var childIndex = 0; childIndex < go.transform.childCount; childIndex++)
    {
      node.Children.Add(GatherInfo(go.transform.GetChild(childIndex).gameObject, node));
    }

    return node;
  }

  private static Vector2 GetCoords(GameObject go)
  {
    var parentRectTransform = go.transform.parent.GetComponent<RectTransform>();
    var childRectTransform = go.GetComponent<RectTransform>();

    var x = parentRectTransform.rect.width * parentRectTransform.pivot.x;
    x -= childRectTransform.rect.width * childRectTransform.pivot.x;
    x += childRectTransform.localPosition.x;

    var y = parentRectTransform.rect.height * (1 - parentRectTransform.pivot.y);
    y -= childRectTransform.rect.height * (1 - childRectTransform.pivot.y);
    y -= childRectTransform.localPosition.y;

    return new Vector2(x, y);
  }
}
