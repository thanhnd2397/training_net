using Microsoft.Extensions.Localization;
using Training.Application.Common;
using Training.Application.Common.Interfaces;

namespace Training.Infrastructure.Services;

    public class MessageService : IMessageService
    {
        private readonly IStringLocalizer _localizer;

        public MessageService(IStringLocalizerFactory factory)
        {
            // Tạo localizer trỏ về Resource trong WebApi
            _localizer = factory.Create("Messages", "Training.WebApi");
        }

        public string GetMessage(string code)
        {
            var message = _localizer[code];
            return string.IsNullOrWhiteSpace(message) ? code : message;
        }
    }
