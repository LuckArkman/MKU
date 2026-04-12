using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace MKU.Scripts.Strucs
{
    public sealed class ExecutionQueue {

        private readonly ConcurrentQueue<Func<Task>> _queue = new ConcurrentQueue<Func<Task>>();

        public Task Completion { get; private set; }

        private async Task ProcessQueueAsync() {

            if (_queue.TryDequeue(out Func<Task> result)) {

                await result();
                if (0 != _queue.Count) await ProcessQueueAsync();

            }

        }
	
        public Task<T> Run<T>(Func<Task<T>> lambda) {

            var tcs = new TaskCompletionSource<T>();

            _queue.Enqueue(async () =>
            {
                // Execute the lambda and propagate the results to the Task returned from Run
                try {
                    T a = await lambda();
                    tcs.TrySetResult(a);
                } catch (OperationCanceledException ex) {
                    tcs.TrySetCanceled(ex.CancellationToken);
                } catch (Exception ex) {
                    tcs.TrySetException(ex);
                }
            });

            if (Completion == null || Completion.IsCompleted) {

                Completion = ProcessQueueAsync();

            }

            return tcs.Task;

        }

    }
}