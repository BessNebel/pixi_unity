using System.IO;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class Exporter
{
  public static Dictionary<string, string> Export(GameObject objectToExport)
  {
    var node = GatherNodes(objectToExport);

    var export = ExportNodes(node, new Dictionary<string, string>());

    StreamWriter exportWriter = new StreamWriter(ExportController.ExportFolder + ExportController.ExportAssetsFolder + objectToExport.name + ".ts", false);
    exportWriter.WriteLine(string.Join("\n", ExportController.imports)  + "\nimport * as SC from './SupportClasses';\n" + export[objectToExport.name]);
    exportWriter.Close();

    export.Remove(objectToExport.name);

    return export;
  }

  private static Dictionary<string, string> ExportNodes(ExporterNode node, Dictionary<string, string> Classes)
  {
    if (node.Children.Count == 0 || Classes.ContainsKey(node.ClassName))
    {
      return Classes;
    }

    var nodeClass = $"\n{node.GetDefinition()} extends {node.Type} {{";

    var additionalParameters = new List<string>();
    var constructorBody = new List<string>();

    foreach (var child in node.Children)
    {
      nodeClass += $"\n\tpublic {child.Name}: ";
      var childName = child.Name;

      if (node.Parent == "Canvas")
      {
        if (child.Children.Count == 0)
        {
          nodeClass += $"{child.ClassName} = new {child.ClassPrefix}{child.ClassName}({child.InitParameters})";
        }
        else
        {
          nodeClass += $"{child.ClassPrefix}{child.ClassName}";
        }

        constructorBody.Add(GenerateChildInit(child));
        childName += child.Id;
      }
      else
      {
        nodeClass += $"{child.ClassName}";
        additionalParameters.Add($"{child.Name}: {child.ClassName}");
      }

      nodeClass += ';';
      constructorBody.Add($"\n\t\tthis.{child.Name} = {childName};");
      constructorBody.Add($"\t\tthis.addChild(this.{child.Name});");
    }

    var template = ExporterNode.ConstructorTemplates[node.Type.ToString()];
    template = template.Replace("<<ADDITIONAL_PARAMETERS>>", $", {string.Join(", ", additionalParameters)}");
    nodeClass += template.Replace("<CONSTRUCTOR_BODY>", string.Join("\n", constructorBody));
    nodeClass += "\n}";

    Classes.Add(node.ClassName, nodeClass);

    foreach (var child in node.Children)
    {
      ExportNodes(child, Classes);
    }

    return Classes;
  }

  private static string GenerateChildInit(ExporterNode node)
  {
    var childInit = "";
    var childsOfChild = new List<string>();

    foreach (var child in node.Children)
    {
      childsOfChild.Add($"{child.Name}{child.Id}");
      childInit += GenerateChildInit(child);
    }

    childInit += $"\n\t\tconst {node.Name}{node.Id} = new {node.ClassPrefix}{node.ClassName}({node.InitParameters}, {string.Join(", ", childsOfChild)});";

    return childInit;
  }

  private static ExporterNode GatherNodes(GameObject go, ExporterNode parent = null)
  {
    var node = new ExporterNode();
    var position = GetCoords(go);

    node.Id = ExportController.Instance.GetNodeId();
    node.Name = go.name;
    node.Parent = go.transform.parent.name;
    node.x = position.x;
    node.y = position.y;    

    var popup = go.GetComponent<PopupController>();
    if (popup != null)
    {
      node.Type = ExporterNode.NodeType.UIPopup;
      node.InitParameters = $"\"\", {node.x}, {node.y}";
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
      node.Children.Add(GatherNodes(go.transform.GetChild(childIndex).gameObject, node));
    }

    node.GenerateClassName();

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
