using Features.MessagesFeature.Commands;
using FluentValidation;

namespace Features.MessagesFeature.Validators
{
    public class SendMessageCommandValidator : AbstractValidator<SendMessageCommand>
    {
        private readonly string[] _allowedImageExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
        private readonly string[] _allowedFileExtensions = { ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".txt" };
        private readonly string[] _allowedVoiceExtensions = { ".mp3", ".wav", ".ogg", ".m4a" };
        
        // 5 MB Max Size
        private const int MaxFileSizeInBytes = 5 * 1024 * 1024;

        public SendMessageCommandValidator()
        {
            RuleFor(x => x.ConversationId)
                .NotEmpty().WithMessage("ConversationId is required.");

            // Either Content or MediaFile must be provided
            RuleFor(x => x)
                .Must(x => !string.IsNullOrWhiteSpace(x.Content) || x.MediaFile != null)
                .WithMessage("Message content or media file must be provided.");

            RuleFor(x => x.MediaFile)
                .Must((command, file) => file == null || file.Length <= MaxFileSizeInBytes)
                .WithMessage("File size must not exceed 5 MB.")
                .Must((command, file) => file == null || IsAllowedExtension(file.FileName, command.MediaType))
                .WithMessage("File extension is not allowed for this media type.");
        }

        private bool IsAllowedExtension(string fileName, string mediaType)
        {
            var ext = System.IO.Path.GetExtension(fileName).ToLower();
            return mediaType switch
            {
                "Image" => System.Array.Exists(_allowedImageExtensions, e => e == ext),
                "File" => System.Array.Exists(_allowedFileExtensions, e => e == ext),
                "Voice" => System.Array.Exists(_allowedVoiceExtensions, e => e == ext),
                _ => false
            };
        }
    }

    public class EditMessageCommandValidator : AbstractValidator<EditMessageCommand>
    {
        public EditMessageCommandValidator()
        {
            RuleFor(x => x.MessageId)
                .NotEmpty().WithMessage("MessageId is required.");

            RuleFor(x => x.NewContent)
                .NotEmpty().WithMessage("New content cannot be empty.");
        }
    }

    public class DeleteMessageCommandValidator : AbstractValidator<DeleteMessageCommand>
    {
        public DeleteMessageCommandValidator()
        {
            RuleFor(x => x.MessageId)
                .NotEmpty().WithMessage("MessageId is required.");
        }
    }
}
