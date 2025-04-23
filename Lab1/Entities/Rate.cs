using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Lab1.Entities;

public class Rate
{
    [Key]
    [JsonPropertyName("Cur_Id")]
    public int CurId { get; set; }

    public DateTime Date { get; set; }

    [JsonPropertyName("Cur_Abbreviation")]
    public string CurAbbreviation { get; set; }

    [JsonPropertyName("Cur_Scale")]
    public int CurScale { get; set; }

    [JsonPropertyName("Cur_Name")]
    public string CurName { get; set; }

    [JsonPropertyName("Cur_OfficialRate")]
    public decimal? CurOfficialRate { get; set; }
}