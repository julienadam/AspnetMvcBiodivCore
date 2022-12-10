namespace AspNetBiodiv.Core.Web.Services.Especes;

public interface ITaxonomie
{
    Espece? RechercherParId(int id);
    Espece? RechercherParNomScientifique(string nomScientifique);
    IEnumerable<Espece> RechercherParTag(string tag);
    IEnumerable<Espece> RechercherParMois(int year, int month);
}