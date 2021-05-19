namespace ShopT.Models.LocalModels
{
    public class PointRegisterLocal
    {
        public PointRegister PointRegister { get; set; }

        public string Sign { get => PointRegister.UsedOrReceived ? "-" : "+"; }
        public string Value { get => $"{Sign} {PointRegister.Points}"; }
    }
}
