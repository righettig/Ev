using System;

namespace Ev.Domain.Client.Behaviours.Core
{
    public class TribeNotFoundException : ArgumentException
    {
        public TribeNotFoundException(string message, string paramName) : base(message, paramName)
        {
        }
    }
}