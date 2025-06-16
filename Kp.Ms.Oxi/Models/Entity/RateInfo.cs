namespace Kp.Ms.Oxi.Models.Entity;

public class RateInfo
{
    public byte Adults { get; set; }
    public byte Children { get; set; }
    public byte Babies { get; set; }
    public int BasePrice { get; set; }
    public long Amount { get; set; }
    public byte ExtraBed { get; set; }
    public byte Cradle { get; set; }
    //public int BabyPrice { get; set; }
    public int ChildrenPrice { get; set; }
    public int ChildrenPriceOnBeds { get; set; }
    public int AdultPriceOnBeds { get; set; }

    public RateInfo(byte adults, byte children, byte babies, int basePrice, long amount, byte extraBed, byte cradle,
                   int childrenPrice, int childrenPriceOnBeds, int adultPriceOnBeds)
    {
        Adults = adults;
        Children = children;
        Babies = babies;
        BasePrice = basePrice;
        Amount = amount;
        ExtraBed = extraBed;
        Cradle = cradle;
        //BabyPrice = babyPrice;
        ChildrenPrice = childrenPrice;
        ChildrenPriceOnBeds = childrenPriceOnBeds;
        AdultPriceOnBeds = adultPriceOnBeds;
    }
}