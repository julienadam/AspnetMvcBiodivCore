namespace AspNetBiodiv.Core.Web.Entities
{
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
}