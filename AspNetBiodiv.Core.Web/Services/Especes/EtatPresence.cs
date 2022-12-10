namespace AspNetBiodiv.Core.Web.Services.Especes;

public enum EtatPresence
{
    Présent = 'P',
    Endémique = 'E',
    SubEndémique = 'S',
    Cryptogène = 'C',
    Introduit = 'I',
    IntroduitEnvahissant = 'J',
    IntroduitNonEtabli = 'M',
    Occasionnel = 'B',
    Douteux = 'D',
    Absent = 'A',
    Disparu = 'W',
    Éteint = 'X',
    IntroduitEteint = 'Y',
    EndémiqueEteint = 'Z',
    MentionnéParErreur = 'Q',
}