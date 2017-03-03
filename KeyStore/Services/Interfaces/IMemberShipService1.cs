using System.Collections.Generic;
using FidoU2f;

namespace KeyStore.Services.Interfaces
{
    public interface IMemberShipService1
    {
        bool AuthenticateUser(string userName, string deviceResponse);
        bool CompleteRegistration(string userName, string deviceResponse);
        void GenerateRandomFidoChallenge(string username);
        List<FidoUniversalTwoFactor> GenerateServerChallenges(string userName);
        bool IsUserRegistered(string userName);
        bool IsValidUserNameAndPassword(string userName, string password);
        bool SaveNewUser(string userName, string password);
    }
}