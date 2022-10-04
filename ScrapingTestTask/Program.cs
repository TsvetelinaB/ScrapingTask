using System.Web;
using HtmlAgilityPack;
using Newtonsoft.Json;

using ScrapingTestTask;

using static ScrapingTestTask.HtmlFileConstants;

var htmlDoc = GetHtmlDocument();

var productTitlesDecoded = htmlDoc
    .DocumentNode
    .SelectNodes(titlesXPath)
    .Select(x => HttpUtility.HtmlDecode(x.GetAttributeValue(titleAttribute, "")))
    .ToList();

var productPricesCurrRemoved = htmlDoc
    .DocumentNode
    .SelectNodes(pricesXPath)
    .Select(x => (x.InnerHtml).Remove(0,1).Replace(",",""))
    .ToList();
   
var productRatingsInitial = htmlDoc
    .DocumentNode
    .SelectNodes(ratingsXPath)
    .Select(x => x.GetAttributeValue(ratingAttribute, ""))
    .ToList();

var productRatingsNormalized = new List<decimal>();
PopulateListNormalizedRatings(productRatingsInitial, productRatingsNormalized);

var extractedProducts = new List<Product>();

for (int i = 0; i < productTitlesDecoded.Count; i++)
{
    var currentProduct = new Product
    {
        productName = productTitlesDecoded[i],
        price = productPricesCurrRemoved[i],
        rating = productRatingsNormalized[i].ToString().TrimEnd('0')
    };

    extractedProducts.Add(currentProduct);
}

var productsInJson = JsonConvert.SerializeObject(extractedProducts, Formatting.Indented);

Console.WriteLine(productsInJson);

static HtmlDocument GetHtmlDocument()
{
    var currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
    var path = Path.Combine(currentDirectory, htmlFilePath);
    var filePath = Path.GetFullPath(path);

    var htmlDoc = new HtmlDocument();
    htmlDoc.Load(filePath);
    return htmlDoc;
}

static void PopulateListNormalizedRatings(List<string> productRatingsInitial, List<decimal> productRatingsNormalized)
{
    foreach (var rating in productRatingsInitial)
    {
        if (decimal.TryParse(rating, out decimal ratingDecimal))
        {
            if (ratingDecimal > 5.0m)
            {
                ratingDecimal = ratingDecimal/10m*5m;
            }
        };

        productRatingsNormalized.Add(ratingDecimal);
    }
}