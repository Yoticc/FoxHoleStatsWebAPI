namespace FoxHoleStatsWebAPI;
public unsafe class StateReader : IDisposable
{
    public StateReader(WebClient client, Action<MapState> action)
    {
        const int BUFER_CAPACITY = 16384;
        const byte NEXT_CHAR = 0xA;
        const long START_MESSAGE_MASK = 0x3A61746164;

        using var stream = client.OpenRead("https://foxholestats.com:7551/channel/state");
        Span<byte> readBufferSpan = stackalloc byte[BUFER_CAPACITY];
        var readBuffer = (byte*)*(nint*)&readBufferSpan;
        var buffer = stackalloc byte[BUFER_CAPACITY];
        var currentBuffer = buffer;
        int byteRead;
        while (!shouldFinalize)
        {
            byteRead = stream.Read(readBufferSpan);
            for (var currentReadBuffer = readBuffer; currentReadBuffer < readBuffer + byteRead - 7; currentReadBuffer++)
                if ((*(long*)currentReadBuffer & 0xFFFFFFFFFF) == START_MESSAGE_MASK)
                    for (; currentReadBuffer < readBuffer + byteRead;)
                    {
                        *currentBuffer = *currentReadBuffer++;
                        if (*currentBuffer++ == NEXT_CHAR)
                        {
                            action(MapState.FromJson(JsonConvert.DeserializeObject<object[][]>(new string((sbyte*)buffer, 0, (int)(currentBuffer - buffer)))));
                            currentBuffer = buffer;
                            break;
                        }
                    }
        }
    }

    bool shouldFinalize;
    public void Dispose() => shouldFinalize = true;
    ~StateReader() => Dispose();
}