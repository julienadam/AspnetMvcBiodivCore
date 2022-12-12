using AspNetBiodiv.Core.Web.Entities;
using Microsoft.EntityFrameworkCore;

namespace AspNetBiodiv.Core.Web.Services.Especes;

public interface ITaxonomie
{
    Espece? RechercherParId(int id);
    Espece? RechercherParNomScientifique(string nomScientifique);
    IEnumerable<Espece> RechercherParTag(string tag);
    IEnumerable<Espece> RechercherParMois(int year, int month);
    IEnumerable<string> RechercheDeTags(string query);
}

public class DbTaxonomie : ITaxonomie
{
    private readonly EspecesContext context;

    public DbTaxonomie(EspecesContext context)
    {
        this.context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public Espece? RechercherParId(int id) => 
        context.Especes
            .SingleOrDefault(e => e.EspeceId == id);

    public Espece? RechercherParNomScientifique(string nomScientifique) => 
        context.Especes
            .SingleOrDefault(e => e.NomScientifique == nomScientifique);
    public IEnumerable<Espece> RechercherParTag(string tag)
    {
        return context.Especes
            .Include(e => e.Tags)
            .Where(e =>
                e.Tags
                    .Any(t => t.Nom.ToLower()  == tag.ToLower()));

    }   

    public IEnumerable<Espece> RechercherParMois(int year, int month)
    {
        yield break;
    }

    public IEnumerable<string> RechercheDeTags(string query)
    {
        return context.Tags
            .Where(t => t.Nom.Contains(query.ToLower()))
            .Select(t => t.Nom)
            .ToList();
    }
}