namespace AspectCentral.Abstractions;

/// <summary>Classifies the shape of an intercepted method for aspect dispatch.</summary>
public enum MethodTypeOptions
{
    /// <summary>A synchronous method that returns <see cref="void" />.</summary>
    SyncAction,

    /// <summary>A synchronous method that returns a value.</summary>
    SyncFunction,

    /// <summary>An asynchronous method that returns <see cref="System.Threading.Tasks.Task{TResult}" /> or <see cref="System.Threading.Tasks.ValueTask{TResult}" />.</summary>
    AsyncFunction,

    /// <summary>An asynchronous method that returns <see cref="System.Threading.Tasks.Task" /> or <see cref="System.Threading.Tasks.ValueTask" />.</summary>
    AsyncAction
}
