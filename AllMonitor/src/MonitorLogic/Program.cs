using HtmlAgilityPack;
using Telegram.Bot;

namespace MonitorLogic;

internal class Program
{
	static async Task Main(string[] args)
	{
		var client = new HttpClient();
		var result = await client.GetAsync("https://online.baby-travel-club.ru/search_tour");
		var resultText = await result.Content.ReadAsStringAsync();
		var html = new HtmlDocument();
		html.LoadHtml(resultText);
		var node = html.DocumentNode.SelectSingleNode("//*[@id=\"header\"]/div[1]/div[2]/table/tbody/tr");
		var rates = node.ChildNodes
			.Where(n => n.HasClass("rate"))
			.Select(n => Convert.ToDouble(n.InnerText.Trim('\n', ' '))).ToArray();


		var euro = rates[0];
		var rub = rates[1];
		var dollar = rates[2];

		TelegramBotClient bot = new (Environment.GetEnvironmentVariable("Token") ?? throw new Exception("Token"));
		var t = await bot.SendMessage(-1002273573999, $"Euro - {euro} Ruble - {rub} Dollar - {dollar}");
	}
}