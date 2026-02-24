using Robust.Shared.Configuration;
using Content.Shared.Atmos;
using Robust.Shared;

namespace Content.Shared.ADT.CCVar;

[CVarDefs]
public sealed class ADTCCVars
{
    /*
    * Barks
    */
    public static readonly CVarDef<string> HeadshotUrl =
        CVarDef.Create("ic.headshot_url", "https://media.discordapp.net/attachments/1446603657255850085/", CVar.SERVER | CVar.REPLICATED);
}

