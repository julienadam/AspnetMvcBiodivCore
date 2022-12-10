namespace AspNetBiodiv.Core.Web.Services.Especes;

public class Espece
{
    public int Id { get; set; }
    public string NomScientifique { get; set; } = string.Empty;
    public int IdInpn { get; set; }
    public EtatPresence Presence { get; set; }
    public Habitat Habitat { get; set; }
}