namespace ScrapingTestTask
{
    public static class HtmlFileConstants
    {
        public static string htmlFilePath = @"..\..\..\HtmlExcerpt.html";

        public static string titlesXPath = @"//div[@class='item']/figure/a/img";
        public static string pricesXPath = @"//div[@class='pricing']/p/span/span[@style='display: none']";
        public static string ratingsXPath = @"//div[@class='item']";

        public static string titleAttribute = "alt";
        public static string ratingAttribute = "rating";
    }
}
