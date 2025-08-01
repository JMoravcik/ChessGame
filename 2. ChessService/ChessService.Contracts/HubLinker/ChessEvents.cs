using ChessGame.ChessService.Contracts.HubMessages;
using ChessGame.ChessService.Contracts.MoveResults;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace ChessGame.ChessService.Contracts.HubLinker;

public class ChessEvents : IDisposable
{
    private ISubject<dynamic> _subject = new Subject<dynamic>();
    private Dictionary<object, List<IDisposable>> _subscriptions = new();

    public void Dispose()
    {
        foreach (var subList in _subscriptions)
            subList.Value.ForEach(sub => sub.Dispose());

        _subscriptions.Clear();
    }

    public void SubscribeToMoveResult(object subscriber, Action<MoveResult> moveResultFunc)
    {
        if (!_subscriptions.ContainsKey(subscriber))
            _subscriptions.Add(subscriber, new List<IDisposable>());

        _subscriptions[subscriber].Add(_subject.OfType<MoveResult>().Subscribe(moveResultFunc));
    }

    internal void PublishMoveResult(MoveResult moveResult)
        => _subject.OnNext(moveResult);

    public void Unsubscribe(object subscriber)
    {
        if (!_subscriptions.ContainsKey(subscriber))
            return;

        _subscriptions[subscriber].ForEach(sub => sub.Dispose());
        _subscriptions.Remove(subscriber);
    }
}
