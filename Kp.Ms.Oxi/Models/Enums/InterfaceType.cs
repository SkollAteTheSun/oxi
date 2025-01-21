using System.ComponentModel;

namespace Kp.Ms.Oxi.Models.Enums;

public enum InterfaceType
{
    [Description("OXI")]
    Oxi = 1,

    [Description("Fidelio")]
    Fidelio = 2
}