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

  private void Start()
  {
    var units = new List<string>();

    units.Add(@"
    import * as PIXI from 'pixi.js';
    import UIButton from './UIButton';
    import UISprite from './UISprite';
    import UILabel from './UILabel';");

    units.Add("const images = [");

    string[] pathes = Directory.GetFiles(SourceFolder, "*.png", SearchOption.AllDirectories);
    foreach (var path in pathes)
    {
      var file = path.Substring(SourceFolder.Length);
      File.Copy(path, ExportFolder + ExportImagesFolder + file, true);

      units.Add($"\"{ExportImagesFolder}{file}\",");
    }

    units.Add("];");

    units.Add(@"
    const app = new PIXI.Application({ width: 760, height: 540, transparent: false });
    app.renderer.backgroundColor = 0xCCCCCC;
    document.body.appendChild(app.view);

    PIXI
      .loader
      .add(Object.values(images))
      .load(setup);");

    units.Add("function setup() {");
    units = Test(gameObject, "app.stage", units);
    units.Add("};");

    var app = string.Join("\n", units);

    StreamWriter writer = new StreamWriter(ExportFolder + ExportAssetsFolder + "app.ts", false);
    writer.WriteLine(app);
    writer.Close();
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
  
  private List<string> Test(GameObject go, string parent, List<string> units)
  {
    var position = GetCoords(go);

    var popup = go.GetComponent<PopupController>();
    if (popup != null)
    {
      var template = @"
      class Popup extends UISprite {
    		public Button?: UIButton;

    		addButton(button: UIButton) {
    			this.Button = button;
    			this.addChild(button);
  			}
    	};";

      units.Add(template);
      units.Add($"let {popup.name} = new Popup(app.stage);");
      units.Add($"{popup.name}.move({position.x}, {position.y});");
    }

    var image = go.GetComponent<Image>();
    if (image != null)
    {
      var prototype = "UISprite";
      var addButton = "";
      if (go.GetComponent<Button>() != null) {
        prototype = "UIButton";
        addButton = $"{parent}.addButton({go.name})";
      }

      var template = "let {0} = new {1}({2}, \"{3}{4}.png\", {5}, {6});";
      var values = new object[] {
        go.name,
        prototype,
        parent,
        ExportImagesFolder,
        image.sprite.name,
        position.x,
        position.y
      };
      units.Add(string.Format(template, values));

      if (addButton.Length > 0) {
        units.Add(addButton);
      }
    }

    var text = go.GetComponent<TextMeshProUGUI>();
    if (text != null)
    {
      var template = "let {0} = new UILabel({1}, \"{2}\", {3}, {4}, {5}, {6});";
      var values = new object[] {
        go.name,
        parent,
        text.text,
        text.fontSize,
        "0x" + ColorUtility.ToHtmlStringRGB(text.color),
        position.x + text.rectTransform.rect.width / 2,
        position.y + text.rectTransform.rect.height / 2
      };

      units.Add(string.Format(template, values));
      units.Add($"{parent}.addLabel({go.name});");
    }

    units.Add("");

    for (var childIndex = 0; childIndex < go.transform.childCount; childIndex++)
    {
      Test(go.transform.GetChild(childIndex).gameObject, go.name, units);
    }

    return units;
  }
}
