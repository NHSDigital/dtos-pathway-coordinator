namespace PathwayCoordinator.PathwayManager.Utils;

using System.Text.Json;

public class TemplateProcessor
{
  public string PopulateTemplate(string templateJson, string inboundDataJson)
  {
    using var jsonDoc = JsonDocument.Parse(inboundDataJson);
    var inboundData = jsonDoc.RootElement;

    foreach (var property in inboundData.EnumerateObject())
    {
      var placeholder = $"{{{{{property.Name}}}}}";
      if (templateJson.Contains(placeholder))
      {
        templateJson = templateJson.Replace(placeholder, property.Value.ToString());
      }
    }

    // Check for unresolved placeholders
    if (templateJson.Contains("{{"))
    {
      Console.WriteLine("Warning: Unresolved placeholders remain in the template.");
    }

    return templateJson;
  }
}
