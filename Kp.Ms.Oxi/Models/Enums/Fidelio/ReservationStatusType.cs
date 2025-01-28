using System.ComponentModel;

namespace Kp.Ms.Oxi.Models.Enums.Fidelio;

public enum ReservationStatusType
{
    [Description("Новая")]
    New = 0,

    [Description("Заехал")]
    CheckedIn = 1,

    [Description("Выехал")]
    CheckedOut = 2,

    [Description("Отменена")]
    Canceled = 3,

    [Description("Не явился")]
    NoShow = 4,

    [Description("Offer")]
    Offer = 5,
}