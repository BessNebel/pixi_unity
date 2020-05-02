using System.IO;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class Exporter : MonoBehaviour
{
  private const string ExportFolder = "Export/";
  private const string ExportImagesFolder = "images/";
  private const string ExportAssetsFolder = "assets/";
  private const string SourceFolder = "Assets/Images/";

  private List<string> Classes = new List<string>();

  private void Start()
  {
    var images = new List<string>();
    images.Add("export const images = [");

    string[] pathes = Directory.GetFiles(SourceFolder, "*.png", SearchOption.AllDirectories);
    foreach (var path in pathes)
    {
      var file = path.Substring(SourceFolder.Length);
      File.Copy(path, ExportFolder + ExportImagesFolder + file, true);

      images.Add($"\t\"{ExportImagesFolder}{file}\",");
    }

    images.Add("];");

    StreamWriter imagesWriter = new StreamWriter(ExportFolder + ExportAssetsFolder + "Images.ts", false);
    imagesWriter.WriteLine(string.Join("\n", images));
    imagesWriter.Close();

    var node = GatherNodes(gameObject);

    var imports = new string[] {
      "import UISprite from './UI/UISprite';",
      "import UIPopup from './UI/UIPopup';",
      "import UILabel from './UI/UILabel';",
      "import UIButton from './UI/UIButton';"
    };

    var export = ExportNodes(node, string.Join("\n", imports));

    StreamWriter exportWriter = new StreamWriter(ExportFolder + ExportAssetsFolder + gameObject.name + ".ts", false);
    exportWriter.WriteLine(export);
    exportWriter.Close();
  }

  private string ExportNodes(ExporterNode node, string export = "")
  {
    if (node.Children.Count == 0 || Classes.IndexOf(node.ClassName) >= 0)
    {
      return export;
    }

    Classes.Add(node.ClassName);

    var exportUnit = $"{node.GetDefinition()} extends {node.Type} {{";

    var additionalParameters = new List<string>();
    var constructorBody = new List<string>();

    foreach (var child in node.Children)
    {
      exportUnit += $"\n\tpublic {child.Name}: {child.ClassName}";

      if (node.Parent == "Canvas")
      {
        if (child.Children.Count == 0)
        {
          exportUnit += $" = new {child.ClassName }({child.InitParameters})";
        }

        constructorBody.Add(GenerateChildInit(child));
      }
      else
      {
        additionalParameters.Add($"{child.Name}: {child.ClassName}");
      }

      exportUnit += ';';
      constructorBody.Add($"\n\t\tthis.{child.Name} = {child.Name};");
      constructorBody.Add($"\t\tthis.addChild(this.{child.Name});");
    }

    var template = ExporterNode.ConstructorTemplates[node.Type.ToString()];
    template = template.Replace("<<ADDITIONAL_PARAMETERS>>", $", {string.Join(", ", additionalParameters)}");
    exportUnit += template.Replace("<CONSTRUCTOR_BODY>", string.Join("\n", constructorBody));
    exportUnit += "\n}";
    export += "\n\n" + exportUnit;

    foreach (var child in node.Children)
    {
      export = ExportNodes(child, export);
    }

    return export;
  }

  private string GenerateChildInit(ExporterNode node)
  {
    var childInit = "";
    var childsOfChild = new List<string>();

    foreach (var child in node.Children)
    {
      childsOfChild.Add(child.Name);
      childInit += GenerateChildInit(child);
    }

    childInit += $"\n\t\tconst {node.Name} = new {node.ClassName}({node.InitParameters}, {string.Join(", ", childsOfChild)});";

    return childInit;
  }

  private ExporterNode GatherNodes(GameObject go, ExporterNode parent = null)
  {
    var node = new ExporterNode();
    var position = GetCoords(go);

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

      node.InitParameters = $"\"{ExportImagesFolder}{image.sprite.name}.png\", {node.x}, {node.y}";
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

  private Vector2 GetCoords(GameObject go)
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
