using HostelManage.Application.Interfaces;
using System;
using System.Collections.Concurrent;

namespace HostelManage.Application.Services
{
    public class VerificationService : IVerificationService
    {
        private readonly EmailService _emailService;

        // ✅ Store codes per email (better than static)
        private static ConcurrentDictionary<string, (string Code, DateTime Time)> _codes
            = new();

        private readonly TimeSpan _expiry = TimeSpan.FromMinutes(5);

        public VerificationService(EmailService emailService)
        {
            _emailService = emailService;
        }

        public async Task SendCodeAsync(string email)
        {
            email = email.ToLower();

            var code = new Random().Next(100000, 999999).ToString();

            _codes[email] = (code, DateTime.UtcNow);

            var subject = "Your Email Verification Code";
            var message = $"Your verification code is: {code}";

            await _emailService.SendEmailAsync(email, subject, message);
        }

        public bool VerifyCode(string email, string code)
        {
            email = email.ToLower();

            if (!_codes.ContainsKey(email))
                return false;

            var (storedCode, time) = _codes[email];

            if (DateTime.UtcNow - time > _expiry)
            {
                _codes.TryRemove(email, out _);
                return false;
            }

            if (storedCode == code)
            {
                _codes.TryRemove(email, out _);
                return true;
            }

            return false;
        }
    }
}