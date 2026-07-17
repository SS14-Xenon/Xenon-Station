using Robust.Shared.Configuration;
using Content.Shared.Atmos;
using Robust.Shared;

namespace Content.Shared.ADT.CCVar;

[CVarDefs]
public sealed class ADTCCVars
{
    public static readonly CVarDef<string> HeadshotDomain =
        CVarDef.Create("ic.headshot_domain", "i.pinimg.com", CVar.SERVER | CVar.REPLICATED);
    public static readonly CVarDef<string> HeadshotUrl =
        CVarDef.Create("ic.headshot_url", "https://media.discordapp.net/attachments/1446603657255850085/", CVar.SERVER | CVar.REPLICATED);
}

