namespace ActiveLog.Web.Models;

public class Ziel
{
    public int Id { get; set; }
    public string Beschreibung { get; set; } = string.Empty;
    public DateTime ZielDatum { get; set; }
    public bool Erreicht { get; set; }
}
