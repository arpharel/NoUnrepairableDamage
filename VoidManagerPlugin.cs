using VoidManager.MPModChecks;

namespace NoUnrepairableDamage
{
    internal class VoidManagerPlugin : VoidManager.VoidPlugin
    {
        public override MultiplayerType MPType => MultiplayerType.Host;

        public override string Author => "Arpharel, Miranoff";

        public override string Description => "Allows you to repair your ship to full health. Hull Breach is required to be able to repair to full.";
    }
}
