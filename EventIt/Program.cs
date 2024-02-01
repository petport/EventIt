namespace EventIt
{
    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;
    class Program
    {
        static void Main(string[] args)
        {
            var Parser = new Parser();
            var theaters = Parser.Parse("https://www.athinorama.gr/theatre/guide/simera/");
            Parser.PrintAllTheatersInfo(theaters);
        }
    }

    class Parser
    {
        private List<Theater> theatersList = new List<Theater>();

        public List<Theater> Parse(string baseUrl)
        {
            var options = new ChromeOptions();
            options.AddArgument("headless");
            var driver = new ChromeDriver(options);
            driver.Url = baseUrl;

            var theaters = driver.FindElements(By.CssSelector(".item.horizontal-dt.card-item"));

            for (int i=0; i<theaters.Count; i++)
            {
                string title = theaters[i].FindElement(By.CssSelector(".item-title")).Text;
                
                string by_who = "";
                try
                {
                    by_who = theaters[i].FindElement(By.CssSelector(".director")).Text;
                }
                catch (Exception)
                {
                    by_who = "";
                }
                var tags = theaters[i].FindElement(By.CssSelector(".tags")).FindElements(By.TagName("li"));

                string type;
                string duration = "";
                string director = "";

                type = tags[0].Text;
                if (tags.Count == 3)
                {
                    duration = tags[1].Text;
                    director = tags[2].Text;
                }
                else if (tags.Count == 2)
                {
                    if (tags[1].Text.Contains("Διάρκεια"))
                    {
                        duration = tags[1].Text;
                    }
                    else if (tags[1].Text.Contains("Σκηνοθ"))
                    {
                        director = tags[1].Text;
                    }
                }

                string description = theaters[i].FindElement(By.CssSelector(".summary")).Text;
                string venue = theaters[i].FindElements(By.CssSelector(".item-description"))[1].Text;
                string moreInfoUrl = theaters[i].FindElement(By.CssSelector(".item-title ")).FindElement(By.TagName("a")).GetAttribute("href");

                theatersList.Add(new Theater
                {
                    Title = title,
                    By_who = by_who,
                    Type = type,
                    Duration = duration,
                    Director = director,
                    Description = description,
                    Venue = venue,
                    MoreInfoUrl = moreInfoUrl
                });                
            }

            return theatersList;
        }

        public void PrintSingleTheaterInfo(int i, string title, string by_who, string type, string duration, string director, 
                                        string description, string venue, string moreInfoUrl)
        {
                Console.WriteLine("====================================================");
                Console.WriteLine("Theater {0}", i+1);
                Console.WriteLine("Title: {0}", title);
                Console.WriteLine("By: {0}", by_who);
                Console.WriteLine("Type: {0}", type);
                Console.WriteLine("Duration: {0}", duration);
                Console.WriteLine("Director: {0}", director);
                Console.WriteLine("Description: {0}", description);
                Console.WriteLine("Venue: {0}", venue);
                Console.WriteLine("More Info: {0}", moreInfoUrl);
        }

        public void PrintAllTheatersInfo(List<Theater> theaters)
        {
            for (int i=0; i<theaters.Count; i++)
            {
                Console.WriteLine("====================================================");
                Console.WriteLine("Theater {0}", i+1);
                Console.WriteLine("Title: {0}", theaters[i].Title);
                Console.WriteLine("By: {0}", theaters[i].By_who);
                Console.WriteLine("Type: {0}", theaters[i].Type);
                Console.WriteLine("Duration: {0}", theaters[i].Duration);
                Console.WriteLine("Director: {0}", theaters[i].Director);
                Console.WriteLine("Description: {0}", theaters[i].Description);
                Console.WriteLine("Venue: {0}", theaters[i].Venue);
                Console.WriteLine("More Info: {0}", theaters[i].MoreInfoUrl);
            }
        }
    }

    class Theater
    {
        public required string Title { get; set; }
        public required string By_who { get; set; }
        public required string Type { get; set; }
        public required string Duration { get; set; }
        public required string Director { get; set; }
        public required string Description { get; set; }
        public required string Venue { get; set; }
        public required string MoreInfoUrl { get; set; }
    }


}