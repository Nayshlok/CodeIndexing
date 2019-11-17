namespace CodeIndexing.Parser
{
    public enum MethodSearchState
    {
        Begin,
        ClosingParen,
        OpenParen,
        Name,
        Type,
    }
}
