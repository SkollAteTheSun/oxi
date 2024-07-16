using System.ComponentModel;

namespace Kp.Ms.Oxi.Models.Enums;

public enum ReservationStatusType
{
    [Description("Новая")]
    New = 1,

    [Description("Оплачена")]
    Paid = 2,

    [Description("Подтверждена")]
    Approved = 3,

    [Description("Отменена")]
    Cancelled = 4,
}