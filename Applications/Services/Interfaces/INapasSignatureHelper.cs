namespace Applications.Services.Interfaces
{
    public interface INapasSignatureHelper
    {
        string SignPayload(string payload);
        bool VerifySignature(string payload, string base64Signature);
    }
}
