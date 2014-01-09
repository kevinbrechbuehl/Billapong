namespace Billapong.Core.Client
{
    public  interface ICallback<TCallback>
    {
        TCallback Callback { get; }
    }
}
