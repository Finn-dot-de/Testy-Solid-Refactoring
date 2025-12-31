using ActiveLog.Web.Models;

namespace ActiveLog.Web.Services;

public class TrainingFactory
{
    public Training CreateTraining(string typ, DateTime datum, int dauerMinuten, string? notizen, Dictionary<string, object>? extraData = null)
    {
        Training training;

        switch (typ)
        {
            case "Cardio":
                training = new CardioTraining
                {
                    Typ = typ,
                    Datum = datum,
                    DauerMinuten = dauerMinuten,
                    Notizen = notizen,
                    Distanz = extraData?.ContainsKey("Distanz") == true ? Convert.ToDouble(extraData["Distanz"]) : 0,
                    DurchschnittsGeschwindigkeit = extraData?.ContainsKey("Geschwindigkeit") == true ? Convert.ToDouble(extraData["Geschwindigkeit"]) : 0
                };
                break;
            case "Kraft":
                training = new KraftTraining
                {
                    Typ = typ,
                    Datum = datum,
                    DauerMinuten = dauerMinuten,
                    Notizen = notizen,
                    GesamtGewicht = extraData?.ContainsKey("Gewicht") == true ? Convert.ToDouble(extraData["Gewicht"]) : 0,
                    AnzahlSaetze = extraData?.ContainsKey("Saetze") == true ? Convert.ToInt32(extraData["Saetze"]) : 0
                };
                break;
            case "Team":
                training = new TeamTraining
                {
                    Typ = typ,
                    Datum = datum,
                    DauerMinuten = dauerMinuten,
                    Notizen = notizen,
                    AnzahlTeilnehmer = extraData?.ContainsKey("Teilnehmer") == true ? Convert.ToInt32(extraData["Teilnehmer"]) : 0,
                    Mannschaft = extraData?.ContainsKey("Mannschaft") == true ? extraData["Mannschaft"].ToString() ?? "" : ""
                };
                break;
            default:
                training = new Training
                {
                    Typ = typ,
                    Datum = datum,
                    DauerMinuten = dauerMinuten,
                    Notizen = notizen
                };
                break;
        }

        return training;
    }
}