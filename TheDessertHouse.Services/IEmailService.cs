using System;
using TheDessertHouse.Domain;

namespace TheDessertHouse.Services.ControllerTests
{
    public interface IEmailService
    {
        void SendNewsLetter(object msgObj);
    }
}