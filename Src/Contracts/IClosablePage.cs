namespace Kfa.SubSystems
{
    public delegate void AnAction(object par = null);

    public interface IClosablePage
    {
        AnAction Close { get; set; }
    }
}