using PdfSharp.Fonts;
using System.Reflection;

namespace CashFlow.Application.Fonts;

public class CashFlowFontResolver : IFontResolver
{
    public byte[]? GetFont(string faceName)
    {
        Stream? stream = ReadFontFile(faceName);

        if (stream is null)
            stream = ReadFontFile(FontHelper.DEFAULT_FONT);

        int length = (int)stream!.Length;
        byte[] data = new byte[length];

        stream.ReadExactly(buffer: data, offset: 0, count: length);

        stream.Dispose();

        return data;
    }

    public FontResolverInfo? ResolveTypeface(string familyName, bool bold, bool italic)
    {
        return new FontResolverInfo(familyName);
    }

    private Stream? ReadFontFile(string faceName)
    {
        Assembly assembly = Assembly.GetExecutingAssembly();

        return assembly.GetManifestResourceStream($"CashFlow.Application.Fonts.{faceName}.ttf");
    }
}
