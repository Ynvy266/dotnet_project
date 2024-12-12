namespace dotnet_project.Models
{
	public class StatisticModel
	{
		public int Id { get; set; }
		public int Quantity { get; set; }
		public int Sold { get; set; }
		public int Revenue { get; set; }
		public int Profit { get; set; }
		public DateTime DateCreated { get; set; }
	}
}
