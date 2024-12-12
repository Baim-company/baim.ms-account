using Global.Infrastructure.Exceptions.PersonalAccount;
using HtmlAgilityPack;
using PersonalAccount.API.Models.Dtos.Clients;
using PersonalAccount.API.Services.Abstractions;

namespace PersonalAccount.API.Services.Implementations;

public class VoenService : IVoenService
{
    private readonly HttpClient _httpClient;

    public VoenService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }



    public async Task<VoenModel> GetVoenDetailsAsync(string voen)
    {
        try
        { 
            VoenModel company = new VoenModel();
            string url = $"https://e-taxes.gov.az/ebyn/commersialChecker.jsp?name={voen}&tip=2&sub_mit=1";

            var response = await _httpClient.GetStringAsync(url);

            var doc = new HtmlDocument();
            doc.LoadHtml(response);

            HtmlNode table = doc.DocumentNode.SelectSingleNode("//table[@class='com']");
            if (table == null) throw new PersonalAccountException(PersonalAccountErrorType.VoenNotFound,"Table not found");

            foreach (HtmlNode row in table.SelectNodes(".//tr[position() > 1]"))
            {
                HtmlNodeCollection cells = row.SelectNodes(".//td");
                if (cells != null && cells.Count >= 10)
                {
                    company.CompanyName = cells[0].InnerText.Trim();
                    int startIndex = company.CompanyName.IndexOf('"');
                    if (startIndex != -1)
                    {
                        startIndex++;
                        int endIndex = company.CompanyName.IndexOf('"', startIndex);
                        if (endIndex != -1) company.CompanyName = company.CompanyName.Substring(startIndex, endIndex - startIndex).Trim();
                    }
                    company.Voen = cells[1].InnerText.Trim();
                    company.LegalForm = cells[3].InnerText.Trim();
                    company.LegalAddress = cells[4].InnerText.Trim();
                    company.LegalRepresentative = cells[7].InnerText.Trim();
                }
            }

            return company;
        }
        catch (Exception)
        {
            throw;
        }
    }
}