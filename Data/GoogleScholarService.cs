using HtmlAgilityPack;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace QuizManager.Data
{
    public class GoogleScholarService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<GoogleScholarService> _logger;

        public GoogleScholarService(HttpClient httpClient, ILogger<GoogleScholarService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;

            // Configure HttpClient to mimic a browser
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");
            _httpClient.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
        }

        public async Task<List<ScholarPublication>> GetPublications(string profileUrl)
        {
            var publications = new List<ScholarPublication>();

            try
            {
                // Fetch the HTML content
                var html = await _httpClient.GetStringAsync(profileUrl);

                // Load HTML document
                var doc = new HtmlDocument();
                doc.LoadHtml(html);

                // Select all publication rows
                var rows = doc.DocumentNode.SelectNodes("//tr[@class='gsc_a_tr']");

                if (rows == null || !rows.Any())
                {
                    _logger.LogWarning("No publications found on Google Scholar profile");
                    return publications;
                }

                foreach (var row in rows)
                {
                    var publication = new ScholarPublication();

                    // Extract title and URL
                    var titleNode = row.SelectSingleNode(".//a[@class='gsc_a_at']");
                    if (titleNode != null)
                    {
                        publication.Title = System.Net.WebUtility.HtmlDecode(titleNode.InnerText.Trim());
                        var href = titleNode.GetAttributeValue("href", "");
                        if (!string.IsNullOrEmpty(href))
                        {
                            publication.Url = $"https://scholar.google.com{href}";
                        }
                    }

                    // Extract authors and journal
                    var authorNodes = row.SelectNodes(".//div[@class='gs_gray']");
                    if (authorNodes != null)
                    {
                        if (authorNodes.Count > 0)
                        {
                            publication.Authors = System.Net.WebUtility.HtmlDecode(authorNodes[0].InnerText.Trim());
                        }
                        if (authorNodes.Count > 1)
                        {
                            publication.Journal = System.Net.WebUtility.HtmlDecode(authorNodes[1].InnerText.Trim());
                        }
                    }

                    // Extract citation count
                    var citedByNode = row.SelectSingleNode(".//a[@class='gsc_a_ac gs_ibl']");
                    if (citedByNode != null)
                    {
                        publication.CitedBy = citedByNode.InnerText.Trim();
                    }

                    // Extract year
                    var yearNode = row.SelectSingleNode(".//span[@class='gsc_a_h gsc_a_hc gs_ibl']");
                    if (yearNode != null)
                    {
                        publication.Year = yearNode.InnerText.Trim();
                    }

                    publications.Add(publication);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching publications from Google Scholar");
                throw; // Re-throw or handle as needed
            }

            return publications;
        }
    }

    public class ScholarPublication
    {
        public string Title { get; set; }
        public string Authors { get; set; }
        public string Journal { get; set; }
        public string CitedBy { get; set; }
        public string Year { get; set; }
        public string Url { get; set; }
    }
}