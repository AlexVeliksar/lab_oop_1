namespace Itmo.ObjectOrientedProgramming.Lab1.Results;

public sealed class RejectedContractResult : ContractResult
{
    public RejectedContractResult(RejectionReason reason)
    {
        Reason = reason;
    }

    public RejectionReason Reason { get; }
}
