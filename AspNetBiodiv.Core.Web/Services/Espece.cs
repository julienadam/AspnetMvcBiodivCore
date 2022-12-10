namespace AspNetBiodiv.Core.Web.Services;

public enum Habitat
{
    Marin = 1,
    EauDouce = 2,
    Terrestre = 3,
    MarinEauDouce = 4,
    MarinTerrestre = 5,
    EauSaumâtre = 6,
    TerrestreEtOuEauDouce = 7,
    TerrestreEtEauDouce = 8,
}

public enum EtatPresence
{
    Présent = (int)'P',
    Endémique = (int)'E',
    SubEndémique = (int)'S',
    Cryptogène = (int)'C',
    Introduit = (int)'I',
    IntroduitEnvahissant = (int)'J',
    IntroduitNonEtabli = (int)'M',
    Occasionnel = (int)'B',
    Douteux = (int)'D',
    Absent = (int)'A',
    Disparu = (int)'W',
    Éteint = (int)'X',
    IntroduitEteint = (int)'Y',
    EndémiqueEteint = (int)'Z',
    MentionnéParErreur = (int)'Q',
}

public class Espece
{
    public int Id { get; set; }
    public string NomScientifique { get; set; } = string.Empty;
    public int IdInpn { get; set; }
    public EtatPresence Presence { get; set; }
    public Habitat Habitat { get; set; }
}