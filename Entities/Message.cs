namespace Dat_api.Entities
{
	public class Message
	{
		public int Id { get; set; }
		public int senderId { get; set; }
		public string senderUsername { get; set; }
		public AppUser sender { get; set; }
		public int RecipientId { get; set; }
		public string RecipientUsername { get; set; }
		public AppUser Recipient { get; set; }
		public string Content { get; set; }
		public DateTime? DateRead { get; set; }
		public DateTime MessageSent { get; set; } = DateTime.UtcNow;
		public bool SenderDeleted { get; set; }
		public bool RecipientDeleted { get; set; }

	}
}
