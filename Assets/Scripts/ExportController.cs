using System.IO;
using System.Collections.Generic;

using UnityEngine;

public class ExportController : MonoBehaviour
{
  public static ExportController Instance = null;

  public const string ExportFolder = "Export/";
  public const string ExportImagesFolder = "images/";
  public const string ExportAssetsFolder = "assets/";
  public const string SourceFolder = "Assets/Images/";

  private int ExporterNodeId = 0;

  public int GetNodeId()
  {
    ExporterNodeId += 1;

    return ExporterNodeId;
  }

  private void Start()
  {
    if (Instance == null) {
      Instance = this;
    }
    else if (Instance == this)
    {
      Destroy(gameObject);
    }

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

    for (var childIndex = 0; childIndex < gameObject.transform.childCount; childIndex++)
    {
      var child = gameObject.transform.GetChild(childIndex).gameObject;
      if (child.activeSelf)
      {
        Exporter.Export(child);
      }
    }
  }
}
