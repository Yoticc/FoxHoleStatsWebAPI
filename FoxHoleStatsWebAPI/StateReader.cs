namespace FoxHoleStatsWebAPI;
public unsafe class StateReader : IDisposable
{
    public StateReader(Action<MapState> action) : this(new(), action) { }
    public StateReader(WebClient client, Action<MapState> action)
    {
        const int BUFFER_CAPACITY = 16384;
        const byte NEXT_CHAR = 0xA;
        const long START_MESSAGE_MASK = 0x3A61746164;

        using var stream = client.OpenRead("https://foxholestats.com:7551/channel/state");
        Span<byte> readBufferSpan = stackalloc byte[BUFFER_CAPACITY];
        var readBuffer = (byte*)*(nint*)&readBufferSpan;
        var buffer = stackalloc byte[BUFFER_CAPACITY];
        var currentBuffer = buffer;
        int byteRead;
        var bufferStarted = false;
        while (!shouldFinalize && (byteRead = stream.Read(readBufferSpan)) != -1)
            for (var currentReadBuffer = readBuffer; currentReadBuffer < readBuffer + byteRead; currentReadBuffer++)
                if (bufferStarted || (bufferStarted = (*(long*)currentReadBuffer & 0xFFFFFFFFFF) == START_MESSAGE_MASK))
                    for (; currentReadBuffer < readBuffer + byteRead;)
                        if ((*currentBuffer++ = *currentReadBuffer++) == NEXT_CHAR)
                        {
                            action(MapState.FromJson(JsonConvert.DeserializeObject<object[]>(new string((sbyte*)buffer, 6, (int)(currentBuffer - buffer)))));
                            bufferStarted = false;
                            currentBuffer = buffer;
                            break;
                        }
    }

    bool shouldFinalize;
    public void Dispose() => shouldFinalize = true;
    ~StateReader() => Dispose();
}