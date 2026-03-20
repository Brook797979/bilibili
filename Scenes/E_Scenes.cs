namespace NoBadConflicts
{
    enum E_Scenes
    {
        Start,
        Explanation,
        Selects,
        Center,
        Package,
        Store,
        Fighting,
        Finish
    }
    class Scene
    {
        public virtual void Run(GameMG gm)
        {
        }
    }
}
