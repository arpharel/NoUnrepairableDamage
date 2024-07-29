using VoidManager.MPModChecks;

namespace NoUnrepairableDamage
{
    internal class VoidManagerPlugin : VoidManager.VoidPlugin
    {
        public override MultiplayerType MPType => MultiplayerType.Host;

        public override string Author => "Arpharel";

        public override string Description => "Allows you to repair your ship to full health";
    }
}
