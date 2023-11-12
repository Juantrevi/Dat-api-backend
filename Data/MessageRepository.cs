using Dat_api.DTOs;
using Dat_api.Entities;
using Dat_api.Helpers;
using Dat_api.Interfaces;

namespace Dat_api.Data
{
	public class MessageRepository : IMessageRepository
	{




		public void AddMessage(Message message)
		{
			throw new NotImplementedException();
		}

		public void DeleteMessage(Message message)
		{
			throw new NotImplementedException();
		}

		public Task<Message> GetMessage(int id)
		{
			throw new NotImplementedException();
		}

		public Task<PagedList<MessageDto>> GetMessagesForUser()
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<MessageDto>> GetMessageThread(int currentUserId, int recipientId)
		{
			throw new NotImplementedException();
		}

		public Task<bool> SaveAllAsync()
		{
			throw new NotImplementedException();
		}
	}
}
