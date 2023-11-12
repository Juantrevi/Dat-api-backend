using Dat_api.Entities;

namespace Dat_api.DTOs
{
	public class MessageDto
	{
		public int Id { get; set; }
		public int senderId { get; set; }
		public string senderUsername { get; set; }
		public string SenderPhotoUrl { get; set; }
		public int RecipientId { get; set; }
		public string RecipientUsername { get; set; }
		public string RecipientPhotoUrl { get; set; }
		public string Content { get; set; }
		public DateTime? DateRead { get; set; }
		public DateTime MessageSent { get; set; };

	}
}
