using ActiveLog.Web.Models;

namespace ActiveLog.Web.Services;

public class TrainingValidator
{
    public bool Validate(Training training)
    {
        if (training.DauerMinuten <= 0)
            return false;

        if (string.IsNullOrWhiteSpace(training.Typ))
            return false;

        if (training.Datum > DateTime.Now.AddDays(1))
            return false;

        return true;
    }
}