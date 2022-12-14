using AspNetBiodiv.Core.Web.Entities;
using System.Diagnostics;
using System.Formats.Asn1;
using System.Globalization;
using System.Linq;
using CsvHelper;

namespace AspNetBiodiv.Core.Web.Services
{
    public interface IImportService
    {
        void Import();
    }

    public class ImportService : IImportService
    {
        private readonly EspecesContext context;

        public ImportService(EspecesContext context)
        {
            this.context = context;
        }

        public void Import()
        {
            var sw = Stopwatch.StartNew();
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "TAXREFv15.txt");
            var tags = GetAllTags(path);
            Console.WriteLine($"Tag import completed in {sw.Elapsed}");
            sw.Restart();
            var allTags = SaveAllTags(context, tags);
            Console.WriteLine($"Total tags {tags.Count}. Saving them all took {sw.Elapsed}");
            tags.Clear(); // Not used anymore
            sw.Restart();
            SaveSpecies(context, path, allTags);
            Console.WriteLine($"All species extracted & saved. Took {sw.Elapsed}");
            Console.WriteLine("Import complete");
        }

        static IDictionary<string, Tag> SaveAllTags(EspecesContext ctx, HashSet<string> tags)
        {
            var result = new Dictionary<string, Tag>();
            var linkedList = new LinkedList<Tag>();
            var batchSize = 1000;
            foreach (var tagName in tags)
            {
                var tag = new Tag { Nom = tagName };
                linkedList.AddLast(tag);
                result.Add(tagName, tag);
                if (linkedList.Count >= batchSize)
                {
                    SaveBatchOfTags(ctx, linkedList);
                    linkedList = new LinkedList<Tag>();
                }
            }

            SaveBatchOfTags(ctx, linkedList);
            return result;
        }

        static void SaveBatchOfTags(EspecesContext ctx, ICollection<Tag> tagsToSave)
        {
            ctx.Tags.AddRange(tagsToSave);
            ctx.SaveChanges();
            Console.WriteLine($"Saved {tagsToSave.Count} tags.");
        }

        static IEnumerable<CsvReader> EnumerateRelevantLines(string path)
        {
            using var sr = File.OpenText(path);

            var config = new CsvHelper.Configuration.CsvConfiguration(new CultureInfo("fr-FR"))
            {
                Delimiter = "\t"
            };

            using var csvReader = new CsvReader(sr, config);
            csvReader.Read();
            csvReader.ReadHeader();
            int processed = 0;
            while (csvReader.Read())
            {
                if (csvReader["RANG"] != "ES") continue;
                if (csvReader["FR"] == "") continue;
                if (csvReader["REGNE"] != "Animalia") continue;
                if (csvReader["NOM_VERN"] == "") continue;

                processed++;
                if (processed % 1000 == 0)
                {
                    Console.WriteLine($"{processed} lines processed");
                }

                yield return csvReader;
            }
        }

        static IEnumerable<string> GetUnvalidatedTagsFromLine(CsvReader reader)
        {
            yield return reader["PHYLUM"];
            yield return reader["CLASSE"];
            yield return reader["ORDRE"];
            yield return reader["FAMILLE"];
            yield return reader["SOUS_FAMILLE"];
            yield return reader["TRIBU"];
            yield return reader["GROUP1_INPN"];
            yield return reader["GROUP2_INPN"];
            yield return reader["GROUP3_INPN"];
            yield return reader["LB_NOM"];

            foreach (var t in (reader["NOM_VERN"]).Split(new[] { ", " }, StringSplitOptions.RemoveEmptyEntries))
            {
                yield return t;
            }
        }

        static IEnumerable<string> GetValidatedTagsFromLine(dynamic line)
        {
            IEnumerable<string> lines = GetUnvalidatedTagsFromLine(line);
            return lines
                .Select(l => l.ToLower().Trim())
                .Where(l => !string.IsNullOrWhiteSpace(l));
        }


        static HashSet<string> GetAllTags(string path)
        {
            var tags = new HashSet<string>();
            foreach (var line in EnumerateRelevantLines(path))
            {
                foreach (var t in GetValidatedTagsFromLine(line))
                {
                    if (tags.Add(t) && tags.Count % 1000 == 0)
                    {
                        Console.WriteLine($"Current tag count {tags.Count}");
                    }
                }
            }
            return tags;
        }

        static void SaveSpecies(EspecesContext ctx, string path, IDictionary<string, Tag> allTags)
        {
            var batch = new LinkedList<Espece>();
            var existingNames = new HashSet<string>();

            foreach (var line in EnumerateRelevantLines(path))
            {
                var lineTags = GetValidatedTagsFromLine(line);
                var tags = GetTags(allTags, lineTags.ToArray());

                var nomScientifique = line["LB_NOM"];
                var lowerNomScientifique = nomScientifique.ToLowerInvariant();
                // Skip duplicates
                if (existingNames.Contains(lowerNomScientifique))
                {
                    continue;
                }
                else
                {
                    existingNames.Add(lowerNomScientifique);
                }

                var espece = new Espece
                {
                    NomScientifique = nomScientifique,
                    Habitat = (Habitat)int.Parse(line["HABITAT"] == "" ? "0" : line["HABITAT"]),
                    PresenceEnMetropole = (EtatPresence)(int)(line["FR"][0]),
                    IdInpn = int.Parse(line["CD_NOM"]),
                    Tags = new HashSet<Tag>(tags)
                };

                batch.AddLast(espece);
                if (batch.Count >= 1000)
                {
                    SaveBatchOfSpecies(ctx, batch);
                    batch = new LinkedList<Espece>();
                }
            }
            SaveBatchOfSpecies(ctx, batch);
        }

        static void SaveBatchOfSpecies(EspecesContext ctx, IReadOnlyCollection<Espece> batch)
        {
            ctx.Especes.AddRange(batch);
            ctx.SaveChanges();
            Console.WriteLine($"Saved {batch.Count} species.");
        }

        static IEnumerable<Tag> GetTags(IDictionary<string, Tag> allTags, params string[] tags)
        {
            foreach (var validTag in tags.Where(s => !string.IsNullOrWhiteSpace(s)))
            {
                if (allTags.TryGetValue(validTag, out Tag existing))
                {
                    yield return existing;
                }
                else
                {
                    throw new InvalidDataException($"Missing tag {validTag}");
                }
            }
        }

    }
}
