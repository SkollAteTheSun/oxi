using System.ComponentModel;

namespace Kp.Ms.Oxi.Models.Enums;

public enum ProfileType
{
    [Description("Контакт")]
    Contact,

    [Description("Тревел агент")]
    Travel,

    [Description("Компания")]
    Company,

    [Description("Группа")]
    Group,

    [Description("Источник")]
    Wholesaler,
}