using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dotnet_project.Models
{
    public class MomoInfoModel
    {
        [Key]
        public int Id { get; set; }
        public string OrderId { get; set; }
        public string OrderInfo { get; set; }
        public string FullName { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaidDate { get; set; }
    }
}
