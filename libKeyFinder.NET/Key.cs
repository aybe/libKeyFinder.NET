using JetBrains.Annotations;

namespace libKeyFinder.NET
{
    [PublicAPI]
    public enum Key
    {
        AMajor = 0,
        AMinor,
        BFlatMajor,
        BFlatMinor,
        BMajor,
        BMinor = 5,
        CMajor,
        CMinor,
        DFlatMajor,
        DFlatMinor,
        DMajor = 10,
        DMinor,
        EFlatMajor,
        EFlatMinor,
        EMajor,
        EMinor = 15,
        FMajor,
        FMinor,
        GFlatMajor,
        GFlatMinor,
        GMajor = 20,
        GMinor,
        AFlatMajor,
        AFlatMinor,
        Silence = 24
    }
}