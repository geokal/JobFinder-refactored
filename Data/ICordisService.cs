using HtmlAgilityPack;
using System.Linq;
using static QuizManager.Pages.QuizViewer4;

public interface ICordisService
{
    Task<Project?> GetProjectByIdAsync(string projectId);
}

public class CordisService : ICordisService
{
    private readonly HttpClient _httpClient;

    public CordisService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Project?> GetProjectByIdAsync(string projectId)
    {
        try
        {
            var response = await _httpClient.GetStringAsync($"https://cordis.europa.eu/project/id/{projectId}");
            var doc = new HtmlDocument();
            doc.LoadHtml(response);

            // Extract Topics information
            var topicsNode = doc.DocumentNode.SelectSingleNode("//h3[@id='topicslist']/following-sibling::ul[contains(@class, 'c-factsheet__list')]");
            var topics = topicsNode?.SelectNodes(".//li/a")?
                .Select(a => a.InnerText.Trim())
                .Where(text => !string.IsNullOrWhiteSpace(text))
                .ToList() ?? new List<string>();

            // Extract Programme(s) information
            var programmeNode = doc.DocumentNode.SelectSingleNode("//h3[@id='fundedunderprogrammes']/following-sibling::ul[1]");
            var programmes = programmeNode?.SelectNodes(".//li")?
                .Select(li => li.InnerText.Trim())
                .Where(text => !string.IsNullOrWhiteSpace(text))
                .ToList() ?? new List<string>();

            // Extract Coordinator (clean text before <br>)
            var coordinatorNode = doc.DocumentNode.SelectSingleNode("//p[@class='coordinated coordinated-name']");
            string coordinator = coordinatorNode?.ChildNodes
                .FirstOrDefault(n => n.NodeType == HtmlNodeType.Text)?.InnerText.Trim() ?? "N/A";

            // Extract Start & End Dates
            var startDate = doc.DocumentNode.SelectSingleNode(
                "//span[contains(@class, 'c-project-info__label') and contains(text(), 'Start date')]/following-sibling::text()[1]"
            )?.InnerText.Trim() ?? "N/A";

            var endDate = doc.DocumentNode.SelectSingleNode(
                "//span[contains(@class, 'c-project-info__label') and contains(text(), 'End date')]/following-sibling::text()[1]"
            )?.InnerText.Trim() ?? "N/A";

            // Extract Keywords
            var keywordNodes = doc.DocumentNode.SelectNodes("//ul[@class='c-factsheet__pills']/li/a[@class='c-factsheet__pillitem']");
            var keywords = keywordNodes?
                .Select(node => node.InnerText.Trim())
                .Where(text => !string.IsNullOrWhiteSpace(text))
                .ToList() ?? new List<string>();

            // Extract Description from c-article__text (new implementation)
            var descriptionNode = doc.DocumentNode.SelectSingleNode("//p[@class='c-article__text']");
            string description = "N/A";

            if (descriptionNode != null)
            {
                // Remove <br> tags and normalize whitespace
                description = descriptionNode.InnerHtml
                    .Replace("<br>", " ")
                    .Replace("<br/>", " ")
                    .Replace("\n", " ")
                    .Replace("  ", " ")
                    .Trim();
            }

            return new Project
            {
                Title = doc.DocumentNode.SelectSingleNode("//h1[@class='c-header-project__title']")?.InnerText.Trim(),
                Acronym = doc.DocumentNode.SelectSingleNode("//div[@class='c-project-info__acronym']")?.InnerText.Trim(),
                GrantId = doc.DocumentNode.SelectSingleNode("//div[@class='c-project-info__id']")?.InnerText.Trim(),
                Duration = $"{startDate} - {endDate}",
                StartDate = startDate,
                EndDate = endDate,
                TotalCost = doc.DocumentNode.SelectSingleNode("//div[@class='c-project-info__overall']")?.InnerText.Trim() ?? "No data",
                EuContribution = doc.DocumentNode.SelectSingleNode("//div[@class='c-project-info__eu']")?.InnerText.Trim(),
                Coordinator = coordinator,
                Keywords = keywords,
                Description = description,
                Programmes = programmes,
                Topics = topics

            };
        }
        catch
        {
            return null;
        }
    }

    // Helper method to extract dates
    private string ExtractDate(HtmlDocument doc, string dateType)
    {
        var node = doc.DocumentNode.SelectSingleNode(
            $"//span[contains(@class, 'c-project-info__label') and contains(text(), '{dateType}')]/following-sibling::text()[1]"
        );
        return node?.InnerText.Trim() ?? "N/A";
    }
}