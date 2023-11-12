using Dat_api.DTOs;
using Dat_api.Entities;
using Dat_api.Helpers;

namespace Dat_api.Interfaces
{
	public interface IMessageRepository
	{
		void AddMessage(Message message);
		void DeleteMessage(Message message);
		Task<Message> GetMessage(int id);
		Task<PagedList<MessageDto>> GetMessagesForUser();
		Task<IEnumerable<MessageDto>> GetMessageThread(int currentUserId, int recipientId);
		Task<bool> SaveAllAsync();


	}
}
