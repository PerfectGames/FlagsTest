namespace FlagsTest
{
    public static class GameExtentions
    {
        public static bool InTeam (this TeamObject thisObject, TeamObject other)
        {
            return thisObject.Team == other.Team;
        }
    }
}
